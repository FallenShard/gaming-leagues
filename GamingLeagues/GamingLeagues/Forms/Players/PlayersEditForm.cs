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
        private IList<Team> m_teams;

        public PlayersEditForm(ISession session, Player player)
        {
            InitializeComponent();

            // Received session holds the edited player
            m_session = session;
            m_player = player;

            // Load the available games
            GetGamesAndTeams();

            clbGames.Items.Clear();
            clbGames.DataSource = m_games;
            clbGames.DisplayMember = "Title";

            cbTeams.Items.Clear();
            cbTeams.DataSource = m_teams;
            cbTeams.DisplayMember = "Name";
            cbTeams.SelectedItem = cbTeams.Items[cbTeams.Items.Count - 1];
        }

        private void GetGamesAndTeams()
        {
            m_games = m_session.CreateQuery("FROM Game").List<Game>();

            m_teams = m_session.CreateQuery("FROM Team").List<Team>();
            Team nullTeam = new Team();
            nullTeam.Name = "(None)";
            m_teams.Add(nullTeam);
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

            if (m_player.Gender == 'M') 
                rbMale.Checked = true;
            else 
                rbFemale.Checked = true;

            // Team
            if (m_player.CurrentTeam != null)
            {
                foreach (Team team in m_teams)
                    if (m_player.CurrentTeam.Id == team.Id)
                        cbTeams.SelectedItem = team;
            }
            else
                cbTeams.SelectedItem = cbTeams.Items[cbTeams.Items.Count - 1];

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

            Team team = cbTeams.SelectedItem as Team;
            if (team.Name != "(None)")
                player.CurrentTeam = team;
            else
                player.CurrentTeam = null;

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
            // Concatenated error messages from multiple inputs
            IList<string> errorMessages = new List<string>();

            // First name
            if (tbName.Text.Length == 0 || tbName.Text.Length > 20)
                errorMessages.Add("First name should be 1-20 characters long");
            if (System.Text.RegularExpressions.Regex.IsMatch(tbName.Text, @"\d"))
                errorMessages.Add("First name should contain only alphabet characters");

            // Last name
            if (tbLastName.Text.Length == 0 || tbLastName.Text.Length > 20)
                errorMessages.Add("First name should be 1-20 characters long");
            if (System.Text.RegularExpressions.Regex.IsMatch(tbLastName.Text, @"\d"))
                errorMessages.Add("First name should contain only alphabet characters");

            // Gender
            if (!rbFemale.Checked && !rbMale.Checked)
                errorMessages.Add("Please select a gender for the player");

            // Birth date
            DateTime pickedDate = dtpBirth.Value;
            if (DateTime.Now.Year - pickedDate.Year < 5)
                errorMessages.Add("Player should be at least 5 years old");

            // Pro date
            DateTime proDate = dtpPro.Value;
            if (proDate < pickedDate)
                errorMessages.Add("Player cannot become pro before being born");

            // Country
            if (tbCountry.Text.Length > 40)
                errorMessages.Add("Country should be 0-40 characters long");
            if (System.Text.RegularExpressions.Regex.IsMatch(tbCountry.Text, @"\d"))
                errorMessages.Add("Country should contain only alphabet characters");

            try
            {
                float temp = float.Parse(tbCareer.Text);
            }
            catch (Exception)
            {
                errorMessages.Add("Career earnings has to be a number");
            }

            if (errorMessages.Count == 0)
                return true;
            else
            {
                string message = "The following errors have been found: " + Environment.NewLine + Environment.NewLine;
                foreach (string error in errorMessages)
                    message += "  -  " + error + Environment.NewLine;

                MessageBox.Show(message, Text);
                return false;
            }
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
