using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class TelerivetSmsSender
{
    private readonly string apiKey = "FNiEb_nnmxrnGNmMdKn7lhwHslaD3bMRHUZw";
    private readonly string projectId = "PJb5545c0519413aec";

    public async Task SendSmsAsync(string toPhone, string message)
    {
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri($"https://api.telerivet.com/v1/projects/{projectId}/");

            var byteArray = Encoding.ASCII.GetBytes($"{apiKey}:");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            var payload = new
            {
                to_number = toPhone,
                content = message
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync("messages/send", content);
            string responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode || responseString.Contains("\"error\""))
            {
                throw new Exception("SMS not accepted by Telerivet.");
            }
        }
    }
}
