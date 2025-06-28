using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Web.UI.DataVisualization.Charting;
using QRCoder;
using System.Web.UI;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using ZimVaxSync.Helpers;
using System.Web;

namespace ZimVaxSync
{
    public partial class ViewChildhoodRecord : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["Zimvaxsync"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int childId = GetChildID();

                if (childId > 0)
                {
                    LoadChildInfo(childId);
                    LoadVaccinationRecords(childId);
                    LoadInfantCare(childId);
                    LoadFeeding(childId);
                    LoadMilestones(childId);
                    string gender = GetChildGender(childId);
                    LoadGrowthCharts(childId, gender);
                    GenerateQRCode();

                    bool isHealthcare = Session["HealthcareProviderId"] != null;
                    btnEdit.Visible = isHealthcare;
                    txtNotes.ReadOnly = !isHealthcare;
                }
                else
                {
                    lblChildInfo.Text = "Child profile not found.";
                }
            }
        }

        private void GenerateQRCode()
        {
            int childId = GetChildID();
            string encryptedId = EncryptionHelper.Encrypt(childId.ToString());
            string url = Request.Url.GetLeftPart(UriPartial.Authority) + "/ViewChildhoodRecord.aspx?token=" + encryptedId;

            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q))
            using (QRCoder.QRCode qrCode = new QRCoder.QRCode(qrCodeData))
            using (System.Drawing.Bitmap qrImage = qrCode.GetGraphic(20))
            using (MemoryStream ms = new MemoryStream())
            {
                qrImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                imgQrCode.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
            }
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "PrintPreview", "window.print();", true);
        }

      

        public override void VerifyRenderingInServerForm(Control control) { }

        private int GetChildID()
        {
            if (Request.QueryString["token"] != null)
            {
                try
                {
                    string decrypted = EncryptionHelper.Decrypt(Request.QueryString["token"]);
                    if (int.TryParse(decrypted, out int idFromToken))
                        return idFromToken;
                }
                catch { }
            }

            if (Request.QueryString["childId"] != null && int.TryParse(Request.QueryString["childId"], out int childId))
                return childId;

            if (Session["ChildID"] != null && int.TryParse(Session["ChildID"].ToString(), out childId))
                return childId;

            return -1;
        }

        private void LoadChildInfo(int childId)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT FullName, DateOfBirth, Gender, IDNumber FROM ChildProfile WHERE ChildID = @ChildID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ChildID", childId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    string name = reader["FullName"].ToString();
                    DateTime dob = Convert.ToDateTime(reader["DateOfBirth"]);
                    string gender = reader["Gender"].ToString();
                    string id = reader["IDNumber"].ToString();
                    int age = CalculateAge(dob);

                    lblChildInfo.Text = $"<b>Name:</b> {name}<br/><b>Gender:</b> {gender}<br/><b>Date of Birth:</b> {dob:dd MMM yyyy}<br/><b>Age:</b> {age} years<br/><b>ID Number:</b> {id}";
                }

                reader.Close();
            }
        }

        private string GetChildGender(int childId)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT Gender FROM ChildProfile WHERE ChildID = @ChildID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ChildID", childId);
                conn.Open();
                object result = cmd.ExecuteScalar();
                return result?.ToString() ?? "male";
            }
        }

        private int CalculateAge(DateTime dob)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - dob.Year;
            if (dob.Date > today.AddYears(-age)) age--;
            return age;
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            int childId = GetChildID();
            if (childId > 0)
                Response.Redirect($"AddChildhoodRecord.aspx?childId={childId}");
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (Session["HealthcareProviderId"] != null)
                Response.Redirect("HealthcareDashboard.aspx");
            else if (Session["CaregiverId"] != null)
                Response.Redirect("CaregiverDashboard.aspx");
            else if (Session["GeneralUserId"] != null)
                Response.Redirect("OtherDashboard.aspx");
            else
                Response.Redirect("Loginpage.aspx");
        }

        private void LoadVaccinationRecords(int childId)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"SELECT VaccineName, AgeGroup, DateAdministered, BatchNumber, Status, NextDoseDate, DoctorRemarks
                                 FROM ChildVaccinationRecords
                                 WHERE ChildID = @ChildID
                                 ORDER BY DateAdministered";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ChildID", childId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvVaccinations.DataSource = dt;
                gvVaccinations.DataBind();
            }
        }

        private void LoadInfantCare(int childId)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string checkQuery = "SELECT IsMotherOnART FROM InfantCare WHERE ChildID = @ChildID";
                SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@ChildID", childId);
                string isOnART = (checkCmd.ExecuteScalar() ?? "").ToString();

                if (string.IsNullOrEmpty(isOnART))
                {
                    lblInfantCare.Text = "No infant care data found.";
                    return;
                }

                if (isOnART.Equals("Yes", StringComparison.OrdinalIgnoreCase))
                {
                    string fullQuery = @"SELECT IsMotherOnART, ARVProphylaxisGiven, ARVProphylaxisDetails, Test1Type, Test1Date,
                                         Test1Result, Test2Type, Test2Date, Test2Result, ARV_6W, ARV_10W, CTX_6W, CTX_10W
                                         FROM InfantCare WHERE ChildID = @ChildID";

                    SqlCommand fullCmd = new SqlCommand(fullQuery, conn);
                    fullCmd.Parameters.AddWithValue("@ChildID", childId);

                    SqlDataReader reader = fullCmd.ExecuteReader();
                    if (reader.Read())
                    {
                        lblInfantCare.Text = $"<b>Mother on ART:</b> {reader["IsMotherOnART"]}<br/>" +
                                             $"<b>ARV Given:</b> {reader["ARVProphylaxisGiven"]}<br/>" +
                                             $"<b>ARV Details:</b> {reader["ARVProphylaxisDetails"]}<br/>" +
                                             $"<b>Test 1:</b> {reader["Test1Type"]}, {reader["Test1Date"]}, Result: {reader["Test1Result"]}<br/>" +
                                             $"<b>Test 2:</b> {reader["Test2Type"]}, {reader["Test2Date"]}, Result: {reader["Test2Result"]}<br/>" +
                                             $"<b>ARV 6W:</b> {reader["ARV_6W"]}, <b>ARV 10W:</b> {reader["ARV_10W"]}<br/>" +
                                             $"<b>CTX 6W:</b> {reader["CTX_6W"]}, <b>CTX 10W:</b> {reader["CTX_10W"]}";
                    }
                    reader.Close();
                }
                else
                {
                    lblInfantCare.Text = $"<b>Mother on ART:</b> {isOnART}";
                }
            }
        }

        private void LoadFeeding(int childId)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"SELECT AgeLabel, FeedingType, Value, DateRecorded 
                                 FROM InfantFeeding 
                                 WHERE ChildID = @ChildID 
                                 ORDER BY DateRecorded";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ChildID", childId);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("<table border='1' cellpadding='6' style='border-collapse:collapse;'>");
                    sb.Append("<tr><th>Age</th><th>Feeding Type</th><th>Value</th><th>Date Recorded</th></tr>");

                    while (reader.Read())
                    {
                        sb.Append("<tr>");
                        sb.AppendFormat("<td>{0}</td>", reader["AgeLabel"]);
                        sb.AppendFormat("<td>{0}</td>", reader["FeedingType"]);
                        sb.AppendFormat("<td>{0}</td>", reader["Value"]);
                        sb.AppendFormat("<td>{0:dd MMM yyyy}</td>", Convert.ToDateTime(reader["DateRecorded"]));
                        sb.Append("</tr>");
                    }

                    sb.Append("</table>");
                    lblFeeding.Text = sb.ToString();
                }
                else
                {
                    lblFeeding.Text = "No feeding data recorded.";
                }

                reader.Close();
            }
        }

        private void LoadMilestones(int childId)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"SELECT Milestone, Achieved, Notes, DateRecorded 
                                 FROM DevelopmentMilestones 
                                 WHERE ChildID = @ChildID 
                                 ORDER BY DateRecorded";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ChildID", childId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvMilestones.DataSource = dt;
                gvMilestones.DataBind();
            }
        }

        private void LoadGrowthCharts(int childID, string gender)
        {
            string tableSuffix = gender.ToLower() == "female" ? "Girls" : "Boys";
            DataTable dtHeight = GetWHOData("WHOHeightForAge" + tableSuffix);
            DataTable dtWeight = GetWHOData("WHOWeightForAge" + tableSuffix);

            ChartHeight.Series.Clear();
            ChartWeight.Series.Clear();

            PlotZScoreLines(dtHeight, ChartHeight, "Height (cm)");
            PlotZScoreLines(dtWeight, ChartWeight, "Weight (kg)");

            string query = "SELECT AgeInMonths, Height, Weight FROM GrowthMonitoring WHERE ChildId = @ChildId ORDER BY AgeInMonths";
            using (SqlConnection con = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@ChildId", childID);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                Series heightSeries = new Series("ChildHeight") { ChartType = SeriesChartType.Line, BorderWidth = 3, Color = Color.Black };
                Series weightSeries = new Series("ChildWeight") { ChartType = SeriesChartType.Line, BorderWidth = 3, Color = Color.Black };

                while (reader.Read())
                {
                    int age = Convert.ToInt32(reader["AgeInMonths"]);
                    if (reader["Height"] != DBNull.Value)
                        heightSeries.Points.AddXY(age, Convert.ToDouble(reader["Height"]));
                    if (reader["Weight"] != DBNull.Value)
                        weightSeries.Points.AddXY(age, Convert.ToDouble(reader["Weight"]));
                }

                reader.Close();
                ChartHeight.Series.Add(heightSeries);
                ChartWeight.Series.Add(weightSeries);
            }
        }

        private DataTable GetWHOData(string tableName)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT * FROM " + tableName;
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        private void PlotZScoreLines(DataTable dt, Chart chart, string yAxisTitle)
        {
            string[] zProperties = { "Minus3SD", "Minus2SD", "Median", "Plus2SD", "Plus3SD" };
            string[] zLabels = chart.ID.Contains("Weight")
                ? new[] { "-3 Severe Underweight", "-2 Moderate Underweight", "Normal", "+2 Possible Overweight", "+3 Check Weight for Height" }
                : new[] { "-3 Severe Stunted", "-2 Moderate Stunted", "Normal", "+2 Normal", "+3 Possible Very Tall" };

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
    }
}

