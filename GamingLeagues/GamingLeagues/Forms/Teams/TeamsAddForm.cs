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
    public partial class TeamsAddForm : Form
    {
        private IList<Player> m_freePlayers = new List<Player>();
        
        public TeamsAddForm()
        {
            InitializeComponent();

            // Load the available players
            GetPlayers();

            // Clear the listbox, set players as data source and display their names
            clbPlayers.Items.Clear();
            clbPlayers.DataSource = m_freePlayers;
            clbPlayers.DisplayMember = "NameNickLast";
        }

        private void GetPlayers()
        {
            // Get a session to read in players
            ISession session = DataAccessLayer.DataAccessLayer.GetSession();

            // Grab the available players with a query
            IList<Player> players = session.CreateQuery("FROM Player").List<Player>();
            for (int i = 0; i < players.Count; i++)
                if (players[i].CurrentTeam == null) m_freePlayers.Add(players[i]);

            // Close the opened session
            session.Close();
        }

        private void SetAttributes(Team team)
        {
            team.Name = tbName.Text;
            team.Tag = tbTag.Text;
            team.Country = tbCountry.Text;
            team.DateCreated = dtpCreated.Value;

            // Players
            CheckedListBox.CheckedItemCollection players = clbPlayers.CheckedItems;

            // Iterate the selected models and add them to the new agency
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

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                // Create new team
                Team team = new Team();
                SetAttributes(team);

                ISession session = DataAccessLayer.DataAccessLayer.GetSession();

                try
                {
                    // Try to save the current player
                    session.SaveOrUpdate(team);
                    session.Flush();

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
                    // Close the session and the form
                    session.Close();
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
