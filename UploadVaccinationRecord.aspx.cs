using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;

namespace ZimVaxSync
{
    public partial class UploadVaccinationRecord : System.Web.UI.Page
    {
        private int userId;

        protected void Page_Load(object sender, EventArgs e)
        {
            userId = Session["UserID"] != null ? Convert.ToInt32(Session["UserID"]) : 1; // Replace with actual session logic
            if (!IsPostBack)
            {
                LoadRecords();
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (!fileUpload.HasFile)
            {
                lblMessage.Text = "Please select a file.";
                return;
            }

            string ext = Path.GetExtension(fileUpload.FileName).ToLower();
            string[] allowed = { ".pdf", ".doc", ".docx", ".jpg", ".jpeg", ".png" };
            if (Array.IndexOf(allowed, ext) < 0)
            {
                lblMessage.Text = "Only PDF, Word, and image files are allowed.";
                return;
            }

            string title = txtTitle.Text.Trim();
            string description = txtDescription.Text.Trim();
            string fileName = Guid.NewGuid() + ext;
            string savePath = "~/UploadedRecords/" + fileName;
            string fullPath = Server.MapPath(savePath);

            try
            {
                Directory.CreateDirectory(Server.MapPath("~/UploadedRecords/"));
                fileUpload.SaveAs(fullPath);

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Zimvaxsync"].ConnectionString))
                {
                    string query = "INSERT INTO UploadedVaccinationRecord (UserID, Title, Description, FilePath, UploadDate) VALUES (@UserID, @Title, @Description, @FilePath, @UploadDate)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    cmd.Parameters.AddWithValue("@Title", title);
                    cmd.Parameters.AddWithValue("@Description", description);
                    cmd.Parameters.AddWithValue("@FilePath", savePath);
                    cmd.Parameters.AddWithValue("@UploadDate", DateTime.Now);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                lblMessage.Text = "File uploaded successfully.";
                txtTitle.Text = "";
                txtDescription.Text = "";
                LoadRecords();
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error: " + ex.Message;
            }
        }

        protected void LoadRecords(string keyword = "")
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Zimvaxsync"].ConnectionString))
            {
                string query = "SELECT RecordID, Title, Description, FilePath, UploadDate FROM UploadedVaccinationRecord WHERE UserID = @UserID";

                if (!string.IsNullOrEmpty(keyword))
                    query += " AND (Title LIKE @Keyword OR Description LIKE @Keyword)";

                query += " ORDER BY UploadDate DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", userId);
                if (!string.IsNullOrEmpty(keyword))
                    cmd.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");

                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvRecords.DataSource = dt;
                gvRecords.DataBind();
            }
        }

        protected void gvRecords_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            gvRecords.PageIndex = e.NewPageIndex;
            LoadRecords(txtSearch.Text.Trim());
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadRecords(txtSearch.Text.Trim());
        }

        protected void gvRecords_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteRecord")
            {
                int recordId = Convert.ToInt32(e.CommandArgument);
                DeleteRecord(recordId);
            }
        }

        private void DeleteRecord(int recordId)
        {
            string filePath = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Zimvaxsync"].ConnectionString))
            {
                conn.Open();

                SqlCommand selectCmd = new SqlCommand("SELECT FilePath FROM UploadedVaccinationRecord WHERE RecordID = @RecordID AND UserID = @UserID", conn);
                selectCmd.Parameters.AddWithValue("@RecordID", recordId);
                selectCmd.Parameters.AddWithValue("@UserID", userId);

                object result = selectCmd.ExecuteScalar();
                if (result != null)
                    filePath = Server.MapPath(result.ToString());

                SqlCommand deleteCmd = new SqlCommand("DELETE FROM UploadedVaccinationRecord WHERE RecordID = @RecordID AND UserID = @UserID", conn);
                deleteCmd.Parameters.AddWithValue("@RecordID", recordId);
                deleteCmd.Parameters.AddWithValue("@UserID", userId);
                deleteCmd.ExecuteNonQuery();
            }

            if (File.Exists(filePath))
                File.Delete(filePath);

            lblMessage.Text = "Record deleted.";
            LoadRecords(txtSearch.Text.Trim());
        }
    }
}
