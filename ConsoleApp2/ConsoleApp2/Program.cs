using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        using (HttpClient client = new HttpClient())
        {
            // Flask API adresi
            string apiUrl = "http://127.0.0.1:5000/chat";

            while (true) // Kullanıcı sürekli soru sorabilsin diye döngü
            {
                Console.Write("Sen: ");
                string userInput = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(userInput))
                    continue;

                var requestData = new { text = userInput };
                string json = JsonSerializer.Serialize(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                try
                {
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                    string result = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        using (JsonDocument doc = JsonDocument.Parse(result))
                        {
                            string botResponse = doc.RootElement.GetProperty("response").GetString();
                            Console.WriteLine("Bot: " + botResponse);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Hata: " + response.StatusCode);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Hata oluştu: " + ex.Message);
                }
            }
        }
    }
}
