namespace WPF.Controls
{
    /// <summary>
    /// Interaction logic for PlayerUserControl.xaml
    /// </summary>
    public partial class PlayerUserControl
    {
        private readonly IFileRepository _repository = RepositoryFactory.GetRepository();
        private const string DefaultImagePath = @"/Resources/placeholder_img.png";

        public string PlayerName { get; set; }
        public int PlayerNumber { get; set; }
        public string PlayerImagePath =>_repository.DoesPictureExist(PlayerName) ? _repository.GetPicturePath(PlayerName) : DefaultImagePath;
        public PlayerUserControl(string playerName, int shirtNumber)
        {
            PlayerName = playerName;
            PlayerNumber = shirtNumber;
            

            InitializeComponent();
        }
    }
}
