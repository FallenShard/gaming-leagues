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

namespace GamingLeagues.Forms.Games
{
    public partial class GamesAddForm : Form
    {
        private DataManagement.DataManagement m_dataManager;

        private IList<Player> m_players;
        private IList<League> m_leagues;
        private IList<Platform> m_platforms;

        public GamesAddForm()
        {
            InitializeComponent();

            m_dataManager = new DataManagement.DataManagement();

            //Loading players, leagues and platforms into checked list boxes
            m_players = m_dataManager.getPlayers();
            clbPlayers.Items.Clear();
            clbPlayers.DataSource = m_players;
            clbPlayers.DisplayMember = "Nickname";

            IList<League> leagues = m_dataManager.getLeagues();
            foreach (League league in leagues)
                if (league.Game == null)
                    m_leagues.Add(league);
            clbLeagues.Items.Clear();
            clbLeagues.DataSource = m_leagues;
            clbLeagues.DisplayMember = "Name";

            IList<Platform> platforms = m_dataManager.getPlatforms();
            foreach (Platform platform in platforms)
                if (platform.VideoGame == null)
                    m_platforms.Add(platform);
            //////////NASTAVITI
            clbSupportedPlatforms.Items.Clear();
            clbSupportedPlatforms.DataSource = m_platforms;
            clbSupportedPlatforms.DisplayMember = "PlatformTitle";
        }

        private bool ValidateInput()
        {
            // TO DO: check various bounds and legit cases for player
            return true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
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
                    m_dataManager.insertGame(tbTitle.Text,
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
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
        private Platform getSelectedPlatform()
        {
            if (clbPlayers.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a platform first.", "Error");
                return null;
            }

            //Platform platform = (Platform)clbPlayers.SelectedItems[0].Tag;
            //return platform;

            return new Platform();
        }

        private void refreshPlatforms()
        {
            IList<Platform> allPlatforms = m_dataManager.getPlatforms();
            foreach (Platform platform in allPlatforms)
                ;
        }

        private void btnAddPlatform_Click(object sender, EventArgs e)
        {

        }

        private void btnDeletePlatform_Click(object sender, EventArgs e)
        {
            Platform platform = getSelectedPlatform();

            if (platform != null &&
                MessageBox.Show("Are you sure you want to delete selected platform?",
                                "Delete Game",
                                MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                m_dataManager.deletePlatform(platform);
                refreshPlatforms();
            }
        }
    }
}