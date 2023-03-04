using gitguardian_integration.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;

namespace gitguardian_integration.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _iConfiguration;

        public HomeController(HttpClient httpClient, IConfiguration iconfiguration)
        {
            _httpClient = httpClient;
            _iConfiguration = iconfiguration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<GitGuardianResponseModel?> CheckSecretAsync(string text)
        {
            try
            {
                var payload = new { document = text };

                var httpRequestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://api.gitguardian.com/v1/scan"),
                    Headers = {
                        { HttpRequestHeader.Authorization.ToString(), $"Token {_iConfiguration.GetValue<string>("GitGuardianAccessToken")}" },
                    },
                    Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json")
                };

                var response = await _httpClient.SendAsync(httpRequestMessage);

                if (!response.IsSuccessStatusCode)
                    return null;

                var result = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<GitGuardianResponseModel>(result);

            }
            catch
            {
                return null;
            }
        }


        [HttpPost]
        public async Task<ActionResult> Check([FromForm] IFormCollection idobj)
        {
            if (idobj is null || (idobj.Count == 0 && !idobj.Files.Any())) 
            {
                return BadRequest(new { status = "error", message = "No data found" });
            }

            if (idobj.Count > 1 || idobj.Files.Count > 1)
            {
                return BadRequest(new { status = "error", message = "1 data at a time" });
            }

            var isFileBased = idobj.Files.Any();
            var isStringBased = !isFileBased;

            var queryString = string.Empty;

            if (isStringBased)
            {
                queryString = idobj.First().Value;

                if (string.IsNullOrWhiteSpace(queryString))
                {
                    return BadRequest(new { status = "error", message = "Text is empty" });
                }

            }

            if (isFileBased)
            {
                var file = idobj.Files.First();
                if (Helper.IsFileOverSize(file))
                {
                    return BadRequest(new { status = "error", message = "File cannot exceed 1 MB" });
                }

                if (!Helper.IsFileTextBased(file))
                {
                    return BadRequest(new { status = "error", message = "File must be text based" });
                }

                queryString = await Helper.ReadFileAsStringAsync(file);
            }

            var checkResult = await CheckSecretAsync(queryString!);

            if(checkResult is null)
                return BadRequest(new { status = "error", message = "Cannot check" });

            return Ok(checkResult);
        }
    }
}