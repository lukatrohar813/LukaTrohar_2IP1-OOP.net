namespace WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private readonly IFileRepository _repository = RepositoryFactory.GetRepository();

        private const string FormsFolder = @"Windows/";
        private const string SettingsWindow = @"Settings.xaml";
        private const string MainForm = @"MainForm.xaml";
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            var uriString = _repository.DoSettingsExist()
                ? $"{FormsFolder}{MainForm}"
                : $"{FormsFolder}{SettingsWindow}";

            StartupUri = new Uri(uriString, UriKind.Relative);
        }
    }
}
