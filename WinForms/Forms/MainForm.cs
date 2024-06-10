using DAL.Api;
using DAL.Models;
using DAL.Models.Matches;
using DAL.Models.Matches.Enums;
using DAL.Repos;
using DAL.Utilities;
using Newtonsoft.Json;
using System.Windows.Forms;
using Newtonsoft.Json.Serialization;
using WinForms.Controls;
using WinForms.HelperClasses;

namespace WinForms.Forms
{
    public partial class MainForm : Form
    {
        #region Fields, Properties and Initialization

        private readonly IFileRepository _repository;
        private readonly IApi _api;
        private readonly IDictionary<string, int> _goals = new Dictionary<string, int>();
        private readonly IDictionary<string, int> _yellowCards = new Dictionary<string, int>();
        private readonly IList<Control> _draggables = new List<Control>();
        private readonly OpenFileDialog _openFileDialog = new OpenFileDialog();
        private readonly StatisticsDisplayForm _statisticsDisplayForm = new StatisticsDisplayForm();
        private IList<Match>? _matches;
        private readonly IList<string> _favoritePlayerNames = new List<string>();
        private const int MaxFavoritePlayers = 3;
        private FlowLayoutPanel departedFrom;
        private FlowLayoutPanel goingTo;
        private bool dndSuccessful;

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
            InitializeDragAndDrop();
            InitializeAsync();
            SetMenuItemStates();
        }

        private async void InitializeAsync()
        {
            await SafeExecuteAsync(InitializeMainForm);
        }

        private async Task InitializeMainForm()
        {
            await FetchAndDisplayTeamDataAsync();
            FormClosing += MainForm_FormClosing;
        }

        private void InitializeCulture()
        {
            var language = _repository.GetLanguage();
            CultureSetter.SetFormCulture(language, GetType(), Controls);
        }

        private void InitializeDragAndDrop()
        {
            flpAllPlayers.AllowDrop = true;
            flpFavoritePlayers.AllowDrop = true;


        }

        #endregion

        #region MainForm Event Handlers

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
                await SafeExecuteAsync(async () =>
                {
                    await LoadPanelWithPlayersAsync(_repository.GetSelectedTeam());
                    await LoadTeamsIntoComboBoxAsync();
                });
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

        #endregion

        #region Control Movement

