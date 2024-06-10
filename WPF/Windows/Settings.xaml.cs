using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using WinForms.Forms;
using WPF.Helper;

namespace WPF.Windows
{
   
    public partial class Settings
    {
        private readonly IFileRepository _repository = RepositoryFactory.GetRepository();


        
        public Settings()
        {
            new CultureSetter().SetCulture(_repository);
            InitializeComponent();
            InitializeCheckBoxes();
            InitializeSizeSetting();
        }

        private void InitializeCheckBoxes()
        {
            ChkMale.IsChecked = true;
            ChkEnglish.IsChecked = true;
            ChkSmall.IsChecked = true;
        }

        private void InitializeSizeSetting()
        {
            ChkSmall.Content = Properties.Resources.appSizeSmall + " (800x600)";
            ChkMedium.Content = Properties.Resources.appSizeMedium + " (1024x768)";
            ChkLarge.Content = Properties.Resources.appSizeLarge + " (1280x1024)";
            ChkFullScreen.Content = Properties.Resources.appSizeFullScreen +"("+ SystemParameters.PrimaryScreenWidth+"x"+ SystemParameters.PrimaryScreenHeight+ ")";

        }
        private void TournamentTypeCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender != ChkMale) ChkMale.IsChecked = false;
            if (sender != ChkFemale) ChkFemale.IsChecked = false;
        }

        private void LanguageCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender != ChkEnglish) ChkEnglish.IsChecked = false;
            if (sender != ChkCroatian) ChkCroatian.IsChecked = false;
        }

        private void AppSizeCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender != ChkSmall) ChkSmall.IsChecked = false;
            if (sender != ChkMedium) ChkMedium.IsChecked = false;
            if (sender != ChkFullScreen) ChkFullScreen.IsChecked = false;
            
        }

        private void Settings_OnClosing(object sender, CancelEventArgs e )
        {
            var confirmResult = MessageBox.Show(
                Properties.Resources.settingsClosingBody,
                Properties.Resources.settingsClosingTitle,
                MessageBoxButton.OKCancel, MessageBoxImage.Question);

            if (confirmResult != MessageBoxResult.OK) e.Cancel = true;
        }

        private void BtnSettingsSave_OnClick(object sender, RoutedEventArgs e)
        {
            var confirmResult = MessageBox.Show(Properties.Resources.settingsMsgBoxText, Properties.Resources.settingsMsgBoxCaption, MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (confirmResult != MessageBoxResult.OK) return;

            try
            {
                var tournamentType = PnlTournamentType.Children.OfType<CheckBox>()
                    .FirstOrDefault(r => r.IsChecked != null && (bool)r.IsChecked)?.Tag.ToString();

                var language = PnlLanguage.Children.OfType<CheckBox>()
                    .FirstOrDefault(r => r.IsChecked != null && (bool)r.IsChecked)?.Tag.ToString();

                var appSize = PnlAppSize.Children.OfType<CheckBox>()
                    .FirstOrDefault(r => r.IsChecked != null && (bool)r.IsChecked)?.Tag.ToString();
 
                _repository.SaveSettings(tournamentType, language);
                _repository.SaveApplicationSize(appSize);
            }
            catch (Exception ex) when (ex is ArgumentNullException || ex is IOException || ex is CultureNotFoundException)
            {
                MessageBox.Show("Unexpected error occured", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }


            Hide();
            new MainForm().ShowDialog();
            Close();
        }


    }
}
