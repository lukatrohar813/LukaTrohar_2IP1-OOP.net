using DAL.Api;
using DAL.Models;
using DAL.Repos;
using DAL.Utilities;
using Newtonsoft.Json;
using WinForms.HelperClasses;

namespace WinForms.Forms
{
    public partial class InitialTeamSelectForm : Form

    {
        private readonly IFileRepository _repository;
        private readonly IApi _api;

        public InitialTeamSelectForm(IFileRepository repository,IApi api)
        {
            _repository = repository;
            _api = api;
            InitializeComponent();
            LoadTeamsAsync();
            lblStatus.Text = Resources.Resources.TeamsSuccessfullyLoaded;
            btnContinue.Enabled = false;
            cbFavoriteTeam.SelectedIndexChanged += (sender, e) =>
            {
                btnContinue.Enabled = cbFavoriteTeam.SelectedIndex != -1;
            };
        }

        private async void LoadTeamsAsync()
        {
            await ComboBoxTeamLoadAsync();

        }

        private async Task ComboBoxTeamLoadAsync()
        {

            try
            {
                this.Cursor = Cursors.WaitCursor;
                var teamGender = _repository.GetTeamGender();
                var endpoint = EndpointBuilder.GetTeamsEndpoint(teamGender);
                lblStatus.Text = Resources.Resources.Fetching;
                var teams = await _api.GetData<IList<Team>>(endpoint);
                teams.ToList().ForEach(t => cbFavoriteTeam.Items.Add(t));
                cbFavoriteTeam.Text = string.Empty;
            }
            catch (Exception ex) when (ex is IOException || ex is JsonReaderException || ex is ArgumentNullException)
            {
                MessageBox.Show(Resources.Resources.AnErrorOccurredWhileFetchingTeamsExMessage, Resources.Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = Resources.Resources.Aborted;
            }finally
            {
                this.Cursor = Cursors.Default;
            }

        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            var selectedOption = cbFavoriteTeam.SelectedItem?.ToString();
            _repository.SaveSelectedTeam(selectedOption);

            Hide();
            new MainForm(_repository,_api).ShowDialog();
            Close();

        }

        private void InitialTeamSelectForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseConfirmation.ConfirmFormClose(e);

        }
    }
}