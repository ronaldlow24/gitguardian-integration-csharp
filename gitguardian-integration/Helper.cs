using gitguardian_integration.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net;
using System.Text.Json;
using System.Text;

namespace gitguardian_integration
{
    public class Helper
    {
        public static bool IsFileOverSize(IFormFile file, int maxFileSizeInBytes = 1000000) // 1MB
        {
            return file.Length > maxFileSizeInBytes;
        }

        public static bool IsFileTextBased(IFormFile file)
        {
            return file.ContentType.Contains("text");
        }

        public static async Task<string> ReadFileAsStringAsync(IFormFile file)
        {
            var result = new StringBuilder();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                    result.AppendLine(await reader.ReadLineAsync());
            }
            return result.ToString();
        }

    }
}
