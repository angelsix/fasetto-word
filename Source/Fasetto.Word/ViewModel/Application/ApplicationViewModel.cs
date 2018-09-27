using Fasetto.Word.Core;
using System.Threading.Tasks;
using static Fasetto.Word.DI;
using static Fasetto.Word.Core.CoreDI;
using System.Windows.Input;

namespace Fasetto.Word
{
    /// <summary>
    /// The application state as a view model
    /// </summary>
    public class ApplicationViewModel : BaseViewModel
    {
        #region Private Members

        /// <summary>
        /// True if the settings menu should be shown
        /// </summary>
        private bool mSettingsMenuVisible;

        #endregion

        #region Public Properties

        /// <summary>
        /// The current page of the application
        /// </summary>
        public ApplicationPage CurrentPage { get; private set; } = ApplicationPage.Login;

        /// <summary>
        /// The view model to use for the current page when the CurrentPage changes
        /// NOTE: This is not a live up-to-date view model of the current page
        ///       it is simply used to set the view model of the current page 
        ///       at the time it changes
        /// </summary>
        public BaseViewModel CurrentPageViewModel { get; set; }

        /// <summary>
        /// True if the side menu should be shown
        /// </summary>
        public bool SideMenuVisible { get; set; } = false;

        /// <summary>
        /// True if the settings menu should be shown
        /// </summary>
        public bool SettingsMenuVisible
        {
            get => mSettingsMenuVisible;
            set
            {
                // If property has not changed...
                if (mSettingsMenuVisible == value)
                    // Ignore
                    return;

                // Set the backing field
                mSettingsMenuVisible = value;

                // If the settings menu is now visible...
                if (value)
                    // Reload settings
                    TaskManager.RunAndForget(ViewModelSettings.LoadAsync);
            }
        }

        /// <summary>
        /// Determines the currently visible side menu content
        /// </summary>
        public SideMenuContent CurrentSideMenuContent { get; set; } = SideMenuContent.Chat;

        /// <summary>
        /// Determines if the application has network access to the fasetto server
        /// </summary>
        public bool ServerReachable { get; set; } = true;

        #endregion

        #region Public Commands

        /// <summary>
        /// The command to change the side menu to the Chat
        /// </summary>
        public ICommand OpenChatCommand { get; set; }

        /// <summary>
        /// The command to change the side menu to the Contacts
        /// </summary>
        public ICommand OpenContactsCommand { get; set; }

        /// <summary>
        /// The command to change the side menu to Media
        /// </summary>
        public ICommand OpenMediaCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// The default constructor
        /// </summary>
        public ApplicationViewModel()
        {
            // Create the commands
            OpenChatCommand = new RelayCommand(OpenChat);
            OpenContactsCommand = new RelayCommand(OpenContacts);
            OpenMediaCommand = new RelayCommand(OpenMedia);
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Changes the current side menu to the Chat
        /// </summary>
        public void OpenChat()
        {
            // Set the current side menu to Chat
            CurrentSideMenuContent = SideMenuContent.Chat;
        }

        /// <summary>
        /// Changes the current side menu to the Contacts
        /// </summary>
        public void OpenContacts()
        {
            // Set the current side menu to Chat
            CurrentSideMenuContent = SideMenuContent.Contacts;
        }

        /// <summary>
        /// Changes the current side menu to Media
        /// </summary>
        public void OpenMedia()
        {
            // Set the current side menu to Chat
            CurrentSideMenuContent = SideMenuContent.Media;
        }

        #endregion

        #region Public Helper Methods

        /// <summary>
        /// Navigates to the specified page
        /// </summary>
        /// <param name="page">The page to go to</param>
        /// <param name="viewModel">The view model, if any, to set explicitly to the new page</param>
        public void GoToPage(ApplicationPage page, BaseViewModel viewModel = null)
        {
            // Always hide settings page if we are changing pages
            SettingsMenuVisible = false;

            // Set the view model
            CurrentPageViewModel = viewModel;

            // See if page has changed
            var different = CurrentPage != page;

            // Set the current page
            CurrentPage = page;

            // If the page hasn't changed, fire off notification
            // So pages still update if just the view model has changed
            if (!different)
                OnPropertyChanged(nameof(CurrentPage));

            // Show side menu or not?
            SideMenuVisible = page == ApplicationPage.Chat;

        }

        /// <summary>
        /// Handles what happens when we have successfully logged in
        /// </summary>
        /// <param name="loginResult">The results from the successful login</param>
        public async Task HandleSuccessfulLoginAsync(UserProfileDetailsApiModel loginResult)
        {
            // Store this in the client data store
            await ClientDataStore.SaveLoginCredentialsAsync(loginResult.ToLoginCredentialsDataModel());

            // Load new settings
            await ViewModelSettings.LoadAsync();

            // Go to chat page
            ViewModelApplication.GoToPage(ApplicationPage.Chat);
        }

        #endregion
    }
}
