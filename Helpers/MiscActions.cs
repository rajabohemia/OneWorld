using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using OneWorld.DTO;

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

        public static string Encode64(string stringToEncode)
        {
            var stringBytes = Encoding.UTF8.GetBytes(stringToEncode);
            return Convert.ToBase64String(stringBytes);
        }

        public static DecodeResult Decode64(string stringToDecode)
        {
            try
            {
                var encodedBytes = Convert.FromBase64String(stringToDecode);
                var decodedString = Encoding.UTF8.GetString(encodedBytes);
                return new DecodeResult(true) {DecodedString = decodedString};
            }
            catch (Exception e)
            {
                return new DecodeResult() {Errors = new[] {"Invalid Strings to Convert."}};
            }
        }
    }

    public class DecodeResult : BaseErrorSuccess
    {
        public DecodeResult()
        {
        }

        public DecodeResult(bool succees)
        {
            Success = succees;
        }

        public string DecodedString { get; set; }
    }
}