        private void PlayerUserControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender is PlayerUserControl playerControl)
            {
                // Ensure selection logic is handled first
                HandleSelection(playerControl, e);

                // Only initiate drag-and-drop if it's a left mouse button click
                if (e.Button == MouseButtons.Left)
                {
                    StartDragDrop(playerControl);
                }
            }
        }

        private void HandleSelection(PlayerUserControl playerControl, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    // Toggle selection state with a slight delay to avoid conflict with drag-and-drop
                    Task.Delay(100).ContinueWith(_ => playerControl.Invoke(new Action(() =>
                    {
                        playerControl.IsSelected = !playerControl.IsSelected;
                    })));
                    break;
                case MouseButtons.Middle:
                    playerControl.IsSelected = false;
                    break;
                case MouseButtons.Right:
                    ShowContextMenu(playerControl, e.Location);
                    break;
            }
        }

        private void StartDragDrop(PlayerUserControl playerControl)
        {
            departedFrom = playerControl.Parent as FlowLayoutPanel;
            playerControl.DoDragDrop(playerControl, DragDropEffects.Move);
        }

        private void FlowLayoutPanel_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(PlayerUserControl)))
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        private void FlowLayoutPanel_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void FlowLayoutPanel_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(PlayerUserControl)))
            {
                var control = (PlayerUserControl)e.Data.GetData(typeof(PlayerUserControl));
                var targetPanel = sender as FlowLayoutPanel;

                if (departedFrom != targetPanel)
                {
                    if (targetPanel == flpFavoritePlayers && flpFavoritePlayers.Controls.Count >= MaxFavoritePlayers)
                    {
                        return;
                    }

                    dndSuccessful = true;
                    control.Parent.Controls.Remove(control);
                    targetPanel.Controls.Add(control);
                    control.FavoriteVisible = (targetPanel == flpFavoritePlayers);
                    SaveFavoritePlayers();
                }
            }
        }


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
                MessageBox.Show(Resources.Resources.Error);
            }
        }

        #endregion

        #region Player Panel Methods

        private void DisplayPlayersPanel(List<StartingEleven> players)
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

                LoadPreviouslySelectedPicture(playerUserControl);
                flpAllPlayers.Controls.Add(playerUserControl);

                // Subscribe to the MouseDown event here
                playerUserControl.MouseDown += PlayerUserControl_MouseDown;
                playerUserControl.AllowDrop = true;
            });

            foreach (PlayerUserControl control in flpFavoritePlayers.Controls)
            {
                // Subscribe to the MouseDown event for favorite players as well
                control.MouseDown += PlayerUserControl_MouseDown;
            }
        }

        private void LoadPreviouslySelectedPicture(PlayerUserControl control)
        {
            if (_repository.DoesPictureExist(control.Name))
            {
                control.Image = Image.FromFile(_repository.GetPicturePath(control.Name));
            }
        }

        private void LoadPicture(PlayerUserControl playerUserControl)
        {
            _openFileDialog.Filter = Resources.Resources.Imagesfilter;
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
            await SafeExecuteAsync(async () =>
            {
                this.Cursor = Cursors.WaitCursor;
                var teamGender = _repository.GetTeamGender();
                _matches = await _api.GetData<IList<Match>>(EndpointBuilder.GetMatchesEndpoint(teamGender));
             
            });
        }

        private async Task LoadPanelWithPlayersAsync(dynamic team)
        {
            await SafeExecuteAsync(async () =>
            {
                this.Cursor = Cursors.WaitCursor;
                ClearAllData();
                await LoadMatchesAsync();

                var country = team is Team t ? t.Country : team as string;
                var match = _matches?.FirstOrDefault(m => m.HomeTeamCountry == country);
                var players = match?.HomeTeamStatistics.StartingEleven.Union(match.HomeTeamStatistics.Substitutes)
                    .ToList();

                DisplayPlayersPanel(players);
                UpdatePlayerStats(match);
                LoadFavoritePlayers();
            });
        }

        private void UpdatePlayerStats(Match match)
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

        private void ClearAllData()
        {
            _goals.Clear();
            _yellowCards.Clear();
            flpAllPlayers.Controls.Clear();
            flpFavoritePlayers.Controls.Clear();
        }

        #endregion

        #region Statistics Methods

        private void ConfigureStatisticsDisplayForm(string title, List<Control> controls)
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

                    LoadPreviouslySelectedPicture(playerUserControl);
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
            ConfigureStatisticsDisplayForm(Resources.Resources.Attendance, controls);
        }

        private void DisplayPlayerStatistics(IDictionary<string, int> playerData, string title, string customText)
        {
            LoadFavoritePlayers();
            var controls = CreatePlayerUserControls(playerData, customText);
            ConfigureStatisticsDisplayForm(title, controls);
        }

        #endregion

        #region ComboBox Methods

        private async Task LoadTeamsIntoComboBoxAsync()
        {
            await SafeExecuteAsync(async () =>
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

        private async Task FetchAndDisplayTeamDataAsync()
        {
            var selectedTeam = _repository.GetSelectedTeam();
            if (selectedTeam != null)
            {
                await LoadPanelWithPlayersAsync(selectedTeam);
            }

            await LoadTeamsIntoComboBoxAsync();
        }

        private async void ChangeLanguage(string confirmationMessage, string title, string cultureCode)
        {
            if (MessageBox.Show(confirmationMessage, title, MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                _repository.SaveSettings(_repository.GetTeamGender(), cultureCode);
                CultureSetter.SetFormCulture(cultureCode, typeof(MainForm), Controls);
                await ReloadMainForm();
            }
        }

        private async Task ReloadMainForm()
        {
            FormClosing -= MainForm_FormClosing;
            Controls.Clear();
            InitializeComponent();
            await InitializeMainForm();
            SetMenuItemStates();
        }

        #endregion

        #region Safe Execution

        private async Task SafeExecuteAsync(Func<Task> action)
        {
            try
            {
                await action();
            }
            catch (Exception ex)
            {
              
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        #endregion


    }
}
