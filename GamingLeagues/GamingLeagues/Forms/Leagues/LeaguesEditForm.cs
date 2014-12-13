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
    public partial class LeaguesEditForm : Form
    {
        private ISession m_session;

        private League m_league;
        private IList<Game> m_games;


        public LeaguesEditForm(ISession session, League league)
        {
            InitializeComponent();

            m_session = session;
            m_league = league;

            GetGames();
            cmbbGame.Items.Clear();
            cmbbGame.DataSource = m_games;
            cmbbGame.DisplayMember = "Title";
        }

        private void GetGames()
        {
            m_games = m_session.CreateQuery("FROM Game").List<Game>();
        }

        private void InitializeLeagueData()
        {
            tbName.Text = m_league.Name;
            tbBudget.Text = m_league.Budget.ToString();
            dtpEndDate.Value = m_league.EndDate;
            dtpStartDate.Value = m_league.StartDate;

            foreach (Game game in m_games)
                if (m_league.Game.Id == game.Id)
                    cmbbGame.SelectedItem = game;
        }

        private void SetAttributes(League league)
        {
            league.Name = tbName.Text;
            league.Budget = float.Parse(tbBudget.Text, CultureInfo.InvariantCulture);
            league.StartDate = dtpStartDate.Value;
            league.EndDate = dtpEndDate.Value;

            league.Game = cmbbGame.SelectedItem as Game;

            CheckedListBox.CheckedItemCollection selected = clbPlayers.CheckedItems;

            // Iterate the selected players and add them to the new league
            league.Players.Clear();
            foreach (var item in selected)
            {
                Player player = item as Player;
                league.Players.Add(player);
            }
        }

        private bool ValidateInput()
        {
            IList<string> errorMessages = new List<string>();

            if (tbName.Text.Length == 0 || tbName.Text.Length > 40)
                errorMessages.Add("League name should be 1-40 characters long");

            DateTime endDate = dtpEndDate.Value;
            if (endDate > DateTime.Now)
                errorMessages.Add("League cannot be ended in the future");

            DateTime startDate = dtpStartDate.Value;
            if (startDate > endDate)
                errorMessages.Add("League cannot start after it is finished");

            try
            {
                float temp = float.Parse(tbBudget.Text);
                if (temp < 0)
                    errorMessages.Add("League budget has to be a nonnegative number");
            }
            catch (Exception)
            {
                errorMessages.Add("League budget has to be a number");
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

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                SetAttributes(m_league);
                
                try
                {
                    m_session.SaveOrUpdate(m_league);
                    m_session.Flush();

                    // Everything went fine
                    this.DialogResult = DialogResult.OK;
                }
                catch (Exception saveExc)
                {
                    MessageBox.Show("Failed to save current session:\n" + saveExc.Message);
                    this.DialogResult = DialogResult.Cancel;
                }

                Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to close this window?",
                Text, MessageBoxButtons.OKCancel) == DialogResult.OK)
                Close();
        }

        private void LeaguesEditForm_Load(object sender, EventArgs e)
        {
            if (m_league == null)
            {
                MessageBox.Show("Error receiving data on league");
                this.Close();
            }

            // Load league data into UI controls
            InitializeLeagueData();
        }

        private void cmbbGame_SelectedIndexChanged(object sender, EventArgs e)
        {
            Game selGame = cmbbGame.SelectedItem as Game;

            IList<Player> players = m_session.CreateQuery("Select Player FROM Game g JOIN g.Players Player WHERE g.Id = :gameid")
                .SetParameter("gameid", selGame.Id).List<Player>();

            clbPlayers.DataSource = players;
            clbPlayers.DisplayMember = "NameNickLast";


            for (int i = 0; i < clbPlayers.Items.Count; i++)
            {
                foreach (Player leaguePlayer in m_league.Players)
                {
                    if (leaguePlayer.Id == (clbPlayers.Items[i] as Player).Id)
                        clbPlayers.SetItemChecked(i, true);
                }
            }
        }
    }
}
