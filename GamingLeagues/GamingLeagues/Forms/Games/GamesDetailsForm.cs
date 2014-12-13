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
using GamingLeagues.Forms.Games;
using GamingLeagues.Forms.Leagues;
using GamingLeagues.Forms.Players;

namespace GamingLeagues.Forms.Games
{
    public partial class GamesDetailsForm : Form
    {
        private int m_gameId = -1;

        public GamesDetailsForm(int gameId)
        {
            InitializeComponent();

            m_gameId = gameId;

            lbPlayers.Items.Clear();
            lbLeagues.Items.Clear();
            lbSupportedPlatforms.Items.Clear();
        }

        private void onLoad(object sender, EventArgs e)
        {
            if (m_gameId == -1)
            {
                MessageBox.Show("Error receiving data on game");
                this.Close();
            }

            InitializeGameData();
        }

        private void InitializeGameData()
        {
            ISession session = DataAccessLayer.DataAccessLayer.GetSession();
            Game game = session.Get<Game>(m_gameId);

            Text = game.Title;
            lblTitle.Text       = game.Title;
            lblDeveloper.Text   = game.Developer;
            lblReleaseDate.Text = game.ReleaseDate.ToString("dd/MM/yyyy");
            lblGenre.Text       = game.Genre;

            IList<Player> players = game.Players;
            if (players.Count > 0)
            {
                lbPlayers.DataSource = players;
                lbPlayers.DisplayMember = "NameNickLast";
            }


            IList<League> leagues = game.Leagues;
            if (leagues.Count > 0)
            {
                lbLeagues.DataSource = leagues;
                lbLeagues.DisplayMember = "Name";
            }

            foreach (Platform platform in game.SupportedPlatforms)
                lbSupportedPlatforms.Items.Add(platform.PlatformTitle);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void lbPlayers_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = lbPlayers.IndexFromPoint(e.Location);
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                Player player = lbPlayers.Items[index] as Player;

                PlayersDetailsForm playerDetailsForm = new PlayersDetailsForm(player.Id);
                playerDetailsForm.Show();
            }
        }
    }
}
