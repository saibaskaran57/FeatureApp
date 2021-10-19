namespace FeatureApp.Common.Helpers
{
    using System.ComponentModel.DataAnnotations;

    public static class EmailValidator
    {
        /// <summary>
        /// Validate email format.
        /// </summary>
        /// <param name="email">The email to be validated.</param>
        /// <returns>The validated email.</returns>
        public static bool IsValid(string email)
        {
            return new EmailAddressAttribute().IsValid(email);
        }
    }
}
