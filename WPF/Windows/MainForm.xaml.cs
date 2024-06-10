using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using DAL.Api;
using DAL.Models;
using DAL.Models.Matches;
using DAL.Models.Matches.Enums;
using DAL.Utilities;
using Newtonsoft.Json;
using WPF.Controls;
using WPF.Helper;
using WinForms.Forms;
using Button = System.Windows.Controls.Button;
using ComboBox = System.Windows.Controls.ComboBox;
using Cursors = System.Windows.Input.Cursors;
using Label = System.Windows.Controls.Label;
using MessageBox = System.Windows.MessageBox;
using Panel = System.Windows.Controls.Panel;

namespace WPF.Windows
{
    public partial class MainForm 
    {
        private readonly IApi _api;
        private readonly IFileRepository _repository;
        private readonly CultureSetter _cultureSetter;

        private Team? HomeTeam { get; set; }
        private MatchTeam? AwayTeam { get; set; }

        public MainForm()
        {
            _api = ApiFactory.GetApi();
            _repository = RepositoryFactory.GetRepository();
            _cultureSetter = new CultureSetter();

            _cultureSetter.SetCulture(_repository);
            InitializeComponent();
            SetWindowSize();

        }

        private async void MainForm_OnLoaded(object sender, RoutedEventArgs e)
        {

            await SafeExecuteAsync(() =>LoadComboBoxWithTeamsAsync(CbHomeTeam));
            await SafeExecuteAsync(LoadPanelTeamSelectedAsync);
        }


        private async void CbHomeTeam_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await SafeExecuteAsync(async () =>
            {
                HideTeamInformationBtns();
                var selectedTeam = (sender as ComboBox)?.SelectedItem as Team;
                if (selectedTeam == null) return;

                HomeTeam = selectedTeam;
                LblResultHomeTeam.Content = selectedTeam.Country;
                ClearHomeTeamPanels();
                ClearAwayTeamPanels();
                ClearMatchResults();
                _repository.SaveSelectedTeam(CbHomeTeam.SelectedItem?.ToString());
                await LoadComboBoxWithOpponentsAsync(CbAwayTeam, HomeTeam);
            });
        }


