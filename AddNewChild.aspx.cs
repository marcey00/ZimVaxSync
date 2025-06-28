using System;
using System.Configuration;
using System.Data.SqlClient;

namespace ZimVaxSync
{
    public partial class AddNewChild : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("HealthcareDashboard.aspx");
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Zimvaxsync"].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Insert caregiver info into Caregiver table
                    string insertCaregiverQuery = @"
                        INSERT INTO Caregiver (FullName, PhoneNumber, Email, Address)
                        OUTPUT INSERTED.CaregiverID
                        VALUES (@FullName, @PhoneNumber, @Email, @Address)";

                    int caregiverId;

                    using (SqlCommand cmdCaregiver = new SqlCommand(insertCaregiverQuery, conn))
                    {
                        cmdCaregiver.Parameters.AddWithValue("@FullName", txtFullName.Text.Trim());
                        cmdCaregiver.Parameters.AddWithValue("@PhoneNumber", txtPhoneNumber.Text.Trim());
                        cmdCaregiver.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                        cmdCaregiver.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());

                        object result = cmdCaregiver.ExecuteScalar();
                        if (result != null)
                        {
                            caregiverId = Convert.ToInt32(result);
                        }
                        else
                        {
                            lblMessage.ForeColor = System.Drawing.Color.Red;
                            lblMessage.Text = "Failed to insert caregiver information.";
                            return;
                        }
                    }

                    // Insert child info into ChildProfile table
                    string gender = rdoMale.Checked ? "Male" : rdoFemale.Checked ? "Female" : "";

                    string insertChildQuery = @"
                        INSERT INTO ChildProfile
                        (FullName, DOB, Gender, IDNumber, RelationshipToChild, CaregiverName, PhoneNumber, Email, Address, CaregiverID)
                        VALUES
                        (@FullName, @DOB, @Gender, @IDNumber, @Relationship, @CaregiverName, @PhoneNumber, @Email, @Address, @CaregiverID)";

                    using (SqlCommand cmdChild = new SqlCommand(insertChildQuery, conn))
                    {
                        cmdChild.Parameters.AddWithValue("@FullName", txtChildName.Text.Trim());
                        cmdChild.Parameters.AddWithValue("@DOB", txtChildDOB.Text.Trim());
                        cmdChild.Parameters.AddWithValue("@Gender", gender);
                        cmdChild.Parameters.AddWithValue("@IDNumber", txtChildID.Text.Trim());
                        cmdChild.Parameters.AddWithValue("@Relationship", txtRelationship.Text.Trim());
                        cmdChild.Parameters.AddWithValue("@CaregiverName", txtFullName.Text.Trim());
                        cmdChild.Parameters.AddWithValue("@PhoneNumber", txtPhoneNumber.Text.Trim());
                        cmdChild.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                        cmdChild.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                        cmdChild.Parameters.AddWithValue("@CaregiverID", caregiverId);

                        int childResult = cmdChild.ExecuteNonQuery();

                        if (childResult > 0)
                        {
                            lblMessage.ForeColor = System.Drawing.Color.Green;
                            lblMessage.Text = "Child and caregiver successfully registered.";
                            ClearFields();
                        }
                        else
                        {
                            lblMessage.ForeColor = System.Drawing.Color.Red;
                            lblMessage.Text = "Child registration failed. Please try again.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Error: " + ex.Message;
            }
        }

        private void ClearFields()
        {
            txtChildName.Text = "";
            txtChildDOB.Text = "";
            rdoMale.Checked = false;
            rdoFemale.Checked = false;
            txtChildID.Text = "";
            txtRelationship.Text = "";
            txtFullName.Text = "";
            txtPhoneNumber.Text = "";
            txtEmail.Text = "";
            txtAddress.Text = "";
        }
    }
}

