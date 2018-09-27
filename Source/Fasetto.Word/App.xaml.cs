using Dna;
using Fasetto.Word.Core;
using Fasetto.Word.Relational;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using System.Windows;
using static Dna.FrameworkDI;
using static Fasetto.Word.Core.CoreDI;
using static Fasetto.Word.DI;

namespace Fasetto.Word
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Custom startup so we load our IoC immediately before anything else
        /// </summary>
        /// <param name="e"></param>
        protected override async void OnStartup(StartupEventArgs e)
        {
            // Let the base application do what it needs
            base.OnStartup(e);

            // Setup the main application 
            await ApplicationSetupAsync();

            // Log it
            Logger.LogDebugSource("Application starting...");

            // Setup the application view model based on if we are logged in
            ViewModelApplication.GoToPage(
                // If we are logged in...
                await ClientDataStore.HasCredentialsAsync() ?
                // Go to chat page
                ApplicationPage.Chat : 
                // Otherwise, go to login page
                ApplicationPage.Login);

            // Show the main window
            Current.MainWindow = new MainWindow();
            Current.MainWindow.Show();
        }

        /// <summary>
        /// Configures our application ready for use
        /// </summary>
        private async Task ApplicationSetupAsync()
        {
            // Setup the Dna Framework
            Framework.Construct<DefaultFrameworkConstruction>()
                .AddFileLogger()
                .AddClientDataStore()
                .AddFasettoWordViewModels()
                .AddFasettoWordClientServices()
                .Build();

            // Ensure the client data store 
            await ClientDataStore.EnsureDataStoreAsync();

            // Monitor for server connection status
            MonitorServerStatus();

            // Load new settings
            TaskManager.RunAndForget(ViewModelSettings.LoadAsync);
        }

        /// <summary>
        /// Monitors the fasetto website is up, running and reachable
        /// by periodically hitting it up
        /// </summary>
        private void MonitorServerStatus()
        {
            // Create a new endpoint watcher
            var httpWatcher = new HttpEndpointChecker(
                // Checking fasetto.chat
                Configuration["FasettoWordServer:HostUrl"],
                // Every 20 seconds
                interval: 20000,
                // Pass in the DI logger
                logger: Framework.Provider.GetService<ILogger>(),
                // On change...
                stateChangedCallback: (result) =>
                {
                    // Update the view model property with the new result
                    ViewModelApplication.ServerReachable = result;
                });
        }
    }
}
