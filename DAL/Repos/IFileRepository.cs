namespace DAL.Repos
{
    public interface IFileRepository
    {
        void SaveSettings(string tournamentType, string language);
        void SaveApplicationSize(string appSize);
        void SavePicturePath(string playerName, string picturePath);
        string LoadSettings();
        string LoadSelectedTeam();
        string GetPicturePath(string playerName);
        string GetTeamGender();
        string GetLanguage();
        string GetSelectedTeam();
        string GetApplicationSize();
        void SaveFavoritePlayers(IEnumerable<string> playerNames);
        void SaveSelectedTeam(string teamName);
        IEnumerable<string> LoadFavoritePlayers();
        bool DoesPictureExist(string playerName);
        bool DoSettingsExist();
        bool DoesSelectedTeamExist();
        
    }
}