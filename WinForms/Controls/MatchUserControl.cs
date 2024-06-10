using System.ComponentModel;

namespace WinForms.Controls
{
    public partial class MatchUserControl : UserControl
    {
#pragma warning disable CS0649 
        private readonly string? _location;
        [Category("MatchUserControl")]
        public new string? Location
        {
            get => _location;
            set => lblLocationText.Text = value;
        }


        private readonly string? _attendances;
        [Category("MatchUserControl")]
        public string? Attendances
        {
            get => _attendances;
            set => lblAttendanceText.Text = value;
        }

        private readonly string? _homeTeam;
        [Category("MatchUserControl")]
        public string? HomeTeam
        {
            get => _homeTeam;
            set => lblHomeTeamText.Text = value;
        }

        private readonly string? _awayTeam;
        [Category("MatchUserControl")]
        public string? AwayTeam
        {
            get => _awayTeam;
            set => lblAwayTeamText.Text = value;
        }

        public MatchUserControl()
        {
            InitializeComponent();
        }
#pragma warning disable CS0649
    }
}
 