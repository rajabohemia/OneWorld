using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace OneWorld.Helpers
{
    public class MiscActions
    {
        public static string GenerateRandomString(int length)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static StringContent GenerateStringContent(object obj)
        {
            var jsonObj = JsonSerializer.Serialize(obj);
            return new StringContent(jsonObj, Encoding.UTF8, "application/json");
        }
    }
}