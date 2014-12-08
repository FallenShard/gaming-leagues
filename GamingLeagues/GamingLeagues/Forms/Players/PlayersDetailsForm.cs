using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using NHibernate;
using GamingLeagues.Entities;
using GamingLeagues.DataAccessLayer;

namespace GamingLeagues.Forms.Players
{
    public partial class PlayersDetailsForm : Form
    {
        // Id of the player displayed
        private int m_playerId = -1;

        public PlayersDetailsForm(int playerId)
        {
            InitializeComponent();

            m_playerId = playerId;

            lbGames.Items.Clear();
            lbLeagues.Items.Clear();


        }

        private void PlayersDetailsForm_Load(object sender, EventArgs e)
        {
            if (m_playerId == -1)
            {
                MessageBox.Show("Error receiving data on player");
                this.Close();
            }

            // Load player data into UI controls
            InitializePlayerData();
        }

        private void InitializePlayerData()
        {
            ISession session = DataAccessLayer.DataAccessLayer.GetSession();
            Player player = session.Get<Player>(m_playerId);

            this.Text = player.Name + " \"" + player.NickName + "\" " + player.LastName;

            lblNickname.Text       = player.NickName;
            lblFullName.Text       = player.Name + " " + player.LastName;
            lblGender.Text         = player.Gender == 'M' ? "Male" : "Female";
            lblBirthDate.Text      = player.DateOfBirth.ToString("dd/mm/yyyy");
            lblCountry.Text        = player.Country;
            lblTurnedPro.Text      = player.DateTurnedPro.ToString("dd/mm/yyyy");;
            lblCareerEarnings.Text = player.CareerEarnings.ToString();
            lblTeam.Text           = player.CurrentTeam != null ? player.CurrentTeam.Name : "--None--";
            
            // Games
            IList<Game> games = player.Games;
            if (games.Count > 0)
            {
                lbGames.DataSource = games;
                lbGames.DisplayMember = "Title";
            }

            // Leagues
            IList<PlaysInLeague> playsInLeague = player.Rankings;
            if (playsInLeague.Count > 0)
            {
                foreach (PlaysInLeague play in playsInLeague)
                    lbLeagues.Items.Add(play.League.Name);
            }

            IList<Match> matches = player.MatchesPlayed;
            lvMatchHistory.Clear();
            lvMatchHistory.Columns.Add("OPPONENT");
            lvMatchHistory.Columns.Add("RESULT");
            lvMatchHistory.Columns.Add("SCORE");
            lvMatchHistory.Columns.Add("GAME");
            lvMatchHistory.Columns.Add("LEAGUE");
            lvMatchHistory.Columns.Add("DATE");
            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    string opponent;
                    string result;
                    if (match.HomePlayer.NickName == player.NickName)
                    {
                        opponent = match.AwayPlayer.NickName;
                        if (match.HomeScore > match.AwayScore)
                            result = "W";
                        else if (match.HomeScore < match.AwayScore)
                            result = "L";
                        else
                            result = "D";
                    }
                    else
                    {
                        opponent = match.HomePlayer.NickName;
                        if (match.HomeScore > match.AwayScore)
                            result = "L";
                        else if (match.HomeScore < match.AwayScore)
                            result = "W";
                        else
                            result = "D";
                    }

                    ListViewItem lvi = new ListViewItem(opponent);
                    lvi.SubItems.Add(result);
                    lvi.SubItems.Add(match.HomeScore.ToString() + " - " + match.AwayScore.ToString());
                    lvi.SubItems.Add(match.League.Game.Title);
                    lvi.SubItems.Add(match.League.Name);
                    lvi.SubItems.Add(match.DatePlayed.ToString("dd/MM/yyyy"));

                    lvMatchHistory.Items.Add(lvi);
                }
            }

            session.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
