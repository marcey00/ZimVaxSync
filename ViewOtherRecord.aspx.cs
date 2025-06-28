using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web.UI.WebControls;
using QRCoder;
using iTextSharp.text;
using iTextSharp.text.pdf;
using ZimVaxSync.Helpers;
using System.Web;

namespace ZimVaxSync
{
    public partial class ViewOtherRecord : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["Zimvaxsync"].ConnectionString;

        private string SortExpression
        {
            get { return ViewState["SortExpression"] as string ?? "DateGiven DESC"; }
            set { ViewState["SortExpression"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserRole"] == null)
                {
                    Response.Redirect("~/LoginPage.aspx"); // prevent unauthenticated access
                    return;
                }

                int userId = GetUserIdFromToken();

                if (Session["ChildId"] != null)
                {
                    LoadChildDemographics(userId);
                    LoadAllVaccinationRecordsForChild(userId);
                }
                else if (Session["OtherUserId"] != null)
                {
                    LoadOtherUserDemographics(userId);
                    LoadAllVaccinationRecordsForUser(userId);
                }

                GenerateAndSaveQRCode();
            }
        }

        private int GetUserIdFromToken()
        {
            if (Request.QueryString["token"] != null)
            {
                try
                {
                    string decrypted = EncryptionHelper.Decrypt(Request.QueryString["token"]);
                    if (int.TryParse(decrypted, out int id))
                        return id;
                }
                catch { }
            }

            if (Session["ChildId"] != null && int.TryParse(Session["ChildId"].ToString(), out int childId))
                return childId;

            if (Session["OtherUserId"] != null && int.TryParse(Session["OtherUserId"].ToString(), out int userId))
                return userId;

            return -1;
        }

        private void LoadChildDemographics(int childId)
        {
            string query = @"SELECT c.FullName AS ChildName, c.Gender, c.DateOfBirth, cg.FullName AS CaregiverName, 
                            c.RelationshipToChild, cg.Address, cg.PhoneNumber 
                            FROM ChildProfile c 
                            INNER JOIN Caregiver cg ON c.CaregiverId = cg.CaregiverId 
                            WHERE c.ChildId = @ChildId";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@ChildId", childId);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string childName = reader["ChildName"]?.ToString() ?? "Unknown";
                        string gender = reader["Gender"]?.ToString() ?? "Unknown";
                        DateTime dob = reader["DateOfBirth"] != DBNull.Value ? Convert.ToDateTime(reader["DateOfBirth"]) : DateTime.MinValue;
                        string dobFormatted = dob != DateTime.MinValue ? dob.ToString("dd/MM/yyyy") : "Unknown";

                        int age = 0;
                        if (dob != DateTime.MinValue)
                        {
                            age = DateTime.Today.Year - dob.Year;
                            if (DateTime.Today < dob.AddYears(age)) age--;
                        }

                        string caregiverName = reader["CaregiverName"]?.ToString() ?? "Unknown";
                        string relationship = reader["RelationshipToChild"]?.ToString() ?? "N/A";
                        string address = reader["Address"]?.ToString() ?? "N/A";
                        string phone = reader["PhoneNumber"]?.ToString() ?? "N/A";

                        lblHeader.Text = $"Child Record:<br />Name: {childName}<br />Gender: {gender}<br />DOB: {dobFormatted}<br />Age: {age}<br /><br />Caregiver: {caregiverName}<br />Relationship: {relationship}<br />Address: {address}<br />Phone: {phone}";
                    }
                    else
                    {
                        lblHeader.Text = "Child Record not found.";
                    }
                }
            }
        }

        private void LoadOtherUserDemographics(int userId)
        {
            string query = "SELECT FullName, Gender, DOB, Address, PhoneNumber FROM OtherGeneralUser WHERE OtherUserId = @UserId";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@UserId", userId);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string name = reader["FullName"]?.ToString() ?? "Unknown";
                        string gender = reader["Gender"]?.ToString() ?? "Unknown";
                        string address = reader["Address"]?.ToString() ?? "N/A";
                        string phone = reader["PhoneNumber"]?.ToString() ?? "N/A";

                        DateTime dob = reader["DOB"] != DBNull.Value ? Convert.ToDateTime(reader["DOB"]) : DateTime.MinValue;
                        string dobFormatted = dob != DateTime.MinValue ? dob.ToString("dd/MM/yyyy") : "Unknown";

                        int age = 0;
                        if (dob != DateTime.MinValue)
                        {
                            age = DateTime.Today.Year - dob.Year;
                            if (DateTime.Today < dob.AddYears(age)) age--;
                        }

                        lblHeader.Text = $"General User Record:<br />Name: {name}<br />Gender: {gender}<br />DOB: {dobFormatted}<br />Age: {age}<br />Address: {address}<br />Phone: {phone}";
                    }
                    else
                    {
                        lblHeader.Text = "General User Record not found.";
                    }
                }
            }
        }

        public void LoadAllVaccinationRecordsForUser(int userId)
        {
            string query = @"SELECT RecordId, VaccineName, DoseNumber, Manufacturer, DateGiven, NextDoseDue, FacilityName, AdministeredBy 
                             FROM VaccinationRecords WHERE OtherUserId = @OtherUserId";
            BindGridWithQuery(query, "@OtherUserId", userId);
        }

        public void LoadAllVaccinationRecordsForChild(int childId)
        {
            string query = @"SELECT RecordId, VaccineName, DoseNumber, Manufacturer, DateGiven, NextDoseDue, FacilityName, AdministeredBy 
                             FROM VaccinationRecords WHERE ChildId = @ChildId";
            BindGridWithQuery(query, "@ChildId", childId);
        }

        private void BindGridWithQuery(string query, string paramName, int id)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue(paramName, id);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }

            DataView dv = dt.DefaultView;
            dv.Sort = SortExpression;

            if (!string.IsNullOrWhiteSpace(txtFilter.Text))
            {
                dv.RowFilter = $"VaccineName LIKE '%{txtFilter.Text.Replace("'", "''")}%'"; // simple filter
            }

            gvVaccinationRecords.DataSource = dv;
            gvVaccinationRecords.DataBind();
        }

        protected void gvVaccinationRecords_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = e.SortExpression;
            string currentDirection = ViewState["SortDirection"] as string ?? "ASC";

            if (ViewState["SortExpression"] != null && ViewState["SortExpression"].ToString() == sortExpression)
                currentDirection = currentDirection == "ASC" ? "DESC" : "ASC";
            else
                currentDirection = "ASC";

            ViewState["SortExpression"] = sortExpression + " " + currentDirection;

            int userId = GetUserIdFromToken();
            if (Session["ChildId"] != null)
                LoadAllVaccinationRecordsForChild(userId);
            else if (Session["OtherUserId"] != null)
                LoadAllVaccinationRecordsForUser(userId);
        }

        protected void txtFilter_TextChanged(object sender, EventArgs e)
        {
            int userId = GetUserIdFromToken();
            if (Session["ChildId"] != null)
                LoadAllVaccinationRecordsForChild(userId);
            else if (Session["OtherUserId"] != null)
                LoadAllVaccinationRecordsForUser(userId);
        }

        protected void btnExportPdf_Click(object sender, EventArgs e)
        {
            DataTable dt = (gvVaccinationRecords.DataSource as DataView)?.ToTable();

            if (dt == null || dt.Rows.Count == 0)
            {
                int userId = GetUserIdFromToken();
                if (Session["ChildId"] != null)
                    LoadAllVaccinationRecordsForChild(userId);
                else if (Session["OtherUserId"] != null)
                    LoadAllVaccinationRecordsForUser(userId);

                dt = (gvVaccinationRecords.DataSource as DataView)?.ToTable();
            }

            if (dt == null || dt.Rows.Count == 0) return;

            using (ClosedXML.Excel.XLWorkbook wb = new ClosedXML.Excel.XLWorkbook())
            {
                wb.Worksheets.Add(dt, "VaccinationRecords");

                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=VaccinationRecords.xlsx");

                using (MemoryStream ms = new MemoryStream())
                {
                    wb.SaveAs(ms);
                    ms.WriteTo(Response.OutputStream);
                    Response.Flush();
                    HttpContext.Current.ApplicationInstance.CompleteRequest(); // instead of Response.End()
                }
            }
        }


        protected void btnBack_Click(object sender, EventArgs e)
        {
            string role = Session["UserRole"] as string;
            switch (role)
            {
                case "Caregiver":
                    Response.Redirect("~/CaregiverDashboard.aspx");
                    break;
                case "Healthcare":
                    Response.Redirect("~/HealthcareDashboard.aspx");
                    break;
                case "Other":
                    Response.Redirect("~/OtherDashboard.aspx");
                    break;
                default:
                    Response.Redirect("~/LoginPage.aspx");
                    break;
            }
        }

        private void GenerateAndSaveQRCode()
        {
            int id = GetUserIdFromToken();
            if (id == -1) return;

            string encrypted = EncryptionHelper.Encrypt(id.ToString());
            string url = Request.Url.GetLeftPart(UriPartial.Authority) + "/ViewOtherRecord.aspx?token=" + encrypted;

            using (QRCodeGenerator qrGen = new QRCodeGenerator())
            {
                QRCodeData qrData = qrGen.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
                using (QRCode qr = new QRCode(qrData))
                using (Bitmap bmp = qr.GetGraphic(20))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bmp.Save(ms, ImageFormat.Png);
                        imgQRCode.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                    }

                    string folder = Server.MapPath("~/QRCodes/");
                    if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                    string filename = $"UserQRCode_{id}.png";
                    string path = Path.Combine(folder, filename);
                    bmp.Save(path, ImageFormat.Png);

                    lnkDownloadQRCode.NavigateUrl = ResolveUrl("~/QRCodes/" + filename);
                    lnkDownloadQRCode.Visible = true;
                }
            }
        }

        protected void gvVaccinationRecords_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedRecordId = Convert.ToInt32(gvVaccinationRecords.SelectedDataKey.Value);
            GenerateAndSaveQRCode(selectedRecordId);
        }

        private void GenerateAndSaveQRCode(int recordId)
        {
            string url = Request.Url.GetLeftPart(UriPartial.Authority) + "/ViewOtherRecord.aspx?recordId=" + recordId;

            using (QRCodeGenerator qrGen = new QRCodeGenerator())
            {
                QRCodeData qrData = qrGen.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
                Bitmap logo = new Bitmap(Server.MapPath("~/Content/logo.png"));

                using (QRCode qr = new QRCode(qrData))
                using (Bitmap bmp = qr.GetGraphic(20, Color.Black, Color.White, logo, 15, 6, false))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bmp.Save(ms, ImageFormat.Png);
                        imgQRCode.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                    }

                    string folder = Server.MapPath("~/QRCodes/");
                    if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                    string filename = $"VaccinationRecord_{recordId}.png";
                    string path = Path.Combine(folder, filename);
                    bmp.Save(path, ImageFormat.Png);

                    lnkDownloadQRCode.NavigateUrl = ResolveUrl("~/QRCodes/" + filename);
                    lnkDownloadQRCode.Visible = true;
                }
            }
        }
    }
}
