using System;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Threading.Tasks;

namespace CertificateAuth.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            HttpResponseMessage response = null;
            try
            {
                var cert = new X509Certificate2(Path.Combine("./Certificates/gouthammavuri.pfx"), "password");
                var handler = new HttpClientHandler();
                var client = new HttpClient(handler);

                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri("https://localhost:5001/WeatherForecast"),
                    Method = HttpMethod.Get,
                };
                request.Headers.Add("X-SSL-CERT", cert.GetRawCertDataString());
                response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var data = JsonDocument.Parse(responseContent);
                }

            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Status code: {response.StatusCode}, Error: {response.ReasonPhrase}");
            }
        }
    }
}
