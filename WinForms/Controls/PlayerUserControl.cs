using System.ComponentModel;
#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value

namespace WinForms.Controls
{
    public partial class PlayerUserControl : UserControl
    {


        private readonly string? _playerName;
        [Category("PlayerUserControl")]
        public string? PlayerName
        {
            get => _playerName;
            set => lblPlayerName.Text = value;
        }

        private readonly string? _playerNumber;
        [Category("PlayerUserControl")]
        public string? PlayerNumber
        {
            get => _playerNumber;
            set => lblPlayerNumber.Text = value;
        }

        private readonly string? _playerPosition;
        [Category("PlayerUserControl")]
        public string? PlayerPosition
        {
            get => _playerPosition;
            set => lblPlayerPosition.Text = value;
        }

        private readonly string? _captain;
        [Category("PlayerUserControl")]
        public string? Captain
        {
            get => _captain;
            set => lblCaptain.Text = value;
        }

        private readonly Image? _image;
        [Category("PlayerUserControl")]
        public Image? Image
        {
            get => _image;
            set
            {
                pbPlayer.Image = value;
                pbPlayer.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }
        private bool _isSelected;
        [Category("PlayerUserControl")]
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                BackColor = IsSelected ? Color.LightBlue : Color.Gainsboro;
            }
        }

        private bool _positionVisible;
        [Category("PlayerUserControl")]
        public bool PositionVisible
        {
            get => _positionVisible;
            set
            {
                _positionVisible = value;
                lblPlayerPosition.Visible = PositionVisible;
            }
        }

        private bool _captainVisible;
        [Category("PlayerUserControl")]
        public bool CaptainVisible
        {
            get => _captainVisible;
            set
            {
                _captainVisible = value;
                lblCaptain.Visible = CaptainVisible;
            }
        }

        private bool _favoriteVisible;

        [Category("PlayerUserControl")]
        public bool FavoriteVisible
        {
            get => _favoriteVisible;
            set
            {
                _favoriteVisible = value;

                if (_favoriteVisible)
                {
                    lblFavorite.Visible = true;
                    return;
                }

                lblFavorite.Visible = false;
            }
        }

        private string? _customText;
        [Category("PlayerUserControl")]
        public string? CustomText
        {
            get => _customText;
            set
            {
                _customText = value;
                lblPlayerNumberText.Text = CustomText;
            }
        }

        public PlayerUserControl()
        {
            InitializeComponent();
            // SetPlayerImage();
        }




    }
}
