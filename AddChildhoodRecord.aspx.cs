using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.IO;

namespace ZimVaxSync
{
    public partial class AddChildhoodRecord : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Zimvaxsync"].ConnectionString);

        private int GetChildID()
        {
            if (Session["ChildID"] != null && int.TryParse(Session["ChildID"].ToString(), out int childId))
            {
                return childId;
            }
            throw new Exception("Child ID not found in session.");
        }

        private string connectionString = ConfigurationManager.ConnectionStrings["Zimvaxsync"].ConnectionString;

        public class WHOStandardEntry
        {
            public int AgeInMonths { get; set; }
            public double Minus3SD { get; set; }
            public double Minus2SD { get; set; }
            public double Minus1SD { get; set; }
            public double Median { get; set; }
            public double Plus1SD { get; set; }
            public double Plus2SD { get; set; }
            public double Plus3SD { get; set; }
        }

        private List<WHOStandardEntry> ReadWHOStandard(string filePath)
        {
            var list = new List<WHOStandardEntry>();
            using (var reader = new StreamReader(filePath))
            {
                reader.ReadLine(); // Skip header
                while (!reader.EndOfStream)
                {
                    var parts = reader.ReadLine().Split(',');
                    list.Add(new WHOStandardEntry
                    {
                        AgeInMonths = int.Parse(parts[0]),
                        Minus3SD = double.Parse(parts[2]),
                        Minus2SD = double.Parse(parts[3]),
                        Minus1SD = double.Parse(parts[4]),
                        Median = double.Parse(parts[5]),
                        Plus1SD = double.Parse(parts[6]),
                        Plus2SD = double.Parse(parts[7]),
                        Plus3SD = double.Parse(parts[8])
                    });
                }
            }
            return list;
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["childId"] != null &&
                    int.TryParse(Request.QueryString["childId"], out int childId))
                {
                    Session["ChildID"] = childId;
                    LoadChildInfo(childId);
                    LoadExistingVaccinations(childId);
                    string gender = ViewState["Gender"]?.ToString() ?? "Unknown"; // Default if null
                    LoadGrowthCharts(childId, gender);
                    LoadInfantFeeding(childId);
                    LoadInfantCare(childId);

                }
                else
                {
                    lblMessage.Text = "Child ID not provided or invalid.";
                }
            }
        }

        private void LoadChildInfo(int childId)
        {
            string query = @"SELECT cp.FullName AS ChildName, cp.Gender, cp.DateOfBirth, cp.IDNumber,
                            cp.RelationshipToChild, cg.FullName AS CaregiverName, cg.Address, cg.PhoneNumber
                            FROM ChildProfile cp
                            INNER JOIN Caregiver cg ON cp.CaregiverId = cg.CaregiverId
                            WHERE cp.ChildId = @ChildId";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@ChildId", childId);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    lblChildInfo.Text = "Child Name: " + reader["ChildName"];
                    lblDOB.Text = "Date of Birth: " + Convert.ToDateTime(reader["DateOfBirth"]).ToString("dd MMM yyyy");
                    lblIDNumber.Text = "ID Number: " + reader["IDNumber"];
                    lblCaregiverName.Text = "Caregiver Name: " + reader["CaregiverName"];
                    lblRelationship.Text = "Relationship to Child: " + reader["RelationshipToChild"];
                    lblCaregiverAddress.Text = "Address: " + reader["Address"];
                    lblCaregiverPhone.Text = "Phone Number: " + reader["PhoneNumber"];
                    ViewState["Gender"] = reader["Gender"].ToString(); // ✅ Store gender
                }
                else
                {
                    lblMessage.Text = "Child record not found.";
                }
            }
        }

        private void LoadExistingVaccinations(int childId)
        {
            var vaccineMap = new Dictionary<string, string>
    {
        { "BCG", "BCG" }, { "HEPB", "HEPB" }, { "DTP-Hib-HEPB1", "DTP1" },
        { "OPV1", "OPV1" }, { "ROTAVIRUS 1", "ROTA1" }, { "NEUMOCOCCAL1", "NEU1" },
        { "DTP-Hib-HEPB 1.2", "DTP2" }, { "OPV2", "OPV2" }, { "ROTAVIRUS 2", "ROTA2" },
        { "NEUMOCOCCAL2", "NEU2" }, { "DTP-Hib-HEPB 1.3", "DTP3" }, { "NEUMOCOCCAL3", "NEU3" },
        { "OPV3", "OPV3" }, { "IPV", "IPV" }, { "MEASLES RUBELLA 1", "MR1" },
        { "TYPHOID VACCINE", "Typhoid" }, { "OPV", "OPVBooster" }, { "DTP", "DTPBooster" },
        { "MEASLES RUBELLA 2", "MR2" }, { "Td1", "Td1" }, { "Td2", "Td2" },
        { "HPV 1", "HPV1" }, { "HPV 2", "HPV2" }
    };

            string query = @"SELECT VaccineName, DateAdministered, NextDoseDate, BatchNumber, Status 
                     FROM ChildVaccinationRecords WHERE ChildID = @ChildID";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@ChildID", childId);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string vaccine = reader["VaccineName"].ToString();
                    if (!vaccineMap.ContainsKey(vaccine)) continue;

                    string key = vaccineMap[vaccine];
                    var txtDate = FindControl($"txtDate{key}") as TextBox;
                    var txtNext = FindControl($"txtNext{key}") as TextBox;
                    var txtBatch = FindControl($"txtBatch{key}") as TextBox;
                    var txtStatus = FindControl($"txtStatus{key}") as TextBox; // <- Status textbox

                    if (txtDate != null && reader["DateAdministered"] != DBNull.Value)
                        txtDate.Text = Convert.ToDateTime(reader["DateAdministered"]).ToString("yyyy-MM-dd");

                    if (txtNext != null && reader["NextDoseDate"] != DBNull.Value)
                        txtNext.Text = Convert.ToDateTime(reader["NextDoseDate"]).ToString("yyyy-MM-dd");

                    if (txtBatch != null)
                        txtBatch.Text = reader["BatchNumber"].ToString();

                    if (txtStatus != null)
                        txtStatus.Text = reader["Status"].ToString();
                }

                reader.Close();
                con.Close();
            }
        }



      protected async void btnSubmit_Click(object sender, EventArgs e)
{
    int childID = GetChildID();
    int savedCount = 0;

    var vaccines = new List<(string AgeGroup, string VaccineName, string Key)>
    {
        ("Birth", "BCG", "BCG"),
        ("Birth", "HEPB", "HEPB"),
        ("6 Weeks", "DTP-Hib-HEPB1", "DTP1"),
        ("6 Weeks", "OPV1", "OPV1"),
        ("6 Weeks", "ROTAVIRUS 1", "ROTA1"),
        ("6 Weeks", "NEUMOCOCCAL1", "NEU1"),
        ("10 Weeks", "DTP-Hib-HEPB 1.2", "DTP2"),
        ("10 Weeks", "OPV2", "OPV2"),
        ("10 Weeks", "ROTAVIRUS 2", "ROTA2"),
        ("10 Weeks", "NEUMOCOCCAL2", "NEU2"),
        ("14 Weeks", "DTP-Hib-HEPB 1.3", "DTP3"),
        ("14 Weeks", "NEUMOCOCCAL3", "NEU3"),
        ("14 Weeks", "OPV3", "OPV3"),
        ("14 Weeks", "IPV", "IPV"),
        ("9 Months", "MEASLES RUBELLA 1", "MR1"),
        ("9 Months", "TYPHOID VACCINE", "Typhoid"),
        ("18 Months", "OPV", "OPVBooster"),
        ("18 Months", "DTP", "DTPBooster"),
        ("18 Months", "MEASLES RUBELLA 2", "MR2"),
        ("5 Years", "Td1", "Td1"),
        ("10 Years", "Td2", "Td2"),
        ("Grade 5 (10 Years)", "HPV 1", "HPV1"),
        ("Grade 6 (11 Years)", "HPV 2", "HPV2")
    };

    foreach (var (ageGroup, vaccineName, key) in vaccines)
    {
        var txtDate = FindControl($"txtDate{key}") as TextBox;
        var txtNext = FindControl($"txtNext{key}") as TextBox;
        var txtBatch = FindControl($"txtBatch{key}") as TextBox;

        if (txtDate == null || txtNext == null || txtBatch == null)
            continue;

        if ((vaccineName == "HPV 1" || vaccineName == "HPV 2") &&
            !string.Equals(ViewState["Gender"]?.ToString(), "Female", StringComparison.OrdinalIgnoreCase))
        {
            continue;
        }

        if (string.IsNullOrWhiteSpace(txtDate.Text) &&
            string.IsNullOrWhiteSpace(txtNext.Text) &&
            string.IsNullOrWhiteSpace(txtBatch.Text))
            continue;

        await SaveVaccine(childID, ageGroup, vaccineName, txtDate.Text, txtNext.Text, txtBatch.Text);
        savedCount++;
    }

    lblMessage.Text = savedCount > 0
        ? "Vaccination records saved successfully."
        : "No vaccination records entered.";
}

