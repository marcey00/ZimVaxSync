using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace VaccinationReminderApp
{
    class Program
    {
        static string connectionString = "Data Source=MARCEY-PC\\SQLEXPRESS;Initial Catalog=Zimvaxsync;Integrated Security=True;TrustServerCertificate=True;";

        static async Task Main(string[] args)
        {
            try
            {
                LogToFile("Vaccination Reminder Job Started.");
                await RunReminderJobAsync();
                CleanupOldSchedules();
                LogToFile("Reminder job completed successfully.");
            }
            catch (Exception ex)
            {
                LogToFile("FATAL ERROR: " + ex);
                SendFailureEmail(ex.ToString());
            }
        }

        static async Task RunReminderJobAsync()
        {
            string query = @"
                SELECT * FROM (
                    -- Child-specific vaccination records
                    SELECT 
                        cp.FullName AS RecipientName,
                        cg.PhoneNumber,
                        cvr.VaccineName,
                        cvr.NextDoseDate
                    FROM ChildVaccinationRecords cvr
                    INNER JOIN ChildProfile cp ON cvr.ChildID = cp.ChildID
                    INNER JOIN Caregiver cg ON cp.CaregiverId = cg.CaregiverId
                    WHERE cvr.NextDoseDate IS NOT NULL

                    UNION

                    -- Child vaccinations stored in VaccinationRecords
                    SELECT 
                        cp.FullName AS RecipientName,
                        cg.PhoneNumber,
                        vr.VaccineName,
                        vr.NextDoseDue AS NextDoseDate
                    FROM VaccinationRecords vr
                    INNER JOIN ChildProfile cp ON vr.ChildID = cp.ChildID
                    INNER JOIN Caregiver cg ON cp.CaregiverId = cg.CaregiverId
                    WHERE vr.NextDoseDue IS NOT NULL AND vr.ChildID IS NOT NULL

                    UNION

                    -- General users
                    SELECT 
                        ou.FullName AS RecipientName,
                        ou.PhoneNumber,
                        vr.VaccineName,
                        vr.NextDoseDue AS NextDoseDate
                    FROM VaccinationRecords vr
                    INNER JOIN OtherGeneralUser ou ON vr.OtherUserID = ou.OtherUserID
                    WHERE vr.NextDoseDue IS NOT NULL
                ) AS Reminders
                WHERE CAST(Reminders.NextDoseDate AS DATE)
                BETWEEN CAST(GETDATE() AS DATE) AND DATEADD(DAY, 3, CAST(GETDATE() AS DATE))";

            Console.WriteLine("Reminder query executing...");
            Console.WriteLine($"Looking for reminders due from {DateTime.Today:yyyy-MM-dd} to {DateTime.Today.AddDays(3):yyyy-MM-dd}");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                int matchCount = 0;

                while (reader.Read())
                {
                    matchCount++;
                    string name = reader["RecipientName"]?.ToString() ?? "";
                    string rawPhone = reader["PhoneNumber"]?.ToString() ?? "";
                    string phone = CleanPhoneNumber(rawPhone);
                    string vaccine = reader["VaccineName"]?.ToString() ?? "";
                    DateTime nextDate = Convert.ToDateTime(reader["NextDoseDate"]);

                    int daysUntilDue = (nextDate.Date - DateTime.Today).Days;

                    string? notificationType = daysUntilDue switch
                    {
                        3 => "3-day",
                        2 => "2-day",
                        1 => "1-day",
                        0 => "Day-of",
                        _ => null
                    };

                    string? message = daysUntilDue switch
                    {
                        3 => $"Hi {name}, your next dose of {vaccine} is due in 3 days on {nextDate:yyyy-MM-dd}. Regards, ZimVaxSync team.",
                        2 => $"Hi {name}, your next dose of {vaccine} is due in 2 days on {nextDate:yyyy-MM-dd}. Regards, ZimVaxSync team.",
                        1 => $"Hi {name}, your next dose of {vaccine} is due tomorrow. Regards, ZimVaxSync team.",
                        0 => $"Hi {name}, your next dose of {vaccine} is due today. Please visit your clinic. Regards, ZimVaxSync team.",
                        _ => null
                    };

                    Console.WriteLine($"[MATCH FOUND] Name={name}, Phone={rawPhone}, Cleaned={phone}, Vaccine={vaccine}, Due={nextDate:yyyy-MM-dd}");

                    if (!string.IsNullOrWhiteSpace(phone) && IsValidZimPhoneNumber(phone) && !string.IsNullOrWhiteSpace(message))
                    {
                        try
                        {
                            await new TelerivetSmsSender().SendSmsAsync(phone, message);
                            LogNotification(phone, vaccine, notificationType!);
                            LogToFile($"Reminder sent to {phone} for {vaccine} ({notificationType})");
                            Console.WriteLine("✅ SMS sent successfully.");
                        }
                        catch (Exception ex)
                        {
                            LogToFile($"Failed to send SMS to {phone}: {ex.Message}");
                            Console.WriteLine("❌ SMS ERROR: " + ex.Message);
                        }
                    }
                    else
                    {
                        Console.WriteLine("⚠️ Skipped: Missing phone, message, or invalid number.");
                    }
                }

                Console.WriteLine($"✅ Total matching reminders: {matchCount}");
                if (matchCount == 0)
                {
                    Console.WriteLine("ℹ️ No reminders matched the due dates.");
                }
            }
        }

        static string CleanPhoneNumber(string rawPhone)
        {
            if (string.IsNullOrWhiteSpace(rawPhone)) return "";
            string cleaned = Regex.Replace(rawPhone.Trim(), "[^0-9+]", "");
            if (cleaned.StartsWith("07")) cleaned = "+263" + cleaned.Substring(1);
            else if (cleaned.StartsWith("263")) cleaned = "+" + cleaned;
            else if (!cleaned.StartsWith("+263")) cleaned = "+263" + cleaned;
            return cleaned;
        }

        static bool IsValidZimPhoneNumber(string phone)
        {
            return Regex.IsMatch(phone, @"^\+2637[1-9][0-9]{7}$");
        }

        static void LogNotification(string phone, string vaccine, string type)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(@"
                        INSERT INTO NotificationLog (RecipientPhone, VaccineName, NotificationType, DateSent)
                        VALUES (@Phone, @Vaccine, @Type, GETDATE())", conn);
                    cmd.Parameters.AddWithValue("@Phone", phone);
                    cmd.Parameters.AddWithValue("@Vaccine", vaccine);
                    cmd.Parameters.AddWithValue("@Type", type);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                LogToFile("❌ Failed to log notification: " + ex.Message);
                Console.WriteLine("❌ Failed to log notification: " + ex.Message);
            }
        }

        static void LogToFile(string message)
        {
            string path = @"C:\Logs\ZimVaxSync_SMS_Log.txt";
            string? dir = Path.GetDirectoryName(path);
            if (dir != null) Directory.CreateDirectory(dir);
            File.AppendAllText(path, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}{Environment.NewLine}");
        }

        static void CleanupOldSchedules()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"
                    DELETE FROM VaccinationSchedule
                    WHERE DueDate < DATEADD(DAY, -30, GETDATE()) AND Status = 'Pending'", conn);
                int count = cmd.ExecuteNonQuery();
                LogToFile($"[Cleanup] Deleted {count} old pending schedules.");
            }
        }

        static void SendFailureEmail(string errorDetails)
        {
            try
            {
                using (var client = new SmtpClient("smtp.gmail.com", 587))
                {
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential("muhuromarceline@gmail.com", "ocdc mqmu pdwc mcih");
                    client.Timeout = 30000;

                    var mail = new MailMessage("muhuromarceline@gmail.com", "muhuromarceline@gmail.com")
                    {
                        Subject = "ZimVaxSync Reminder Job Failed",
                        Body = $"An error occurred:\n\n{errorDetails}"
                    };

                    client.Send(mail);
                }
            }
            catch (Exception ex)
            {
                LogToFile("ERROR sending failure email: " + ex);
            }
        }
    }

    public class TelerivetSmsSender
    {
        private readonly string apiKey = "FNiEb_nnmxrnGNmMdKn7lhwHslaD3bMRHUZw";
        private readonly string projectId = "PJb5545c0519413aec";

        public async Task SendSmsAsync(string toPhone, string message)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri($"https://api.telerivet.com/v1/projects/{projectId}/");

            var byteArray = Encoding.ASCII.GetBytes($"{apiKey}:");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            var payload = new { to_number = toPhone, content = message };
            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync("messages/send", content);
            string responseString = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"Telerivet response: {response.StatusCode} - {responseString}");

            if (!response.IsSuccessStatusCode || responseString.Contains("\"error\""))
            {
                throw new Exception("SMS not accepted by Telerivet. Full response: " + responseString);
            }
        }
    }
}
