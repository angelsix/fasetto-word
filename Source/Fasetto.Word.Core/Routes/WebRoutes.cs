namespace Fasetto.Word.Core
{
    /// <summary>
    /// The relative routes to all normal (non-API) calls in the server
    /// </summary>
    public static class WebRoutes
    {
        /// <summary>
        /// The route to the CreateUser method
        /// </summary>
        public const string CreateUser = "/user/create";

        /// <summary>
        /// The route to the Logout method
        /// </summary>
        public const string Logout = "/logout";

        /// <summary>
        /// The route to the Login method
        /// </summary>
        public const string Login = "/login";
    }
}