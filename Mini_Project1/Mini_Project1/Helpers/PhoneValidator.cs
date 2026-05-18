using System.Linq;

namespace Mini_Project1.Helpers
{
    internal static class PhoneValidator
    {
        public static bool IsValid(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;

            //if (!phoneNumber.All(char.IsDigit))
            //    return false;

            if (phoneNumber.Length != 13)
                return false;

            string[] prefixes =
            {
                "+99450","+99451","+99455","+99470","+99477","+99499","+99410"
            };
            if (!prefixes.Any(phoneNumber.StartsWith))
                return false;

            string numbers = phoneNumber.Replace("+", "");

            return numbers.All(char.IsDigit);

        }
    }
}