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
    public partial class GamesEditForm : Form
    {
        private DataManagement.DataManagement m_dataManager;

        private Game m_game;

        private IList<Player> m_players;
        private IList<League> m_leagues;
        private IList<Platform> m_platforms;

        public GamesEditForm(Game game, DataManagement.DataManagement dataManager)
        {
            InitializeComponent();

            m_dataManager = dataManager;
            m_game = game;

            //Loading players, leagues and platforms into checked list boxes
            m_players = m_dataManager.getPlayers();
            clbPlayers.Items.Clear();
            clbPlayers.DataSource = m_players;
            clbPlayers.DisplayMember = "Nickname";

            IList<League> leagues = m_dataManager.getLeagues();
            foreach (League league in leagues)
                if (league.Game == null ||
                    league.Game == m_game)
                    m_leagues.Add(league);
            clbLeagues.Items.Clear();
            clbLeagues.DataSource = m_leagues;
            clbLeagues.DisplayMember = "Name";

            m_platforms = m_dataManager.getPlatforms();
            clbSupportedPlatforms.Items.Clear();
            clbSupportedPlatforms.DataSource = m_platforms;
            clbSupportedPlatforms.DisplayMember = "PlatformTitle";
        }

        private void onLoad(object sender, EventArgs e)
        {
            InitializePlayerData();
        }

        private void InitializePlayerData()
        {
            tbTitle.Text = m_game.Title;
            tbDeveloper.Text = m_game.Developer;
            dtpReleaseDate.Value = m_game.ReleaseDate;
            tbGenre.Text = m_game.Genre;

            foreach (Player player in m_game.Players)
            {
                for (int i = 0; i < clbPlayers.Items.Count; i++)
                {
                    Player currentPlayer = clbPlayers.Items[i] as Player;
                    if (m_game.Players.Contains(currentPlayer))
                        clbPlayers.SetItemChecked(i, true);
                }
            }

            foreach (League league in m_game.Leagues)
            {
                for (int i = 0; i < clbLeagues.Items.Count; i++)
                {
                    League currentLeague = clbLeagues.Items[i] as League;
                    if (m_game.Leagues.Contains(currentLeague))
                        clbPlayers.SetItemChecked(i, true);
                }
            }

            foreach (Platform platform in m_game.SupportedPlatforms)
            {
                for (int i = 0; i < clbSupportedPlatforms.Items.Count; i++)
                {
                    Platform currentPlatform = clbSupportedPlatforms.Items[i] as Platform;
                    if (m_game.SupportedPlatforms.Contains(currentPlatform))
                        clbPlayers.SetItemChecked(i, true);
                }
            }
        }

        private bool ValidateInput()
        {
            // TO DO: check various bounds and legit cases for player
            return true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //Creating players, leagues and platforms lists from checked list boxes
            CheckedListBox.CheckedItemCollection selectedPlayers = clbPlayers.CheckedItems;
            List<Player> players = new List<Player>();
            foreach (var item in selectedPlayers)
            {
                Player player = item as Player;
                players.Add(player);
            }

            CheckedListBox.CheckedItemCollection selectedLeagues = clbLeagues.CheckedItems;
            List<League> leagues = new List<League>();
            foreach (var item in selectedLeagues)
            {
                League league = item as League;
                leagues.Add(league);
            }

            CheckedListBox.CheckedItemCollection selectedPlatforms = clbSupportedPlatforms.CheckedItems;
            List<Platform> platforms = new List<Platform>();
            foreach (var item in selectedPlatforms)
            {
                Platform platform = item as Platform;
                platforms.Add(platform);
            }

            try
            {
                m_dataManager.updateGame(m_game,
                                        tbTitle.Text,
                                        tbDeveloper.Text,
                                        dtpReleaseDate.Value,
                                        tbGenre.Text,
                                        platforms,
                                        leagues,
                                        players);
                DialogResult = DialogResult.OK;
            }
            catch (Exception saveExc)
            {
                MessageBox.Show("Failed to save current session:\n" + saveExc.Message);
                DialogResult = DialogResult.Cancel;
            }
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
