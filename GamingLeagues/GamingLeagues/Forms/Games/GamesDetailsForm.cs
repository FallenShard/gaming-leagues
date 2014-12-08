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

namespace GamingLeagues.Forms.Games
{
    public partial class GamesDetailsForm : Form
    {
        private Game m_game;

        public GamesDetailsForm(Game game)
        {
            InitializeComponent();

            lbPlayers.Items.Clear();
            lbLeagues.Items.Clear();
            lbSupportedPlatforms.Items.Clear();

            m_game = game;
        }

        private void onLoad(object sender, EventArgs e)
        {
            lblTitle.Text = m_game.Title;
            lblDeveloper.Text = m_game.Developer;
            lblReleaseDate.Text = m_game.ReleaseDate.ToString("dd/mm/yyyy");
            lblGenre.Text = m_game.Genre;

            foreach (Player player in m_game.Players)
                lbPlayers.Items.Add(player.NickName);

            foreach (League league in m_game.Leagues)
                lbLeagues.Items.Add(league.Name);

            foreach (Platform platform in m_game.SupportedPlatforms)
                lbSupportedPlatforms.Items.Add(platform.PlatformTitle);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
