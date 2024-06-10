using System.Globalization;
using DAL.Api;
using DAL.Repos;
using WinForms.HelperClasses;

namespace WinForms.Forms
{
    public partial class InitialSettingsForm : Form
    {
        private readonly IFileRepository _repository;
        private readonly IApi _api;

        public InitialSettingsForm(IFileRepository repo, IApi api)
        {
            _repository = repo;
            _api = api;
            InitializeComponent();

        }

        private void cbMale_CheckedChanged(object sender, EventArgs e)
        {
            if (cbMale.Checked) { cbFemale.Checked = false; }
            UpdateSubmitButtonState();

        }

        private void cbFemale_CheckedChanged(object sender, EventArgs e)
        {
            if (cbFemale.Checked) { cbMale.Checked = false; }
            UpdateSubmitButtonState();

        }

        private void cbEnglish_CheckedChanged(object sender, EventArgs e)
        {
            if (cbEnglish.Checked) { cbCroatian.Checked = false; }
            UpdateSubmitButtonState();

        }

        private void cbCroatian_CheckedChanged(object sender, EventArgs e)
        {
            if (cbCroatian.Checked) { cbEnglish.Checked = false; }
            UpdateSubmitButtonState();

        }
        private void UpdateSubmitButtonState()
        {

            bool isTypeSelected = gbType.Controls.OfType<CheckBox>().Any(cb => cb.Checked);


            bool isLanguageSelected = gbLanguage.Controls.OfType<CheckBox>().Any(cb => cb.Checked);


            btnSubmit.Enabled = isTypeSelected && isLanguageSelected;
        }
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show(Resources.Resources.SettingsConfirmationBody, Resources.Resources.SettingsConfirmationTitle, MessageBoxButtons.OKCancel);
            if (confirmResult != DialogResult.OK) return;

            try
            {
                var selectedType = gbType.Controls.OfType<CheckBox>()
                          .FirstOrDefault(cb => cb.Checked)?.Tag?.ToString();

                var selectedLanguage = gbLanguage.Controls.OfType<CheckBox>()
                                          .FirstOrDefault(cb => cb.Checked)?.Tag?.ToString();

                _repository.SaveSettings(selectedType, selectedLanguage);
                CultureSetter.SetFormCulture(selectedLanguage, GetType(), Controls);
            }
            catch (Exception ex) when (ex is ArgumentNullException || ex is IOException || ex is CultureNotFoundException)
            {
                MessageBox.Show(Resources.Resources.Error);
            }





            Hide();
            new InitialTeamSelectForm(_repository, _api).ShowDialog();
            Close();
        }

        private void Settings_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseConfirmation.ConfirmFormClose(e);
        }
    }
}