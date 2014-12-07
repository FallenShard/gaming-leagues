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

namespace GamingLeagues.Forms.Players
{
    public partial class PlayersEditForm : Form
    {
        private ISession m_session;

        private Player m_player;           // Designer currently being edited

        // List of available games
        private IList<Game> m_games;

        public PlayersEditForm(ISession session, Player player)
        {
            InitializeComponent();

            // Received session holds the edited player
            m_session = session;
            m_player = player;

            // Load the available games
            GetGames();

            clbGames.Items.Clear();
            clbGames.DataSource = m_games;
            clbGames.DisplayMember = "Title";
        }

        private void GetGames()
        {
            m_games = m_session.CreateQuery("FROM Game").List<Game>();
        }

        private void InitializePlayerData()
        {
            tbNickName.Text = m_player.NickName;
            tbName.Text = m_player.Name;
            tbLastName.Text = m_player.LastName;
            tbCareer.Text = m_player.CareerEarnings.ToString();
            tbCountry.Text = m_player.Country;
            dtpBirth.Value = m_player.DateOfBirth;
            dtpPro.Value = m_player.DateTurnedPro;

            if (m_player.Gender == 'M') rbMale.Checked = true;
            else rbFemale.Checked = true;

            // Check all games that player is linked to
            IList<Game> playerGames = m_player.Games;
            foreach (Game game in playerGames)
            {
                for (int i = 0; i < clbGames.Items.Count; i++)
                {
                    Game availableGame = clbGames.Items[i] as Game;
                    if (availableGame.Id == game.Id)
                        clbGames.SetItemChecked(i, true);
                }
            }
        }

        private void SetAttributes(Player player)
        {
            player.Name = tbName.Text;
            player.LastName = tbLastName.Text;
            player.NickName = tbNickName.Text;
            player.DateOfBirth = dtpBirth.Value;
            player.DateTurnedPro = dtpPro.Value;
            player.Country = tbCountry.Text;
            player.CareerEarnings = float.Parse(tbCareer.Text, CultureInfo.InvariantCulture);
            player.Gender = rbMale.Checked ? 'M' : 'F';

            // Games
            CheckedListBox.CheckedItemCollection selectedGames = clbGames.CheckedItems;

            // Iterate the selected games and add them to the new player
            player.Games.Clear();
            foreach (var item in selectedGames)
            {
                Game game = item as Game;
                player.Games.Add(game);
            }
        }

        private bool ValidateInput()
        {
            // TODO
            return true;
        }

        private void PlayersEditForm_Load(object sender, EventArgs e)
        {
            if (m_player == null)
            {
                MessageBox.Show("Error receiving data on player");
                this.Close();
            }

            // Load player data into UI controls
            InitializePlayerData();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                try
                {
                    SetAttributes(m_player);
                    // Try to save the current player
                    m_session.SaveOrUpdate(m_player);
                    m_session.Flush();

                    // Everything went fine
                    this.DialogResult = DialogResult.OK;
                }
                catch (Exception saveExc)
                {
                    MessageBox.Show("Failed to save current session:\n" + saveExc.Message);
                    this.DialogResult = DialogResult.Cancel;
                }

                // Close the form
                Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to close this window?",
                Text, MessageBoxButtons.OKCancel) == DialogResult.OK)
                Close();
        }
    }
}
