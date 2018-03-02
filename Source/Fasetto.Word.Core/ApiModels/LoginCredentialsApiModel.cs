namespace Fasetto.Word.Core
{
    /// <summary>
    /// The credentials for  an API client to log into the server and receive a token back
    /// </summary>
    public class LoginCredentialsApiModel
    {
        #region Public Properties

        /// <summary>
        /// The users username or email
        /// </summary>
        public string UsernameOrEmail { get; set; }

        /// <summary>
        /// The users password
        /// </summary>
        public string Password { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public LoginCredentialsApiModel()
        {
            
        }

        #endregion
    }
}
