namespace Fasetto.Word.Core
{
    /// <summary>
    /// The response for all Web API calls made
    /// </summary>
    public class ApiResponse
    {
        #region Public Properties

        /// <summary>
        /// Indicates if the API call was successful
        /// </summary>
        public bool Successful => ErrorMessage == null;

        /// <summary>
        /// The error message for a failed API call
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// The API response object
        /// </summary>
        public object Response { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ApiResponse()
        {
            
        }

        #endregion
    }

    /// <summary>
    /// The response for all Web API calls made
    /// with a specific type of known response
    /// </summary>
    /// <typeparam name="T">The specific type of server response</typeparam>
    public class ApiResponse<T> : ApiResponse
    {
        /// <summary>
        /// The API response object as T
        /// </summary>
        public new T Response { get => (T)base.Response; set => base.Response = value; }
    }
}
