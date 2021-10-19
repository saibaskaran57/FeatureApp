namespace FeatureApp.Common.Models
{
    public enum ResultCode
    {
        /// <summary>
        /// Indicates operation is successful.
        /// </summary>
        Success = 0,

        /// <summary>
        /// Indicates operation is not modified.
        /// </summary>
        NotModified = 1,

        /// <summary>
        /// Indicates operation is modified.
        /// </summary>
        Modified = 2,
    }
}
