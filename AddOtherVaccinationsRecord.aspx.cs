using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Services;
using System.Web.UI;

namespace ZimVaxSync
{
    public partial class AddOtherVaccinationsRecord : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["Zimvaxsync"].ConnectionString;

        private static readonly Dictionary<string, (int totalDoses, List<int> intervalsInDays)> vaccineSchedules = new Dictionary<string, (int, List<int>)>
        {
            { "COVID-19", (2, new List<int> { 21 }) },
            { "HEPATITIS A", (2, new List<int> { 180 }) },
            { "HEPATITIS B", (3, new List<int> { 30, 150 }) },
            { "TYPHOID", (1, new List<int>()) },
            { "RABIES", (5, new List<int> { 3, 4, 7, 14 }) },
            { "CHIKUNGUNYA", (1, new List<int>()) },
            { "YELLOW FEVER", (1, new List<int>()) },
            { "CHOLERA", (2, new List<int> { 14 }) },
            { "MENINGITIS ACWY", (1, new List<int>()) },
            { "MENINGITIS B", (2, new List<int> { 28 }) }
        };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                caregiverSection.Visible = false;

                if (Request.QueryString["childId"] != null && int.TryParse(Request.QueryString["childId"], out int childId))
                {
                    LoadChildInfo(childId);
                }
                else if (Request.QueryString["userId"] != null)
                {
                    string userId = Request.QueryString["userId"];
                    LoadOtherUserInfo(userId);
                }
                else
                {
                    lblMessage.Text = "No valid user or child ID was provided in the URL.";
                }

                string profession = Session["Profession"]?.ToString();
                string fullName = Session["FullName"]?.ToString();
                if (!string.IsNullOrEmpty(profession) && !string.IsNullOrEmpty(fullName))
                {
                    txtAdministeredBy.Text = $"{profession} {fullName}";
                }