private async Task SaveVaccine(int childID, string ageGroup, string vaccineName, string dateAdministered, string nextDoseDate, string batchNumber)
{
    string status = CalculateStatus(dateAdministered, nextDoseDate, batchNumber);

    if (vaccineName == "BCG")
    {
        nextDoseDate = null;
    }

    string query = @"IF EXISTS (SELECT 1 FROM ChildVaccinationRecords 
        WHERE ChildID = @ChildID AND VaccineName = @VaccineName)
    BEGIN
        UPDATE ChildVaccinationRecords
        SET AgeGroup = @AgeGroup,
            Status = @Status,
            DateAdministered = @DateAdministered,
            NextDoseDate = @NextDoseDate,
            BatchNumber = @BatchNumber
        WHERE ChildID = @ChildID AND VaccineName = @VaccineName
    END
    ELSE
    BEGIN
        INSERT INTO ChildVaccinationRecords 
        (ChildID, AgeGroup, VaccineName, Status, DateAdministered, NextDoseDate, BatchNumber) 
        VALUES (@ChildID, @AgeGroup, @VaccineName, @Status, @DateAdministered, @NextDoseDate, @BatchNumber)
    END";

    using (SqlCommand cmd = new SqlCommand(query, con))
    {
        cmd.Parameters.AddWithValue("@ChildID", childID);
        cmd.Parameters.AddWithValue("@AgeGroup", ageGroup);
        cmd.Parameters.AddWithValue("@VaccineName", vaccineName);
        cmd.Parameters.AddWithValue("@BatchNumber", (object)batchNumber ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Status", status);
        cmd.Parameters.AddWithValue("@DateAdministered", string.IsNullOrWhiteSpace(dateAdministered) ? DBNull.Value : (object)Convert.ToDateTime(dateAdministered));
        cmd.Parameters.AddWithValue("@NextDoseDate", string.IsNullOrWhiteSpace(nextDoseDate) ? DBNull.Value : (object)Convert.ToDateTime(nextDoseDate));

        con.Open();
        await cmd.ExecuteNonQueryAsync();
        con.Close();
    }

    if (status == "Vaccinated" && !string.IsNullOrWhiteSpace(nextDoseDate) && vaccineName != "BCG")
    {
        string scheduleQuery = @"IF NOT EXISTS (SELECT 1 FROM VaccinationSchedule 
            WHERE ChildID = @ChildID AND VaccineName = @VaccineName)
            BEGIN
                INSERT INTO VaccinationSchedule (ChildID, VaccineName, DueDate, Status)
                VALUES (@ChildID, @VaccineName, @DueDate, 'Pending')
            END";

        using (SqlCommand schedCmd = new SqlCommand(scheduleQuery, con))
        {
            schedCmd.Parameters.AddWithValue("@ChildID", childID);
            schedCmd.Parameters.AddWithValue("@VaccineName", vaccineName);
            schedCmd.Parameters.AddWithValue("@DueDate", Convert.ToDateTime(nextDoseDate));

            con.Open();
            await schedCmd.ExecuteNonQueryAsync();
            con.Close();
        }
    }

    // Send SMS if needed
    if (!string.IsNullOrWhiteSpace(nextDoseDate) && HasMultipleDoses(vaccineName))
    {
        string childName = GetChildName(childID);
        string caregiverPhone = GetCaregiverPhone(childID);
        string message = $"Dear caregiver, thank you for getting your child {childName} vaccinated for {vaccineName}. Their next dose is on {nextDoseDate}. Regards, ZimVaxSync Team.";

        try
        {
            var smsSender = new TelerivetSmsSender();
            await smsSender.SendSmsAsync(caregiverPhone, message);
        }
        catch (Exception ex)
        {
            // Log error somewhere
            Console.WriteLine("Failed to send SMS: " + ex.Message);
        }
    }
}


       
        
        private bool HasMultipleDoses(string vaccineName)
        {
            // Simplified example: adjust based on your schedule or database
            var multiDoseVaccines = new List<string>
    {
        "DTP-Hib-HEPB1", "OPV1", "ROTAVIRUS 1", "NEUMOCOCCAL1", "MEASLES RUBELLA 1", "TYPHOID", "Td1"
    };
            return multiDoseVaccines.Contains(vaccineName);
        }

        private string GetChildName(int childID)
        {
            string name = "";
            string query = "SELECT FullName FROM ChildProfile WHERE ID = @ChildID";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@ChildID", childID);
                con.Open();
                var result = cmd.ExecuteScalar();
                if (result != null) name = result.ToString();
                con.Close();
            }
            return name;
        }

        private string GetCaregiverPhone(int childID)
        {
            string phone = "";
            string query = @"SELECT c.PhoneNumber FROM Caregiver c
                     JOIN ChildProfile cp ON c.CaregiverID = cp.CaregiverID
                     WHERE cp.ID = @ChildID";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@ChildID", childID);
                con.Open();
                var result = cmd.ExecuteScalar();
                if (result != null) phone = result.ToString();
                con.Close();
            }
            return phone;
        }


        protected void btnSaveAll_Click(object sender, EventArgs e)
        {
            int childID = GetChildID();

            // Milestone list using full type declaration and no target-typed new()
            var milestones = new List<Tuple<string, CheckBox, TextBox>>();
            milestones.Add(new Tuple<string, CheckBox, TextBox>("Smiles", chkSmiles, txtDateSmiles));
            milestones.Add(new Tuple<string, CheckBox, TextBox>("HoldsHead", chkHoldsHead, txtDateHoldsHead));
            milestones.Add(new Tuple<string, CheckBox, TextBox>("Sits", chkSits, txtDateSits));
            milestones.Add(new Tuple<string, CheckBox, TextBox>("Crawls", chkCrawls, txtDateCrawls));
            milestones.Add(new Tuple<string, CheckBox, TextBox>("FirstWords", chkFirstWords, txtDateFirstWords));
            milestones.Add(new Tuple<string, CheckBox, TextBox>("Social", chkSocial, txtDateSocial));

            foreach (var item in milestones)
            {
                string milestone = item.Item1;
                CheckBox checkbox = item.Item2;
                TextBox dateBox = item.Item3;

                if (checkbox.Checked)
                {
                    string query = @"INSERT INTO DevelopmentMilestones 
                             (ChildId, Milestone, Achieved, DateRecorded)  
                             VALUES (@ChildId, @Milestone, @Achieved, @DateRecorded)";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ChildId", childID);
                        cmd.Parameters.AddWithValue("@Milestone", milestone);
                        cmd.Parameters.AddWithValue("@Achieved", true);
                        cmd.Parameters.AddWithValue("@DateRecorded", DateTime.Parse(dateBox.Text));
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }

                    lblMessage.Text = "Milestones saved successfully.";
                }
            }

            // Save Vitamin A Doses
            string[] ageGroups = { "6_11", "12_23", "24_35", "36_47", "48_59" };
            string[] doseTypes = { "First", "Second" };

            foreach (string doseType in doseTypes)
            {
                foreach (string ageGroup in ageGroups)
                {
                    string batchId = $"txt{doseType}Dose_{ageGroup}_Batch";
                    string dateId = $"txt{doseType}Dose_{ageGroup}_Date";

                    TextBox batchBox = FindControl(batchId) as TextBox;
                    TextBox dateBox = FindControl(dateId) as TextBox;

                    if (batchBox != null && dateBox != null && !string.IsNullOrWhiteSpace(dateBox.Text))
                    {
                        string query = @"INSERT INTO VitaminADoses 
            (ChildID, DoseType, AgeGroup, BatchNo, DateGiven) 
            VALUES (@ChildID, @DoseType, @AgeGroup, @BatchNo, @DateGiven)";

                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@ChildID", childID);
                            cmd.Parameters.AddWithValue("@DoseType", doseType);
                            cmd.Parameters.AddWithValue("@AgeGroup", ageGroup.Replace("_", "-"));
                            cmd.Parameters.AddWithValue("@BatchNo", batchBox.Text.Trim());
                            cmd.Parameters.AddWithValue("@DateGiven", DateTime.Parse(dateBox.Text));

                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
            }

            // Save Deworming Doses
            for (int i = 1; i <= 5; i++)
            {
                TextBox dateBox = FindControl($"txtDewormingDate{i}") as TextBox;
                TextBox batchBox = FindControl($"txtDewormingBatch{i}") as TextBox;

                if (dateBox != null && batchBox != null && !string.IsNullOrWhiteSpace(dateBox.Text))
                {
                    string query = @"INSERT INTO DewormingDoses 
        (ChildID, DoseNumber, BatchNo, DateGiven) 
        VALUES (@ChildID, @DoseNumber, @BatchNo, @DateGiven)";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ChildID", childID);
                        cmd.Parameters.AddWithValue("@DoseNumber", i);
                        cmd.Parameters.AddWithValue("@BatchNo", batchBox.Text.Trim());
                        cmd.Parameters.AddWithValue("@DateGiven", DateTime.Parse(dateBox.Text));

                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }

            // Save Doctor Remarks
            string remarkQuery = "UPDATE ChildVaccinationRecords SET DoctorRemarks = @Remarks WHERE ChildID = @ChildID";
            using (SqlCommand cmd = new SqlCommand(remarkQuery, con))
            {
                cmd.Parameters.AddWithValue("@Remarks", txtDoctorRemarks.Text);
                cmd.Parameters.AddWithValue("@ChildID", childID);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            lblMessage.Text = "Childhood record saved successfully.";
        }
        private void LoadMilestones(int childID)
        {
            string query = "SELECT Milestone, DateRecorded FROM DevelopmentMilestones WHERE ChildId = @ChildId";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@ChildId", childID);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string milestone = reader["Milestone"].ToString();
                    string date = Convert.ToDateTime(reader["DateRecorded"]).ToString("yyyy-MM-dd");

                    switch (milestone)
                    {
                        case "Smiles":
                            chkSmiles.Checked = true;
                            txtDateSmiles.Text = date;
                            break;
                        case "HoldsHead":
                            chkHoldsHead.Checked = true;
                            txtDateHoldsHead.Text = date;
                            break;
                        case "Sits":
                            chkSits.Checked = true;
                            txtDateSits.Text = date;
                            break;
                        case "Crawls":
                            chkCrawls.Checked = true;
                            txtDateCrawls.Text = date;
                            break;
                        case "FirstWords":
                            chkFirstWords.Checked = true;
                            txtDateFirstWords.Text = date;
                            break;
                        case "Social":
                            chkSocial.Checked = true;
                            txtDateSocial.Text = date;
                            break;
                    }
                }
                reader.Close();
                con.Close();
            }
        }

        protected void btnSaveGrowth_Click(object sender, EventArgs e)
        {
            int childId = GetChildID();
            if (int.TryParse(txtAgeInMonths.Text, out int age) &&
                decimal.TryParse(txtHeight.Text, out decimal height) &&
                decimal.TryParse(txtWeight.Text, out decimal weight))
            {
                string query = @"INSERT INTO GrowthMonitoring (ChildId, AgeInMonths, Height, Weight)
                         VALUES (@ChildId, @Age, @Height, @Weight)";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ChildId", childId);
                    cmd.Parameters.AddWithValue("@Age", age);
                    cmd.Parameters.AddWithValue("@Height", height);
                    cmd.Parameters.AddWithValue("@Weight", weight);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

                lblMessage.Text = "Growth entry saved successfully.";
                string gender = ViewState["Gender"]?.ToString() ?? "Unknown"; // Default if null
                LoadGrowthCharts(childId, gender);

            }
            else
            {
                lblMessage.Text = "Please enter valid numbers for age, height, and weight.";
            }
        }

        protected void btnClearGrowth_Click(object sender, EventArgs e)
        {
            txtAgeInMonths.Text = "";
            txtHeight.Text = "";
            txtWeight.Text = "";
            lblMessage.Text = "";
        }

        private void LoadGrowthCharts(int childID, string gender)
        {
            // Determine which WHO tables to use
            string tableSuffix = gender.ToLower() == "female" ? "Girls" : "Boys";
            DataTable dtHeight = GetWHOData("WHOHeightForAge" + tableSuffix);
            DataTable dtWeight = GetWHOData("WHOWeightForAge" + tableSuffix);

            // Plot WHO standards
            PlotZScoreLines(dtHeight, chartHeightForAge, "Height (cm)");
            PlotZScoreLines(dtWeight, chartWeightForAge, "Weight (kg)");

            // Plot child's growth data
            string query = "SELECT AgeInMonths, Height, Weight FROM GrowthMonitoring WHERE ChildId = @ChildId ORDER BY AgeInMonths";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@ChildId", childID);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                Series heightSeries = chartHeightForAge.Series.FindByName("ChildHeight") ?? new Series("ChildHeight") { ChartType = SeriesChartType.Line, BorderWidth = 3, Color = Color.Black };
                if (chartHeightForAge.Series.FindByName("ChildHeight") == null)
                    chartHeightForAge.Series.Add(heightSeries);
                else
                    heightSeries.Points.Clear();

                Series weightSeries = chartWeightForAge.Series.FindByName("ChildWeight") ?? new Series("ChildWeight") { ChartType = SeriesChartType.Line, BorderWidth = 3, Color = Color.Black };
                if (chartWeightForAge.Series.FindByName("ChildWeight") == null)
                    chartWeightForAge.Series.Add(weightSeries);
                else
                    weightSeries.Points.Clear();

                while (reader.Read())
                {
                    int age = Convert.ToInt32(reader["AgeInMonths"]);
                    if (reader["Height"] != DBNull.Value)
                        heightSeries.Points.AddXY(age, Convert.ToDecimal(reader["Height"]));
                    if (reader["Weight"] != DBNull.Value)
                        weightSeries.Points.AddXY(age, Convert.ToDecimal(reader["Weight"]));
                }
                reader.Close();
                con.Close();
            }
        }

        private void PlotZScoreLines(DataTable dt, Chart chart, string yAxisTitle)
        {
            string[] zProperties = { "Minus3SD", "Minus2SD", "Median", "Plus2SD", "Plus3SD" };
            string[] zLabels = chart.ID.Contains("Weight") ?
                new[] { "-3 Severe Underweight", "-2 Moderate Underweight", "Normal", "+2 Possible Overweight", "+3 Check Weight for Height" } :
                new[] { "-3 Severe Stunted", "-2 Moderate Stunted", "Normal", "+2 Normal", "+3 Possible Very Tall" };

            foreach (var label in zLabels)
            {
                if (chart.Series.FindByName(label) != null)
                    chart.Series.Remove(chart.Series[label]);
            }

            for (int i = 0; i < zProperties.Length; i++)
            {
                Series series = new Series(zLabels[i])
                {
                    ChartType = SeriesChartType.Line,
                    BorderWidth = zLabels[i] == "Normal" ? 3 : 2,
                    Color = zLabels[i].Contains("+3") || zLabels[i].Contains("-3") ? Color.Red :
                            zLabels[i].Contains("+2") || zLabels[i].Contains("-2") ? Color.Orange : Color.Green,
                    IsVisibleInLegend = true
                };

                foreach (DataRow row in dt.Rows)
                {
                    int age = Convert.ToInt32(row["AgeInMonths"]);
                    double value = Convert.ToDouble(row[zProperties[i]]);
                    series.Points.AddXY(age, value);
                }

                chart.Series.Add(series);
            }

            chart.ChartAreas[0].AxisY.Title = yAxisTitle;
            chart.ChartAreas[0].AxisY2.Enabled = AxisEnabled.True;
        }

        private string CalculateStatus(string dateAdministered, string nextDoseDate, string batchNumber)
        {
            if (!string.IsNullOrWhiteSpace(dateAdministered) && !string.IsNullOrWhiteSpace(batchNumber))
            {
                return "Vaccinated";
            }

            if (string.IsNullOrWhiteSpace(dateAdministered) && DateTime.TryParse(nextDoseDate, out DateTime nextDate))
            {
                return DateTime.Now.Date > nextDate.Date ? "Missed" : "Pending";
            }

            return "Pending";
        }


        private DataTable GetWHOData(string tableName)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Zimvaxsync"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SELECT * FROM " + tableName, con))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
        private void SaveInfantFeeding(int childId)
        {
            var feedingEntries = new List<(string AgeLabel, string FeedingType, TextBox Input)>
    {
        ("Birth", "Breast milk only", txtBirth1),
        ("6W", "Breast milk only", txt6W1),
        ("10W", "Breast milk only", txt10W1),
        ("14W", "Breast milk only", txt14W1),
        ("5M", "Breast milk only", txt5M1),
        ("Birth", "", txtBirth2),
        ("6W", "", txt6W2),
        ("10W", "", txt10W2),
        ("14W", "", txt14W2),
        ("5M", "", txt5M2),
        ("6M", "Complementary foods", txt6M_CF),
("7M", "Complementary foods", txt7M_CF),
// ...
("24M", "Breastfeeding continues", txt24M_BF),

        ("6M-24M", "Food Groups given/day", txtFoodGroups),
        ("6M-24M", "No. of meals/ day", txtMealsPerDay)
    };

            // Clear existing entries
            string deleteQuery = "DELETE FROM InfantFeeding WHERE ChildId = @ChildId";
            using (SqlCommand delCmd = new SqlCommand(deleteQuery, con))
            {
                delCmd.Parameters.AddWithValue("@ChildId", childId);
                con.Open();
                delCmd.ExecuteNonQuery();
                con.Close();
            }

            // Insert new values
            string insertQuery = @"INSERT INTO InfantFeeding (ChildId, AgeLabel, FeedingType, Value)
                           VALUES (@ChildId, @AgeLabel, @FeedingType, @Value)";

            foreach (var (AgeLabel, FeedingType, Input) in feedingEntries)
            {
                string value = Input.Text.Trim();
                if (!string.IsNullOrEmpty(value) && int.TryParse(value, out int code) && code >= 1 && code <= 6)
                {
                    using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@ChildId", childId);
                        cmd.Parameters.AddWithValue("@AgeLabel", AgeLabel);
                        cmd.Parameters.AddWithValue("@FeedingType", FeedingType);
                        cmd.Parameters.AddWithValue("@Value", value);

                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                else if (!string.IsNullOrEmpty(value))
                {
                    // Optional: you can log or warn about invalid entry
                    lblMessage.Text += $"Invalid code in {AgeLabel} ({FeedingType}) — please enter a number 1 to 6.<br/>";
                }
            }
        }
        private void LoadInfantFeeding(int childId)
        {
            string query = "SELECT AgeLabel, FeedingType, Value FROM InfantFeeding WHERE ChildId = @ChildId";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@ChildId", childId);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string ageLabel = reader["AgeLabel"].ToString();
                    string type = reader["FeedingType"].ToString();
                    string value = reader["Value"].ToString();

                    // Match the label and type to the right textbox
                    if (ageLabel == "Birth" && type == "Breast milk only") txtBirth1.Text = value;
                    else if (ageLabel == "Birth" && type == "") txtBirth2.Text = value;
                    else if (ageLabel == "6W" && type == "Breast milk only") txt6W1.Text = value;
                    else if (ageLabel == "6W" && type == "") txt6W2.Text = value;
                    else if (ageLabel == "6M-24M" && type == "Food Groups given/day") txtFoodGroups.Text = value;
                    else if (ageLabel == "6M-24M" && type == "No. of meals/ day") txtMealsPerDay.Text = value;
                    // Add all other mappings...
                }
                con.Close();
            }
        }
        private void LoadInfantCare(int ChildID)
        {
            string connStr = ConfigurationManager.ConnectionStrings["Zimvaxsync"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT TOP 1 * FROM InfantCare WHERE ChildId = @ChildId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ChildId", ChildID);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        chkMotherOnARTYes.Checked = (bool)reader["IsMotherOnART"];
                        chkARVYes.Checked = reader["ARVProphylaxisGiven"].ToString() == "Yes";
                        chkARVNo.Checked = reader["ARVProphylaxisGiven"].ToString() == "No";
                        txtARVProphylaxis.Text = reader["ARVProphylaxisDetails"].ToString();

                        // Set values back to controls (same as Save)
                        txtARV_6W.Text = reader["ARV_6W"].ToString();
                        txtCTX_6W.Text = reader["CTX_6W"].ToString();
                        // etc...
                    }
                }
            }
        }
        protected void btnSaveCare_Click(object sender, EventArgs e)
        {
            int childId = GetChildID(); // or GetInfantID if it's different
            SaveInfantCare(childId);
        }

        protected void btnSaveFeeding_Click(object sender, EventArgs e)
        {
            int childId = GetChildID(); // use your existing method to get current child ID
            SaveInfantFeeding(childId);
        }

        private void SaveInfantCare(int infantID)
        {
            string connStr = ConfigurationManager.ConnectionStrings["Zimvaxsync"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"INSERT INTO InfantCare (
                            ChildId, IsMotherOnART, ARVProphylaxisGiven, ARVProphylaxisDetails, ARVProphylaxisInitials,
                            Test1Type, Test1Date, Test1Number, Test1Result, Test1ReferredDate, Test1InitiatedDate,
                            Test2Type, Test2Date, Test2Number, Test2Result, Test2ReferredDate, Test2InitiatedDate,
                            Test3Type, Test3Date, Test3Number, Test3Result, Test3ReferredDate, Test3InitiatedDate,
                            ARV_6W, ARV_10W, ARV_14W, ARV_5M, ARV_7M, ARV_8M, ARV_10M, ARV_11M, ARV_13M, ARV_14M,
                            ARV_15M, ARV_16M, ARV_17M, ARV_18M, ARV_19M, ARV_20M, ARV_21M, ARV_22M, ARV_23M, ARV_24M,
                            CTX_6W, CTX_10W, CTX_14W, CTX_5M, CTX_7M, CTX_8M, CTX_10M, CTX_11M, CTX_13M, CTX_14M,
                            CTX_15M, CTX_16M, CTX_17M, CTX_18M, CTX_19M, CTX_20M, CTX_21M, CTX_22M, CTX_23M, CTX_24M
                        )
                        VALUES (
                            @ChildId, @IsMotherOnART, @ARVGiven, @ARVDetails, @ARVInitials,
                            @Test1Type, @Test1Date, @Test1Num, @Test1Result, @Test1Ref, @Test1Init,
                            @Test2Type, @Test2Date, @Test2Num, @Test2Result, @Test2Ref, @Test2Init,
                            @Test3Type, @Test3Date, @Test3Num, @Test3Result, @Test3Ref, @Test3Init,
                            @ARV_6W, @ARV_10W, @ARV_14W, @ARV_5M, @ARV_7M, @ARV_8M, @ARV_10M, @ARV_11M, @ARV_13M, @ARV_14M,
                            @ARV_15M, @ARV_16M, @ARV_17M, @ARV_18M, @ARV_19M, @ARV_20M, @ARV_21M, @ARV_22M, @ARV_23M, @ARV_24M,
                            @CTX_6W, @CTX_10W, @CTX_14W, @CTX_5M, @CTX_7M, @CTX_8M, @CTX_10M, @CTX_11M, @CTX_13M, @CTX_14M,
                            @CTX_15M, @CTX_16M, @CTX_17M, @CTX_18M, @CTX_19M, @CTX_20M, @CTX_21M, @CTX_22M, @CTX_23M, @CTX_24M
                        )";

                SqlCommand cmd = new SqlCommand(query, conn);

                // Add parameters here like:
                cmd.Parameters.AddWithValue("@ChildId", infantID);
                cmd.Parameters.AddWithValue("@IsMotherOnART", chkMotherOnARTYes.Checked ? 1 : 0);
                cmd.Parameters.AddWithValue("@ARVGiven", chkARVYes.Checked ? "Yes" : chkARVNo.Checked ? "No" : "N/A");
                cmd.Parameters.AddWithValue("@ARVDetails", txtARVProphylaxis.Text);
                cmd.Parameters.AddWithValue("@ARVInitials", "NVP"); // example or from user input

                // Similarly bind all test fields and follow-up fields using:
                cmd.Parameters.AddWithValue("@Test1Type", txtTest1Type.Text);
                // ... continue for Test1Date, Test1Number, etc.

                // ARV Supplied
                cmd.Parameters.AddWithValue("@ARV_6W", txtARV_6W.Text);
                cmd.Parameters.AddWithValue("@ARV_10W", txtARV_10W.Text);
                cmd.Parameters.AddWithValue("@ARV_14W", txtARV_14W.Text);
                cmd.Parameters.AddWithValue("@ARV_5M", txtARV_5M.Text);
                cmd.Parameters.AddWithValue("@ARV_7M", txtARV_7M.Text);
                cmd.Parameters.AddWithValue("@ARV_8M", txtARV_8M.Text);
                cmd.Parameters.AddWithValue("@ARV_10M", txtARV_10M.Text);
                cmd.Parameters.AddWithValue("@ARV_11M", txtARV_11M.Text);
                cmd.Parameters.AddWithValue("@ARV_13M", txtARV_13M.Text);
                cmd.Parameters.AddWithValue("@ARV_14M", txtARV_14M.Text);
                cmd.Parameters.AddWithValue("@ARV_15M", txtARV_15M.Text);
                cmd.Parameters.AddWithValue("@ARV_16M", txtARV_16M.Text);
                cmd.Parameters.AddWithValue("@ARV_17M", txtARV_17M.Text);
                cmd.Parameters.AddWithValue("@ARV_18M", txtARV_18M.Text);
                cmd.Parameters.AddWithValue("@ARV_19M", txtARV_19M.Text);
                cmd.Parameters.AddWithValue("@ARV_20M", txtARV_20M.Text);
                cmd.Parameters.AddWithValue("@ARV_21M", txtARV_21M.Text);
                cmd.Parameters.AddWithValue("@ARV_22M", txtARV_22M.Text);
                cmd.Parameters.AddWithValue("@ARV_23M", txtARV_23M.Text);
                cmd.Parameters.AddWithValue("@ARV_24M", txtARV_24M.Text);

                // CTX Supplied
                cmd.Parameters.AddWithValue("@CTX_6W", txtCTX_6W.Text);
                cmd.Parameters.AddWithValue("@CTX_10W", txtCTX_10W.Text);
                cmd.Parameters.AddWithValue("@CTX_14W", txtCTX_14W.Text);
                cmd.Parameters.AddWithValue("@CTX_5M", txtCTX_5M.Text);
                cmd.Parameters.AddWithValue("@CTX_7M", txtCTX_7M.Text);
                cmd.Parameters.AddWithValue("@CTX_8M", txtCTX_8M.Text);
                cmd.Parameters.AddWithValue("@CTX_10M", txtCTX_10M.Text);
                cmd.Parameters.AddWithValue("@CTX_11M", txtCTX_11M.Text);
                cmd.Parameters.AddWithValue("@CTX_13M", txtCTX_13M.Text);
                cmd.Parameters.AddWithValue("@CTX_14M", txtCTX_14M.Text);
                cmd.Parameters.AddWithValue("@CTX_15M", txtCTX_15M.Text);
                cmd.Parameters.AddWithValue("@CTX_16M", txtCTX_16M.Text);
                cmd.Parameters.AddWithValue("@CTX_17M", txtCTX_17M.Text);
                cmd.Parameters.AddWithValue("@CTX_18M", txtCTX_18M.Text);
                cmd.Parameters.AddWithValue("@CTX_19M", txtCTX_19M.Text);
                cmd.Parameters.AddWithValue("@CTX_20M", txtCTX_20M.Text);
                cmd.Parameters.AddWithValue("@CTX_21M", txtCTX_21M.Text);
                cmd.Parameters.AddWithValue("@CTX_22M", txtCTX_22M.Text);
                cmd.Parameters.AddWithValue("@CTX_23M", txtCTX_23M.Text);
                cmd.Parameters.AddWithValue("@CTX_24M", txtCTX_24M.Text);
                // Repeat for ARV_10W to ARV_24M and CTX...

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}

