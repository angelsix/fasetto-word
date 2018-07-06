namespace Fasetto.Word.Web.Server
{
    /// <summary>
    /// An error response for a <see cref="SendGridResponse"/>
    /// </summary>
    public class SendGridResponseError
    {
        /// <summary>
        /// The error message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The field inside the email message details that the error is related to
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// Useful information for resolving the error
        /// </summary>
        public string Help { get; set; }
    }
}
