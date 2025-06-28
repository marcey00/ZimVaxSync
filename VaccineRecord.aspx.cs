using System;
using System.Configuration;
using System.Data.SqlClient;

namespace ZimVaxSync
{
    public partial class VaccineRecord : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["Zimvaxsync"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadUserDetails();
            }
        }

        private void LoadUserDetails()
        {
            string userId = Request.QueryString["UserID"];
            if (string.IsNullOrEmpty(userId)) return;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT FullName, Gender, DateOfBirth, NationalID, PhoneNumber, Address FROM OtherGeneralUser WHERE OtherUserID = @UserID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", userId);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    lblFullName.Text = reader["FullName"].ToString();
                    lblGender.Text = reader["Gender"].ToString();
                    lblDOB.Text = Convert.ToDateTime(reader["DateOfBirth"]).ToString("yyyy-MM-dd");
                    lblNationalID.Text = reader["NationalID"].ToString();
                    lblPhone.Text = reader["PhoneNumber"].ToString();
                    lblAddress.Text = reader["Address"].ToString();
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string userId = Request.QueryString["UserID"];
            if (string.IsNullOrEmpty(userId)) return;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"INSERT INTO VaccinationRecord (UserID, FullName, Gender, DateOfBirth, NationalID, PhoneNumber, Address,
                                  VaccineName, DoseNumber, DateGiven, BatchNumber, Manufacturer, AdministeredBy, NextDoseDue, FacilityName, Comments)
                                 VALUES (@UserID, @FullName, @Gender, @DOB, @NationalID, @Phone, @Address, 
                                         @VaccineName, @DoseNumber, @DateGiven, @BatchNumber, @Manufacturer, @AdministeredBy, @NextDose, @FacilityName, @Comments)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", userId);
                cmd.Parameters.AddWithValue("@FullName", lblFullName.Text);
                cmd.Parameters.AddWithValue("@Gender", lblGender.Text);
                cmd.Parameters.AddWithValue("@DOB", Convert.ToDateTime(lblDOB.Text));
                cmd.Parameters.AddWithValue("@NationalID", lblNationalID.Text);
                cmd.Parameters.AddWithValue("@Phone", lblPhone.Text);
                cmd.Parameters.AddWithValue("@Address", lblAddress.Text);
                cmd.Parameters.AddWithValue("@VaccineName", txtVaccineName.Text.Trim());
                cmd.Parameters.AddWithValue("@DoseNumber", int.Parse(txtDoseNumber.Text.Trim()));
                cmd.Parameters.AddWithValue("@DateGiven", Convert.ToDateTime(txtDateGiven.Text));
                cmd.Parameters.AddWithValue("@BatchNumber", txtBatchNumber.Text.Trim());
                cmd.Parameters.AddWithValue("@Manufacturer", txtManufacturer.Text.Trim());
                cmd.Parameters.AddWithValue("@AdministeredBy", txtAdministeredBy.Text.Trim());
                cmd.Parameters.AddWithValue("@NextDose", string.IsNullOrEmpty(txtNextDose.Text) ? DBNull.Value : (object)Convert.ToDateTime(txtNextDose.Text));
                cmd.Parameters.AddWithValue("@FacilityName", txtFacilityName.Text.Trim());
                cmd.Parameters.AddWithValue("@Comments", txtComments.Text.Trim());

                conn.Open();
                cmd.ExecuteNonQuery();
                lblMessage.Text = "Vaccination record saved successfully!";
            }
        }
    }
}
