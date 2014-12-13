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

namespace GamingLeagues.Forms.Teams
{
    public partial class TeamsEditForm : Form
    {
        private ISession m_session;

        private Team m_team;           // Team currently being edited

        private IList<Player> m_freePlayers = new List<Player>();

        public TeamsEditForm(ISession session, Team team)
        {
            InitializeComponent();

            // Received session holds the edited player
            m_session = session;
            m_team = team;

            // Load the available games
            GetPlayers();

            clbPlayers.Items.Clear();
            clbPlayers.DataSource = m_freePlayers;
            clbPlayers.DisplayMember = "NameNickLast";
        }

        private void GetPlayers()
        {
            // Grab the available players with a query
            IList<Player> players = m_session.CreateQuery("FROM Player").List<Player>();
            for (int i = 0; i < players.Count; i++)
                if (players[i].CurrentTeam == null || players[i].CurrentTeam.Id == m_team.Id) 
                    m_freePlayers.Add(players[i]);
        }

        private void InitializeTeamData()
        {
            tbName.Text = m_team.Name;
            tbTag.Text = m_team.Tag;
            tbCountry.Text = m_team.Country;
            dtpCreated.Value = m_team.DateCreated;

            // Check all players that team is linked to
            IList<Player> teamPlayers = m_team.Players;
            foreach (Player player in teamPlayers)
            {
                for (int i = 0; i < clbPlayers.Items.Count; i++)
                {
                    Player possiblePlayer = clbPlayers.Items[i] as Player;
                    if (possiblePlayer.Id == player.Id)
                        clbPlayers.SetItemChecked(i, true);
                }
            }
        }

        private void SetAttributes(Team team)
        {
            team.Name = tbName.Text;
            team.Tag = tbTag.Text;
            team.Country = tbCountry.Text;
            team.DateCreated = dtpCreated.Value;

            // Players
            CheckedListBox.CheckedItemCollection players = clbPlayers.CheckedItems;
            foreach (Player player in team.Players)
                player.CurrentTeam = null;
            team.Players.Clear();

            // Iterate the selected players and add them to the team
            foreach (var item in players)
            {
                Player player = item as Player;
                player.CurrentTeam = team;
                team.Players.Add(player);
            }
        }

        private bool ValidateInput()
        {
            // Concatenated error messages from multiple inputs
            IList<string> errorMessages = new List<string>();

            // Pro date
            DateTime createdDate = dtpCreated.Value;
            if (createdDate > DateTime.Now)
                errorMessages.Add("Cannot create a team in the future");

            // Country
            if (tbCountry.Text.Length > 40)
                errorMessages.Add("Country should be 0-40 characters long");
            if (System.Text.RegularExpressions.Regex.IsMatch(tbCountry.Text, @"\d"))
                errorMessages.Add("Country should contain only alphabet characters");

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

        private void TeamsEditForm_Load(object sender, EventArgs e)
        {
            if (m_team == null)
            {
                MessageBox.Show("Error receiving data on team");
                this.Close();
            }

            // Load team data into UI controls
            InitializeTeamData();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                try
                {
                    SetAttributes(m_team);

                    // Try to save the current team
                    m_session.SaveOrUpdate(m_team);
                    m_session.Flush();

                    // Everything went fine
                    this.DialogResult = DialogResult.OK;
                }
                catch (Exception saveExc)
                {
                    MessageBox.Show("Failed to save team in current session:\n" + saveExc.Message);
                    this.DialogResult = DialogResult.Cancel;
                }
                finally
                {
                    // Close the form
                    Close();
                }
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