                txtFacilityName.Text = Session["Institution"]?.ToString();
            }
        }

        [WebMethod]
        public static object GetNextDose(string vaccineName, string dateGiven, string nationalIdOrChildId, string userType)
        {
            if (!vaccineSchedules.TryGetValue(vaccineName.ToUpper(), out var schedule))
                return null;

            if (!DateTime.TryParse(dateGiven, out DateTime parsedDate))
                return null;

            int nextDoseNumber = 1;
            DateTime? nextDue = null;

            string connStr = ConfigurationManager.ConnectionStrings["Zimvaxsync"].ConnectionString;
            string column = userType == "child" ? "ChildId" : "OtherUserID";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string query = $"SELECT COUNT(*) FROM VaccinationRecords WHERE VaccineName = @VaccineName AND {column} = @ID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@VaccineName", vaccineName);
                    cmd.Parameters.AddWithValue("@ID", nationalIdOrChildId);
                    object result = cmd.ExecuteScalar();
                    int previousDoseCount = result != null ? Convert.ToInt32(result) : 0;
                    nextDoseNumber = previousDoseCount + 1;

                    if (schedule.totalDoses >= nextDoseNumber && schedule.intervalsInDays.Count >= nextDoseNumber)
                    {
                        nextDue = parsedDate.AddDays(schedule.intervalsInDays[nextDoseNumber - 1]);
                    }
                }
            }

            return new
            {
                doseNumber = nextDoseNumber.ToString(),
                nextDateDue = nextDue?.ToString("yyyy-MM-dd")
            };
        }

        private void LoadChildInfo(int childId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT c.FullName AS ChildName, c.Gender, c.DateOfBirth, c.IDNumber,
                           cg.FullName AS CaregiverName, cg.RelationshipToChild, cg.Address, cg.PhoneNumber
                    FROM ChildProfile c
                    INNER JOIN Caregiver cg ON c.CaregiverID = cg.CaregiverID
                    WHERE c.ChildId = @ChildId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ChildId", childId);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lblFullName.Text = reader["ChildName"].ToString();
                            lblGender.Text = reader["Gender"].ToString();
                            lblDOB.Text = Convert.ToDateTime(reader["DateOfBirth"]).ToString("yyyy-MM-dd");
                            lblIDNumber.Text = reader["IDNumber"].ToString();

                            caregiverSection.Visible = true;
                            lblCaregiverName.Text = reader["CaregiverName"].ToString();
                            lblRelationship.Text = reader["RelationshipToChild"].ToString();
                            lblAddress.Text = reader["Address"].ToString();
                            lblPhoneNumber.Text = reader["PhoneNumber"].ToString();
                        }
                    }
                }
            }
        }

        private void LoadOtherUserInfo(string nationalID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT FullName, Gender, DOB, NationalIDNumber, Address, PhoneNumber 
                    FROM OtherGeneralUser 
                    WHERE NationalIDNumber = @NationalID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@NationalID", nationalID);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lblFullName.Text = reader["FullName"].ToString();
                            lblGender.Text = reader["Gender"].ToString();
                            lblDOB.Text = Convert.ToDateTime(reader["DOB"]).ToString("yyyy-MM-dd");
                            lblIDNumber.Text = reader["NationalIDNumber"].ToString();
                            lblAddress.Text = reader["Address"].ToString();
                            lblPhoneNumber.Text = reader["PhoneNumber"].ToString();
                        }
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string vaccineName = ddlVaccineName.SelectedValue.Trim();
            string doseNumber = txtDoseNumber.Text.Trim();
            string batchNumber = txtBatchNumber.Text.Trim();
            string manufacturer = txtManufacturer.Text.Trim();
            string dateGiven = txtDateGiven.Text;
            DateTime? nextDateDue = null;

            if (string.IsNullOrEmpty(vaccineName))
            {
                lblMessage.Text = "Please select a vaccine from the list.";
                return;
            }

            if (DateTime.TryParse(dateGiven, out DateTime parsedGivenDate))
            {
                if (vaccineSchedules.TryGetValue(vaccineName.ToUpper(), out var schedule))
                {
                    if (int.TryParse(doseNumber, out int currentDoseNumber) &&
                        currentDoseNumber < schedule.totalDoses &&
                        currentDoseNumber - 1 < schedule.intervalsInDays.Count)
                    {
                        int intervalDays = schedule.intervalsInDays[currentDoseNumber - 1];
                        nextDateDue = parsedGivenDate.AddDays(intervalDays);
                    }
                }
            }

            string administeredBy = txtAdministeredBy.Text;
            string facilityName = txtFacilityName.Text;
            string comments = txtComments.Text.Trim();
            string nationalID = lblIDNumber.Text.Trim();

            int foreignKeyID = -1;
            string foreignKeyColumn = "";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                if (Request.QueryString["childId"] != null && int.TryParse(Request.QueryString["childId"], out int childId))
                {
                    using (SqlCommand getChildCmd = new SqlCommand("SELECT ChildId FROM ChildProfile WHERE ChildId = @ChildId", conn))
                    {
                        getChildCmd.Parameters.AddWithValue("@ChildId", childId);
                        object result = getChildCmd.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out foreignKeyID))
                        {
                            foreignKeyColumn = "ChildId";
                        }
                        else
                        {
                            lblMessage.Text = "Child not found.";
                            return;
                        }
                    }
                }
                else if (Request.QueryString["userId"] != null)
                {
                    using (SqlCommand getUserCmd = new SqlCommand("SELECT OtherUserID FROM OtherGeneralUser WHERE NationalIDNumber = @NationalID", conn))
                    {
                        getUserCmd.Parameters.AddWithValue("@NationalID", nationalID);
                        object result = getUserCmd.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out foreignKeyID))
                        {
                            foreignKeyColumn = "OtherUserID";
                        }
                        else
                        {
                            lblMessage.Text = "General user not found.";
                            return;
                        }
                    }
                }
                else
                {
                    lblMessage.Text = "No valid user or child ID was provided in the URL.";
                    return;
                }

                string insertQuery = $@"
                    INSERT INTO VaccinationRecords
                    ({foreignKeyColumn}, VaccineName, DoseNumber, BatchNumber, Manufacturer, DateGiven, NextDoseDue, AdministeredBy, FacilityName, Comments)
                    VALUES
                    (@ForeignKeyID, @VaccineName, @DoseNumber, @BatchNumber, @Manufacturer, @DateGiven, @NextDoseDue, @AdministeredBy, @FacilityName, @Comments)";

                using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                {
                    insertCmd.Parameters.AddWithValue("@ForeignKeyID", foreignKeyID);
                    insertCmd.Parameters.AddWithValue("@VaccineName", vaccineName);
                    insertCmd.Parameters.AddWithValue("@DoseNumber", doseNumber);
                    insertCmd.Parameters.AddWithValue("@BatchNumber", batchNumber);
                    insertCmd.Parameters.AddWithValue("@Manufacturer", manufacturer);
                    insertCmd.Parameters.AddWithValue("@DateGiven", dateGiven);
                    insertCmd.Parameters.AddWithValue("@NextDoseDue", nextDateDue.HasValue ? (object)nextDateDue.Value : DBNull.Value);
                    insertCmd.Parameters.AddWithValue("@AdministeredBy", administeredBy);
                    insertCmd.Parameters.AddWithValue("@FacilityName", facilityName);
                    insertCmd.Parameters.AddWithValue("@Comments", comments);

                    insertCmd.ExecuteNonQuery();
                }

                if (nextDateDue.HasValue && !string.Equals(vaccineName, "BCG", StringComparison.OrdinalIgnoreCase))
                {
                    string scheduleInsert = $@"
                        IF NOT EXISTS (
                            SELECT 1 FROM VaccinationSchedule 
                            WHERE {foreignKeyColumn} = @ForeignKeyID 
                              AND VaccineName = @VaccineName
                        )
                        BEGIN
                            INSERT INTO VaccinationSchedule ({foreignKeyColumn}, VaccineName, DueDate, Status)
                            VALUES (@ForeignKeyID, @VaccineName, @DueDate, 'Pending')
                        END";

                    using (SqlCommand scheduleCmd = new SqlCommand(scheduleInsert, conn))
                    {
                        scheduleCmd.Parameters.AddWithValue("@ForeignKeyID", foreignKeyID);
                        scheduleCmd.Parameters.AddWithValue("@VaccineName", vaccineName);
                        scheduleCmd.Parameters.AddWithValue("@DueDate", nextDateDue.Value);
                        scheduleCmd.ExecuteNonQuery();
                    }
                }

                try
                {
                    string recipientName = lblFullName.Text;
                    string recipientPhone = lblPhoneNumber.Text;
                    string cleanedPhone = CleanPhoneNumber(recipientPhone);

                    if (!string.IsNullOrEmpty(cleanedPhone))
                    {
                        string thankYouMessage = $"Dear {recipientName}, thank you for being vaccinated with {vaccineName}. Your next dose is due on {nextDateDue?.ToString("yyyy-MM-dd")}. Regards, ZimVaxSync team.";

                        var smsSender = new TelerivetSmsSender();
                        smsSender.SendSmsAsync(cleanedPhone, thankYouMessage).Wait();
                    }

                    ClearFormFields();
                    ScriptManager.RegisterStartupScript(this, GetType(), "showSuccessModal", "showModal('successModal');", true);

                }
                catch (Exception ex)
                {
                    Console.WriteLine("SMS Error: " + ex.Message);
                    ScriptManager.RegisterStartupScript(this, GetType(), "showErrorModal", "showModal('#errorModal').modal('show');", true);
                }
            }
        }

        private void ClearFormFields()
        {
            ddlVaccineName.SelectedIndex = 0;
            txtDoseNumber.Text = "";
            txtBatchNumber.Text = "";
            txtManufacturer.Text = "";
            txtDateGiven.Text = "";
            txtNextDateDue.Text = "";
            txtComments.Text = "";
        }

        private string CleanPhoneNumber(string rawPhone)
        {
            if (string.IsNullOrWhiteSpace(rawPhone)) return "";
            string cleaned = rawPhone.Trim().Replace("\t", "").Replace("\n", "").Replace("\r", "");
            cleaned = System.Text.RegularExpressions.Regex.Replace(cleaned, "[^0-9+]", "");
            if (cleaned.StartsWith("07")) return "+263" + cleaned.Substring(1);
            if (cleaned.StartsWith("263")) return "+" + cleaned;
            if (!cleaned.StartsWith("+263")) return "+263" + cleaned;
            return cleaned;
        }
    }
}