namespace FeatureApp.Common
{
    using System;

    public static class Guard
    {
        /// <summary>
        /// Verifies if a parameter is null. If null, throws ArgumentNullException.
        /// </summary>
        /// <param name="value">The value to be validated. </param>
        /// <param name="parameterName">The parameter name for error message.</param>
        public static void RequiresNotNull(object value, string parameterName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        /// <summary>
        /// Verifies if a parameters is empty. If empty, throws ArgumentNullException.
        /// </summary>
        /// <param name="value">The value to be validated. </param>
        /// <param name="parameterName">The parameter name for error message.</param>
        public static void RequiresNotEmpty(string value, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw (value == null)
                    ? new ArgumentNullException(parameterName)
                    : new ArgumentException("String was empty or whitespace", parameterName);
            }
        }
    }
}
