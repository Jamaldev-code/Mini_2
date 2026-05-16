

namespace Mini_Project1.Methods
{
    internal class ValidateEmailMethod
    {
        public bool Execute(string email)
        {
            return email.Contains('@') &&
                   (email.EndsWith(".com") ||
                    email.EndsWith(".ru"));
        }
    }
}
