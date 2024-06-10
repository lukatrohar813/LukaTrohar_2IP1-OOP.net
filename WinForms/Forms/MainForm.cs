using DAL.Api;
using DAL.Models;
using DAL.Models.Matches;
using DAL.Models.Matches.Enums;
using DAL.Repos;
using DAL.Utilities;
using Newtonsoft.Json;
using System.Windows.Forms;
using WinForms.Controls;
using WinForms.HelperClasses;

namespace WinForms.Forms
{
    public partial class MainForm : Form
    {
        #region Fields, Properties and Initialization

        private readonly IFileRepository _repository;
        private readonly IApi _api;
        private bool _formFirstTimeShown = true;
        private readonly IDictionary<string, int> _goals = new Dictionary<string, int>();
        private readonly IDictionary<string, int> _yellowCards = new Dictionary<string, int>();
        private readonly IList<Control> _draggables = new List<Control>();
        private readonly OpenFileDialog _openFileDialog = new OpenFileDialog();
        private readonly StatisticsDisplayForm _statisticsDisplayForm = new StatisticsDisplayForm();
        private IList<Match>? _matches;
        private readonly IList<string> _favoritePlayerNames = new List<string>();
        private const int MaxFavoritePlayers = 3;

        public enum MenuType
        {
            AllPlayers,
            FavoritePlayers
        }

        public MainForm(IFileRepository repository, IApi api)
        {
            _repository = repository;
            _api = api;
            InitializeCulture();
            InitializeComponent();
            LoadAsync();
        }

        private async void LoadAsync()
        {
            await SafeExecute(InitializeForm);
        }

        private async Task InitializeForm()
        {
            InitializeDragAndDrop();
            await LoadTeamDataAsync();
            SetMenuItemStates();
            FormClosing += MainForm_FormClosing;
        }

        private async Task LoadTeamDataAsync()
        {
            var selectedTeam = _repository.GetSelectedTeam();
            if (selectedTeam != null)
            {
                await LoadPanelWithPlayersAsync(selectedTeam);
            }

            await LoadTeamsIntoComboBoxAsync();
        }

        private void InitializeDragAndDrop()
        {
            flpAllPlayers.AllowDrop = true;
            flpFavoritePlayers.AllowDrop = true;
            flpAllPlayers.DragDrop += flpAllPlayers_DragDrop;
            flpAllPlayers.DragEnter += flpAllPlayers_DragEnter;
            flpFavoritePlayers.DragEnter += flpFavoritePlayers_DragEnter;
            flpFavoritePlayers.DragDrop += flpFavoritePlayers_DragDrop;
        }

        private void InitializeCulture()
        {
            var language = _repository.GetLanguage();
            CultureSetter.SetFormCulture(language, GetType(), Controls);
        }

        private void MakeControlsDraggable(IEnumerable<Control> controls) =>
            controls.ToList().ForEach(c => c.DoDragDrop(c.Name, DragDropEffects.Move));

        #endregion

        #region Event Handlers

        private void flpFavoritePlayers_DragDrop(object sender, DragEventArgs e)
        {
            if (flpFavoritePlayers.Controls.Count >= MaxFavoritePlayers) return;

            var controlName = e.Data!.GetData(typeof(string)) as string;
            var userControl = Controls.Find(controlName, true).FirstOrDefault();

            if (userControl != null && ((PlayerUserControl)userControl).IsSelected)
            {
                MoveControlToPanel(userControl, (FlowLayoutPanel)sender);
                ((PlayerUserControl)userControl).FavoriteVisible = true;
            }

            SaveFavoritePlayers();
        }

        private void flpAllPlayers_DragDrop(object sender, DragEventArgs e)
        {
            var controlName = e.Data!.GetData(typeof(string)) as string;
            var userControl = Controls.Find(controlName, true).FirstOrDefault();

            if (userControl != null && ((PlayerUserControl)userControl).FavoriteVisible)
            {
                MoveControlToPanel(userControl, (FlowLayoutPanel)sender);
                ((PlayerUserControl)userControl).FavoriteVisible = false;
            }

            SaveFavoritePlayers();
        }

        private void MoveControlToPanel(Control control, FlowLayoutPanel panel)
        {
            control.Parent.Controls.Remove(control);
            panel.Controls.Add(control);
            ((PlayerUserControl)control).IsSelected = false;
        }

        private void flpAllPlayers_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = CanMoveToPanel(e, false) ? DragDropEffects.Move : DragDropEffects.None;
        }

