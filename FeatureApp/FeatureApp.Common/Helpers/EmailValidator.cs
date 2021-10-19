using System.ComponentModel.DataAnnotations;

namespace FeatureApp.Common.Helpers
{
    public static class EmailValidator
    {
        public static bool IsValid(string email)
        {
            return new EmailAddressAttribute().IsValid(email);
        }
    }
}
