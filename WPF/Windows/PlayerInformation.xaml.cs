namespace WPF.Windows
{
    /// <summary>
    /// Interaction logic for PlayerInformation.xaml
    /// </summary>
    public partial class PlayerInformation
    {
        private readonly IFileRepository _repository = RepositoryFactory.GetRepository();
        private const string DefaultImagePath = @"../../Resources/placeholder_img.png";

        public string PlayerImagePath => _repository.DoesPictureExist(PlayerName)
            ? _repository.GetPicturePath(PlayerName)
            : DefaultImagePath;

        public string PlayerName { get; set; }
        public string ShirtNumber { get; set; }
        public string Position { get; set; }
        public bool Captain { get; set; }
        public string GoalsScored { get; set; }
        public string YellowCardsReceived { get; set; }

        public PlayerInformation(string playerName, string shirtNumber, string position,
            bool captain, string goalsScored, string yellowCardsReceived)
        {
            PlayerName = playerName;
            ShirtNumber = shirtNumber;
            Position = position;
            Captain = captain;
            GoalsScored = goalsScored;
            YellowCardsReceived = yellowCardsReceived;
            InitializeComponent();
            TxtCaptain.Text = Captain ? Properties.Resources._Captian : Properties.Resources._NotCaptian;
         
        }
    }
}

