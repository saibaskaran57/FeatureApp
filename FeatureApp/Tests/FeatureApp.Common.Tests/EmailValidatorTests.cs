namespace FeatureApp.Common.Tests
{
    using FeatureApp.Common.Helpers;
    using Xunit;

    public class EmailValidatorTests
    {
        [Theory]
        [InlineData("test@gmail.com")]
        [InlineData("test.123@example.com")]
        [InlineData("disposable.style.email.with+symbol@example.com")]
        [InlineData("admin@mailserver1")]
        [InlineData("mailhost!username@example.org")]
        public void ShouldValidateTrueForCorrectEmailFormat(string email)
        {
            var actual = EmailValidator.IsValid(email);

            Assert.True(actual);
        }

        [Theory]
        [InlineData("test")]
        [InlineData("Abc.example.com")]
        [InlineData("test@@gmail.com")]
        public void ShouldValidateFalseForBadEmailFormat(string email)
        {
            var actual = EmailValidator.IsValid(email);

            Assert.False(actual);
        }
    }
}