        private void flpFavoritePlayers_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = CanMoveToPanel(e, true) ? DragDropEffects.Move : DragDropEffects.None;
        }

        private bool CanMoveToPanel(DragEventArgs e, bool isFavoritePanel)
        {
            var controlName = e.Data.GetData(typeof(string)) as string;
            var userControl = Controls.Find(controlName, true).FirstOrDefault();
            return userControl != null && ((PlayerUserControl)userControl).FavoriteVisible == isFavoritePanel;
        }

        private void PlayerUserControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender is PlayerUserControl puc)
            {
                var control = (Control)sender;
                switch (e.Button)
                {
                    case MouseButtons.Left:
                        puc.IsSelected = true;
                        _draggables.Add(control);
                        MakeControlsDraggable(_draggables);
                        break;
                    case MouseButtons.Middle:
                        puc.IsSelected = false;
                        _draggables.Remove(control);
                        break;
                    case MouseButtons.Right:
                        ShowContextMenu(puc, e.Location);
                        break;
                }
            }
        }

        private async void Players_Load(object sender, EventArgs e)
        {
            if (_formFirstTimeShown)
            {
                await LoadPanelWithPlayersAsync(_repository.GetSelectedTeam());
                _formFirstTimeShown = false;
            }
        }

        private void tsmAttendance_Click(object sender, EventArgs e)
        {
            DisplayAttendance(_repository.GetSelectedTeam());
        }

        private void rankByGoalsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DisplayPlayerStatistics(_goals, Resources.Resources.rankingsGoals, Resources.Resources.goals);
        }

        private void rankByYellowCardsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DisplayPlayerStatistics(_yellowCards, Resources.Resources.rankingsCards, Resources.Resources.cards);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseConfirmation.ConfirmFormClose(e);
        }

        private async void cbTeamSelection_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (MessageBox.Show(Resources.Resources.teamSelectionBody, Resources.Resources.teamSelectionTitle,
                    MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                _repository.SaveSelectedTeam(cbTeamSelection.SelectedItem?.ToString());
                await SafeExecute(async () =>
                {
                    await LoadPanelWithPlayersAsync(_repository.GetSelectedTeam());
                    await LoadTeamsIntoComboBoxAsync();
                });
            }
        }

        private void SetMenuItemStates()
        {
            tsmiCroatian.Enabled = _repository.GetLanguage() != Resources.Resources.Croatian;
            tsmiEnglish.Enabled = _repository.GetLanguage() != Resources.Resources.English;
            tsmiMale.Enabled = _repository.GetTeamGender() != Resources.Resources.typeMale;
            tsmiFemale.Enabled = _repository.GetTeamGender() != Resources.Resources.typeFemale;
        }

        private void tsmiEnglish_Click(object sender, EventArgs e) =>
            ChangeLanguage(Resources.Resources.languageChangeBody, Resources.Resources.languageChangeTittle, "en");

        private void tsmiCroatian_Click(object sender, EventArgs e) =>
            ChangeLanguage(Resources.Resources.languageChangeBody, Resources.Resources.languageChangeTittle, "hr");

        private void tsmiFemale_Click(object sender, EventArgs e) => ChangeType(Resources.Resources.typeChangeBody,
            Resources.Resources.typeChangeTitle, "Female", _repository.GetLanguage());

        private void tsmiMale_Click(object sender, EventArgs e) => ChangeType(Resources.Resources.typeChangeBody,
            Resources.Resources.typeChangeTitle, "Male", _repository.GetLanguage());

        #endregion

        #region Context Menu Methods

        private void ShowContextMenu(PlayerUserControl puc, Point cursorPosition)
        {
            var contextMenu =
                CreateContextMenu(puc.Parent.Name == "flpAllPlayers" ? MenuType.AllPlayers : MenuType.FavoritePlayers,
                    puc);
            contextMenu.Show(puc, cursorPosition);
        }

        private ContextMenuStrip CreateContextMenu(MenuType menuType, PlayerUserControl puc)
        {
            var contextMenu = new ContextMenuStrip();
            var loadImageItem = new ToolStripMenuItem { Text = Resources.Resources.LoadImage };
            loadImageItem.Click += (s, e) => LoadPicture(puc);

            var favoritePlayerItem = new ToolStripMenuItem
            {
                Text = menuType == MenuType.AllPlayers
                    ? Resources.Resources.AddFavorite
                    : Resources.Resources.RemoveFavorite
            };
            favoritePlayerItem.Click += (s, e) => ToggleFavoritePlayer(puc, menuType);

            contextMenu.Items.AddRange(new[] { loadImageItem, favoritePlayerItem });
            return contextMenu;
        }

        #endregion

        #region Favorite Player Methods

        private void ToggleFavoritePlayer(PlayerUserControl puc, MenuType menuType)
        {
            if (menuType == MenuType.AllPlayers && flpFavoritePlayers.Controls.Count < MaxFavoritePlayers)
            {
                puc.FavoriteVisible = true;
                flpFavoritePlayers.Controls.Add(puc);
            }
            else
            {
                puc.FavoriteVisible = false;
                flpFavoritePlayers.Controls.Remove(puc);
                flpAllPlayers.Controls.Add(puc);
            }

            SaveFavoritePlayers();
        }

        private void SaveFavoritePlayers()
        {
            var controlNames = flpFavoritePlayers.Controls.Cast<PlayerUserControl>().Select(c => c.Name);
            _repository.SaveFavoritePlayers(controlNames);
        }

        private void LoadFavoritePlayers()
        {
            try
            {
                _favoritePlayerNames.Clear();
                _repository.LoadFavoritePlayers().ToList().ForEach(c =>
                {
                    var userControl = Controls.Find(c.Trim(), true).FirstOrDefault();
                    if (userControl is PlayerUserControl puc)
                    {
                        puc.FavoriteVisible = true;
                        _favoritePlayerNames.Add(puc.Name);
                        flpAllPlayers.Controls.Remove(puc);
                        flpFavoritePlayers.Controls.Add(puc);
                    }
                });
            }
            catch
            {
                MessageBox.Show("Selected Team File does not exist");
            }
        }

        #endregion

        #region Player Panel Methods

        private void AddPlayersToPanel(List<StartingEleven> players)
        {
            players?.ForEach(p =>
            {
                var playerUserControl = new PlayerUserControl
                {
                    PlayerName = p.Name,
                    PlayerNumber = p.ShirtNumber.ToString(),
                    PlayerPosition = p.Position.ToString(),
                    Captain = p.Captain ? Resources.Resources.Captain : null,
                    Name = p.Name,
                    FavoriteVisible = false
                };

                LoadPictureIfPreviouslySelected(playerUserControl);
                flpAllPlayers.Controls.Add(playerUserControl);
                playerUserControl.MouseDown += PlayerUserControl_MouseDown;

                _goals[p.Name] = 0;
                _yellowCards[p.Name] = 0;
            });
        }

        private void LoadPictureIfPreviouslySelected(PlayerUserControl control)
        {
            if (_repository.DoesPictureExist(control.Name))
            {
                control.Image = Image.FromFile(_repository.GetPicturePath(control.Name));
            }
        }

        private void LoadPicture(PlayerUserControl playerUserControl)
        {
            _openFileDialog.Filter = "Image Files (*.bmp;*.jpg;*.jpeg;*.png)|*.bmp;*.jpg;*.jpeg;*.png";
            _openFileDialog.Title = Resources.Resources._openFileDialog_Title;
            if (_openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var filePath = _openFileDialog.FileName;
                _repository.SavePicturePath(playerUserControl.Name.Trim(), filePath.Trim());
                playerUserControl.Image = Image.FromFile(filePath);
            }
        }

        #endregion

        #region Match Methods

        private async Task LoadMatchesAsync()
        {
            await SafeExecute(async () =>
            {
                this.Cursor = Cursors.WaitCursor;
                var teamGender = _repository.GetTeamGender();
                _matches = await _api.GetData<IList<Match>>(EndpointBuilder.GetMatchesEndpoint(teamGender));
            });
        }

        private async Task LoadPanelWithPlayersAsync(dynamic team)
        {
            await SafeExecute(async () =>
            {
                this.Cursor = Cursors.WaitCursor;
                ClearData();
                await LoadMatchesAsync();

                var country = team is Team t ? t.Country : team as string;
                var match = _matches?.FirstOrDefault(m => m.HomeTeamCountry == country);
                var players = match?.HomeTeamStatistics.StartingEleven.Union(match.HomeTeamStatistics.Substitutes)
                    .ToList();

                AddPlayersToPanel(players);
                UpdateGoalsAndYellowCards(match);
                LoadFavoritePlayers();
            });
        }

        private void UpdateGoalsAndYellowCards(Match match)
        {
            _matches?
                .Where(m => m.HomeTeamCountry == match?.HomeTeamCountry)
                .ToList()
                .ForEach(m => m.HomeTeamEvents.ToList().ForEach(te =>
                {
                    switch (te.TypeOfEvent)
                    {
                        case TypeOfEvent.Goal:
                            _goals[te.Player]++;
                            break;
                        case TypeOfEvent.YellowCard:
                        case TypeOfEvent.YellowCardSecond:
                            _yellowCards[te.Player]++;
                            break;
                    }
                }));
        }

        private void ClearData()
        {
            _goals.Clear();
            _yellowCards.Clear();
            flpAllPlayers.Controls.Clear();
            flpFavoritePlayers.Controls.Clear();
        }

        #endregion

        #region Statistics Methods

        private void SetupStatisticsDisplayForm(string title, List<Control> controls)
        {
            _statisticsDisplayForm.flpDisplayForm.Controls.Clear();
            _statisticsDisplayForm.Text = title;
            _statisticsDisplayForm.flpDisplayForm.Controls.AddRange(controls.ToArray());
            _statisticsDisplayForm.ResizeFormToFitControls();
            _statisticsDisplayForm.ShowDialog();
        }

        private List<Control> CreatePlayerUserControls(IDictionary<string, int> playerData, string customText)
        {
            return playerData
                .OrderByDescending(kvp => kvp.Value)
                .Select(kvp =>
                {
                    var playerUserControl = new PlayerUserControl
                    {
                        PlayerName = kvp.Key,
                        PlayerNumber = kvp.Value.ToString(),
                        PositionVisible = false,
                        CaptainVisible = false,
                        CustomText = customText,
                        FavoriteVisible = _favoritePlayerNames.Contains(kvp.Key),
                        Name = kvp.Key
                    };

                    LoadPictureIfPreviouslySelected(playerUserControl);
                    return playerUserControl as Control;
                })
                .ToList();
        }

        private List<Control> CreateMatchUserControls(IEnumerable<Match> matches)
        {
            return matches
                .OrderByDescending(m => m.Attendance)
                .Select(m => new MatchUserControl
                {
                    Location = m.Location,
                    Attendances = m.Attendance.ToString(),
                    HomeTeam = m.HomeTeamCountry,
                    AwayTeam = m.AwayTeamCountry
                })
                .Cast<Control>()
                .ToList();
        }

        private void DisplayAttendance(dynamic team)
        {
            var country = team is Team t ? t.Country : team as string;
            var match = _matches?.FirstOrDefault(m => m.HomeTeamCountry == country);
            var matches = _matches?.Where(m =>
                m.HomeTeamCountry == match?.HomeTeamCountry || m.AwayTeamCountry == match?.HomeTeamCountry).ToList();

            var controls = CreateMatchUserControls(matches);
            SetupStatisticsDisplayForm(Resources.Resources.Attendance, controls);
        }

        private void DisplayPlayerStatistics(IDictionary<string, int> playerData, string title, string customText)
        {
            LoadFavoritePlayers();
            var controls = CreatePlayerUserControls(playerData, customText);
            SetupStatisticsDisplayForm(title, controls);
        }

        #endregion

        #region ComboBox Methods

        private async Task LoadTeamsIntoComboBoxAsync()
        {
            await SafeExecute(async () =>
            {
                cbTeamSelection.Items.Clear();
                this.Cursor = Cursors.WaitCursor;
                var teamGender = _repository.GetTeamGender();
                var endpoint = EndpointBuilder.GetTeamsEndpoint(teamGender);
                var teams = await _api.GetData<IList<Team>>(endpoint);
                teams.ToList().ForEach(t => cbTeamSelection.Items.Add(t));
                cbTeamSelection.Text = _repository.GetSelectedTeam();
            });
        }

        #endregion

        #region Settings Methods

        private void ChangeType(string confirmationMessage, string title, string typeValue, string languageValue)
        {
            if (MessageBox.Show(confirmationMessage, title, MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                FormClosing -= MainForm_FormClosing;
                _repository.SaveSettings(typeValue, languageValue);
                Hide();
                LoadInitialTeamSelectForm();
                Close();
            }
        }

        private void LoadInitialTeamSelectForm()
        {
            var initialForm = new InitialTeamSelectForm(_repository, _api);
            initialForm.ShowDialog();
        }

        private async void ChangeLanguage(string confirmationMessage, string title, string cultureCode)
        {
            if (MessageBox.Show(confirmationMessage, title, MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                _repository.SaveSettings(_repository.GetTeamGender(), cultureCode);
                CultureSetter.SetFormCulture(cultureCode, typeof(MainForm), Controls);
                await ReloadForm();
            }
        }

        private async Task ReloadForm()
        {
            FormClosing -= MainForm_FormClosing;
            Controls.Clear();
            InitializeComponent();
            await InitializeForm();
        }

        #endregion

        #region Helper Methods

        private async Task SafeExecute(Func<Task> action)
        {
            try
            {
                await action();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
    }
}

#endregion