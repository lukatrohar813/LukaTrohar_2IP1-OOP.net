namespace DAL.Repos
{
    internal class FileRepository : IFileRepository
    {
        private const string DefaultGender = "female";
        private const string DefaultLanguage = "en";
        private const string DefaultSize = "medium";
        private const string RelativePath = "Shared_Settings";
        private static readonly string SolutionDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
        private static readonly string Folder = Path.Combine(SolutionDirectory, RelativePath);
        private const string SettingsFileName = "settings.txt";
        private const string PicturesFileName = "pictures.txt";
        private const string AppSizeFileName = "app_size.txt";
        private const string FavoritePlayersFileName = "favorite_players.txt";
        private const string selectedTeamFileName = "selected_team.txt";
        private const char Separator = '|';
        private static readonly string SettingsFilePath = Path.Combine(Folder, SettingsFileName);
        private static readonly string PicturesFilePath = Path.Combine(Folder, PicturesFileName);
        private static readonly string AppSizeFilePath = Path.Combine(Folder, AppSizeFileName);
        private static readonly string FavoritePlayersFilePath = Path.Combine(Folder, FavoritePlayersFileName);
        private static readonly string SelectedTeamFilePath = Path.Combine(Folder, selectedTeamFileName);
        private static readonly FileRepository _instance = new FileRepository();
        public static FileRepository Instance => _instance;

        public FileRepository()
        {
            EnsureFolderExists();
        }

        private void EnsureFolderExists()
        {
            if (!Directory.Exists(Folder))
            {
                Directory.CreateDirectory(Folder);
            }
        }

        public void SaveSettings(string tournamentType, string language)
        {
            if (string.IsNullOrEmpty(tournamentType)) tournamentType = DefaultGender;
            if (string.IsNullOrEmpty(language)) language = DefaultLanguage; 
            File.WriteAllText(SettingsFilePath, $"{tournamentType}{Separator}{language}");
        }

        public void SaveApplicationSize(string appSize)
        {
            if(string.IsNullOrEmpty(appSize)) appSize = DefaultSize;
            File.WriteAllText(AppSizeFilePath, appSize);
        }

        public void SavePicturePath(string playerName, string picturePath)
        {
            File.AppendAllText(PicturesFilePath, $"{playerName}{Separator}{picturePath}{Environment.NewLine}");
        }

        public string LoadSettings()
        {
            return File.Exists(SettingsFilePath) ? File.ReadAllText(SettingsFilePath) : string.Empty;
        }

        public string LoadSelectedTeam()
        {
            return File.Exists(SelectedTeamFilePath) ? File.ReadAllText(SelectedTeamFilePath) : string.Empty;
        }

        public string GetPicturePath(string playerName)
        {
            if (!File.Exists(PicturesFilePath)) return string.Empty;

            return File.ReadLines(PicturesFilePath)
                       .Select(line => line.Split(Separator))
                       .FirstOrDefault(parts => parts.Length > 1 && parts[0] == playerName)?
                       .ElementAtOrDefault(1) ?? string.Empty;
        }

        public string GetTeamGender()
        {
            var settings = LoadSettings().Split(Separator);
            var gender = settings.ElementAtOrDefault(0)?.Trim() ?? DefaultGender;
            return gender;
        }

        public string GetLanguage()
        {
            var settings = LoadSettings().Split(Separator);
            return settings.ElementAtOrDefault(1)?.Trim() ?? DefaultLanguage;
        }

        public string GetSelectedTeam()
        {
            var selectedTeam = LoadSelectedTeam();
            return selectedTeam.Split('(').FirstOrDefault()?.Trim() ?? string.Empty;
        }

        public string GetApplicationSize()
        {
            return File.Exists(AppSizeFilePath) ? File.ReadAllText(AppSizeFilePath) : string.Empty;
        }

  

        public void SaveFavoritePlayers(IEnumerable<string> playerNames)
        {
            File.WriteAllLines(FavoritePlayersFilePath, playerNames);
        }

        public void SaveSelectedTeam(string teamName)
        {
            File.WriteAllText(SelectedTeamFilePath, teamName);
        }

        public IEnumerable<string> LoadFavoritePlayers()
        {
            return File.Exists(FavoritePlayersFilePath) ? File.ReadLines(FavoritePlayersFilePath) : Enumerable.Empty<string>();
        }

        public bool DoesPictureExist(string playerName)
        {
            if (!File.Exists(PicturesFilePath)) return false;

            return File.ReadLines(PicturesFilePath)
                       .Any(line => line.Split(Separator).ElementAtOrDefault(0) == playerName);
        }

        public bool DoSettingsExist()
        {
            return File.Exists(SettingsFilePath);
        }

        public bool DoesSelectedTeamExist()
        {
            return File.Exists(SelectedTeamFilePath);
        }
    }
}
