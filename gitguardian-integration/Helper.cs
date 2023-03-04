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
            //check if it is binary, if is binary, it is not text based
            const int charsToCheck = 8000;
            const char nulChar = '\0';

            using var streamReader = new StreamReader(file.OpenReadStream());

            for (var i = 0; i < charsToCheck; i++)
            {
                if (streamReader.EndOfStream)
                    return true;

                if ((char)streamReader.Read() == nulChar)
                {
                    return false;
                }
            }

            return true;
        }

        public static async Task<string> ReadFileAsStringAsync(IFormFile file)
        {
            using var reader = new StreamReader(file.OpenReadStream());
            return await reader.ReadToEndAsync();
        }

    }
}