        private async void CbAwayTeam_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await SafeExecuteAsync(async () =>
            {
                var selectedMatchTeam = (sender as ComboBox)?.SelectedItem as MatchTeam;
                if (selectedMatchTeam == null) return;

                AwayTeam = selectedMatchTeam;

                var matches = await _api.GetData<IList<Match>>(EndpointBuilder.GetMatchesEndpoint(_repository.GetTeamGender()));
                var match = matches?.FirstOrDefault(m => m.HomeTeamCountry == HomeTeam.Country && m.AwayTeamCountry == AwayTeam.Country);

                if (match == null)
                {
                    MessageBox.Show("Could not retrieve match data", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                MatchResultLabels(match.AwayTeam.Country, $"{match.HomeTeam.Goals} : {match.AwayTeam.Goals}");
                await LoadPanelWithPlayersAsync(match);
                ShowTeamInformationBtns();
            });
        }

        private void ClearMatchResults()
        {
            LblResultHomeTeam.Content = "";
            LblResultAwayTeam.Content = "";
            LblResult_Result.Content = "";
        }
        private void MatchResultLabels(string awayTeam, string result)
        {
            LblResultHomeTeam.Content = _repository.GetSelectedTeam();
            LblResultAwayTeam.Content = result;
            LblResult_Result.Content = awayTeam;
        }
    
        private void ClearHomeTeamPanels()
        {
            PnlHomeTeamGoalie.Children.Clear();
            PnlHomeTeamDefender.Children.Clear();
            PnlHomeTeamMidfield.Children.Clear();
            PnlHomeTeamForward.Children.Clear();
        }

        private void ClearAwayTeamPanels()
        {
            PnlAwayTeamGoalie.Children.Clear();
            PnlAwayTeamDefender.Children.Clear();
            PnlAwayTeamMidfield.Children.Clear();
            PnlAwayTeamForward.Children.Clear();
        }

        private async Task LoadComboBoxWithTeamsAsync(ItemsControl control)
        {
            control.Items.Clear();
            var teamGender = _repository.GetTeamGender();
            var endpoint = EndpointBuilder.GetTeamsEndpoint(teamGender);
            var teams = await _api.GetData<IList<Team>>(endpoint);
            foreach (var team in teams)
            {
                control.Items.Add(team);
            }
            CbHomeTeam.SelectedItem = teams.FirstOrDefault(t => t.Country == _repository.GetSelectedTeam());
        }


        private Task LoadPanelWithPlayersAsync(Match match)
        {
            ClearHomeTeamPanels();
            ClearAwayTeamPanels();

            var playersHome = match.HomeTeamStatistics?.StartingEleven;
            foreach (var player in playersHome)
            {
                var playerUserControl = new PlayerUserControl(player.Name, (int)player.ShirtNumber)
                {
                    MaxHeight = 150,
                    MaxWidth = 120,
                    BtnUserControl = { ClickMode = ClickMode.Press }
                };
                playerUserControl.BtnUserControl.Click += OnUserControlClickAsync;

                Panel panel = player.Position switch
                {
                    Position.Defender => PnlHomeTeamDefender,
                    Position.Forward => PnlHomeTeamForward,
                    Position.Goalie => PnlHomeTeamGoalie,
                    Position.Midfield => PnlHomeTeamMidfield,
                    _ => null
                };

                panel?.Children.Add(playerUserControl);
            }

            var playersAway = match.AwayTeamStatistics?.StartingEleven;
            foreach (var player in playersAway)
            {
                var playerUserControl = new PlayerUserControl(player.Name, (int)player.ShirtNumber)
                {
                    MaxHeight = 150,
                    MaxWidth = 120,
                    BtnUserControl = { ClickMode = ClickMode.Press }
                };
                playerUserControl.BtnUserControl.Click += OnUserControlClickAsync;

                Panel panel = player.Position switch
                {
                    Position.Defender => PnlAwayTeamDefender,
                    Position.Forward => PnlAwayTeamForward,
                    Position.Goalie => PnlAwayTeamGoalie,
                    Position.Midfield => PnlAwayTeamMidfield,
                    _ => null
                };

                panel?.Children.Add(playerUserControl);
            }

            return Task.CompletedTask;
        }



        private async Task LoadComboBoxWithOpponentsAsync(ItemsControl control, Team homeTeam)
        {
            control.Items.Clear();

            var teamGender = _repository.GetTeamGender();
            var endpoint = EndpointBuilder.GetMatchesEndpoint(teamGender);
            var matches = await _api.GetData<IList<Match>>(endpoint);

            var matchesPlayed = matches.Where(m => m.HomeTeam.Country == homeTeam.Country);
            foreach (var match in matchesPlayed)
            {
                control.Items.Add(match.AwayTeam);
            }
        }

        private async void GetTeamsResultsAsync(dynamic team)
        {
            await SafeExecuteAsync(async () =>
            {
                if (team is null) return;

                var teamGender = _repository.GetTeamGender();
                var endpoint = EndpointBuilder.GetTeamResultsEndpoint(teamGender);
                var allTeamResults = await _api.GetData<IList<TeamStats>>(endpoint);
                var teamResult = allTeamResults.FirstOrDefault(tr => tr.Country == team.Country);

                new TeamStatistics(
                    teamResult?.Country,
                    teamResult?.FifaCode,
                    teamResult?.GamesPlayed.ToString(),
                    teamResult?.Wins.ToString(),
                    teamResult?.Losses.ToString(),
                    teamResult?.Draws.ToString(),
                    teamResult?.GoalsFor.ToString(),
                    teamResult?.GoalsAgainst.ToString()).ShowDialog();
            });
        }

        private async void OnUserControlClickAsync(object sender, RoutedEventArgs e)
        {
            await SafeExecuteAsync(async () =>
            {
                if (!(CbHomeTeam.SelectionBoxItem is Team homeTeam) ||
                    !(CbAwayTeam.SelectionBoxItem is MatchTeam awayTeam))
                {
                    MessageBox.Show("Please select both home and away teams before viewing player information.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var teamGender = _repository.GetTeamGender();
                var endpoint = EndpointBuilder.GetMatchesEndpoint(teamGender);
                var matches = await _api.GetData<IList<Match>>(endpoint);

                var selectedMatch = matches.FirstOrDefault(m =>
                    m.HomeTeamCountry == homeTeam.Country &&
                    m.AwayTeamCountry == awayTeam.Country);

                if (selectedMatch == null)
                {
                    MessageBox.Show("Match data not found for the selected teams.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var userControlButton = sender as Button;
                var userControlPanel = userControlButton?.Content as StackPanel;
                var playerName = (userControlPanel?.Children[0] as Label)?.Content.ToString();

                var playerInformation = selectedMatch.HomeTeamStatistics.StartingEleven
                    .Union(selectedMatch.AwayTeamStatistics.StartingEleven)
                    .FirstOrDefault(p => p.Name == playerName);

                if (playerInformation == null)
                {
                    MessageBox.Show("Player information not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                int goalsScored = 0, yellowCards = 0;
                var events = selectedMatch.HomeTeamEvents
                    .Union(selectedMatch.AwayTeamEvents)
                    .Where(ev => ev.Player == playerName);

                foreach (var ev in events)
                {
                    switch (ev.TypeOfEvent)
                    {
                        case TypeOfEvent.Goal:
                        case TypeOfEvent.GoalOwn:
                            goalsScored++;
                            break;
                        case TypeOfEvent.YellowCard:
                        case TypeOfEvent.YellowCardSecond:
                            yellowCards++;
                            break;
                    }
                }

                new PlayerInformation(
                    playerName,
                    playerInformation.ShirtNumber.ToString(),
                    playerInformation.Position.ToString(),
                    playerInformation.Captain,
                    goalsScored.ToString(),
                    yellowCards.ToString()).ShowDialog();
            });
        }


        private async Task LoadPanelTeamSelectedAsync()
        {
            if (_repository.DoesSelectedTeamExist())
            {
                var teams = await _api.GetData<IList<Team>>(EndpointBuilder.GetTeamsEndpoint(_repository.GetTeamGender()));
                HomeTeam = teams.FirstOrDefault(t => t.Country == _repository.GetSelectedTeam());

                if (HomeTeam != null)
                {
                    CbHomeTeam.SelectedItem = HomeTeam;
                }
            }
        }


        private void SetWindowSize()
        {
            var size = _repository.GetApplicationSize().ToLowerInvariant();
            (Width, Height) = size switch
            {
                "small" => (800, 600),
                "medium" => (1024, 768),
                "large" => (1280, 1024),
                "fullscreen" => (SystemParameters.PrimaryScreenWidth, SystemParameters.PrimaryScreenHeight),
                _ => (800, 600)
            };

            if (size == "fullscreen")
            {
                WindowState = WindowState.Maximized;
            }
        }





        private void BtnHomeTeamInformation_OnClick(object sender, RoutedEventArgs e) => GetTeamsResultsAsync(CbHomeTeam.SelectionBoxItem as Team);

        private void BtnAwayTeamInformation_OnClick(object sender, RoutedEventArgs e) => GetTeamsResultsAsync(CbAwayTeam.SelectionBoxItem as MatchTeam);

        private void WinForms_OnClick(object sender, RoutedEventArgs e)
        {
            var confirmResult = MessageBox.Show(
               Properties.Resources.WinFormsOpeningBody,
               Properties.Resources.WinFormsOpeningTitle,
               MessageBoxButton.OKCancel, MessageBoxImage.Question);

            if (confirmResult == MessageBoxResult.OK)
            {
                this.Closing -= MainForm_OnClosing;
                Hide();
                new WinForms.Forms.MainForm(_repository,_api).ShowDialog();
                Close();
            }
            return;

        }

        private void Settings_OnClick(object sender, RoutedEventArgs e)
        {
            var confirmResult = MessageBox.Show(
               Properties.Resources.SettingsOpeningBody,
               Properties.Resources.SettingsOpeningTitle,
               MessageBoxButton.OKCancel, MessageBoxImage.Question);

            if (confirmResult == MessageBoxResult.OK)
            {
                this.Closing -= MainForm_OnClosing;
                Hide();
                new Settings().ShowDialog();
                Close();
            }
            return;
        }

        private void MainForm_OnClosing(object sender, CancelEventArgs e)
        {
            var confirmResult = MessageBox.Show(
                Properties.Resources.mainFormClosingBody,
                Properties.Resources.mainFormClosingTitle,
                MessageBoxButton.OKCancel, MessageBoxImage.Question);

            if (confirmResult != MessageBoxResult.OK) e.Cancel = true;
        }

        void ShowTeamInformationBtns() 
        {
            BtnHomeTeamInformation.Visibility = Visibility.Visible;
            BtnAwayTeamInformation.Visibility = Visibility.Visible;
            BtnHomeTeamInformation.Content = $"{CbHomeTeam.SelectedItem.ToString().Split('(').ElementAtOrDefault(0)}{Properties.Resources.btnHomeTeamInformation}";
            BtnAwayTeamInformation.Content = $"{CbAwayTeam.SelectedItem.ToString().Split('(').ElementAtOrDefault(0)}{Properties.Resources.btnAwayTeamInformation}";
        }

        void HideTeamInformationBtns()
        {
            BtnHomeTeamInformation.Visibility = Visibility.Hidden;
            BtnAwayTeamInformation.Visibility = Visibility.Hidden;
        }
        private async Task SafeExecuteAsync(Func<Task> asyncFunction)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;

                await asyncFunction();
            }
            catch (Exception ex) when (ex is IOException || ex is JsonReaderException || ex is ArgumentNullException)
            {
                MessageBox.Show(Properties.Resources.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }

    }

}