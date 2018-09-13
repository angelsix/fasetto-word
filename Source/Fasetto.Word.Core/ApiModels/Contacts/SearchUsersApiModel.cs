namespace Fasetto.Word.Core
{
    /// <summary>
    /// Details used to search for a user
    /// </summary>
    public class SearchUsersApiModel
    {
        #region Public Properties

        /// <summary>
        /// The users username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The users first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The users last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The users email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The users phone number
        /// </summary>
        public string PhoneNumber { get; set; }

        #endregion
    }
}
