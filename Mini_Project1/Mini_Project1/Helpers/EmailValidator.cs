using System.Net.Mail; //.NET-in öz sinfidir — bütün domain-ləri düzgün yoxlayır, arasdirmisam

namespace Mini_Project1.Helpers
{
    internal static class EmailValidator
    {
        public static bool IsValid(string email)
        {
            try
            {
                _ = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
