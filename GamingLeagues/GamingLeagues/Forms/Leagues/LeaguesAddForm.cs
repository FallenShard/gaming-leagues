using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

using NHibernate;
using GamingLeagues.Entities;

namespace GamingLeagues.Forms.Leagues
{
    public partial class LeaguesAddForm : Form
    {
        private DataManagement.DataManagement m_dataManager;

        private IList<Game> m_games;
        private IList<Sponsor> m_sponsors;
        private IList<Player> m_players;

        public LeaguesAddForm()
        {
            InitializeComponent();

            m_dataManager = new DataManagement.DataManagement();

            //Loading games, sponsors and players into combo box and checked list boxes
            m_games = m_dataManager.getGames();
            cmbbGame.Items.Clear();
            cmbbGame.DataSource = m_games;
            cmbbGame.DisplayMember = "Title";

            m_sponsors = m_dataManager.getSponsors();
            clbSponsors.Items.Clear();
            clbSponsors.DataSource = m_sponsors;
            clbSponsors.DisplayMember = "Name";

            m_players = m_dataManager.getPlayers();
            clbPlayers.Items.Clear();
            clbPlayers.DataSource = m_players;
            clbPlayers.DisplayMember = "Nickname";
        }
        private bool ValidateInput()
        {
            // TO DO: check various bounds and legit cases for player
            return true;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                //Creating game nadplayers and sponsors lists from combo box and checked list boxes
                Game selectedGame = cmbbGame.SelectedItem as Game;

                CheckedListBox.CheckedItemCollection selectedPlayers = clbPlayers.CheckedItems;
                List<Player> players = new List<Player>();
                foreach (var item in selectedPlayers)
                {
                    Player player = item as Player;
                    players.Add(player);
                }

                CheckedListBox.CheckedItemCollection selectedSponsors = clbSponsors.CheckedItems;
                List<Sponsor> sponsors = new List<Sponsor>();
                foreach (var item in selectedSponsors)
                {
                    Sponsor sponsor = item as Sponsor;
                    sponsors.Add(sponsor);
                }

                float budget = float.Parse(tbBudget.Text, CultureInfo.InvariantCulture);

                try
                {
                    m_dataManager.insertLeague(tbName.Text,
                                                dtpStartDate.Value,
                                                dtpEndDate.Value,
                                                budget,
                                                selectedGame,
                                                sponsors,
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
    }
}
