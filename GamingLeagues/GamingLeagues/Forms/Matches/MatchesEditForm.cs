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

namespace GamingLeagues.Forms.Matches
{
    public partial class MatchesEditForm : Form
    {
        private ISession m_session;

        private Match m_match;
        private IList<Player> m_players;

        public MatchesEditForm(int matchId)
        {
            InitializeComponent();

            m_session = DataAccessLayer.DataAccessLayer.GetSession();
            m_match = m_session.Get<Match>(matchId);
        }

        private void onLoad(object sender, EventArgs e)
        {
            if (m_match == null)
            {
                MessageBox.Show("Error receiving data on match");
                this.Close();
            }

            InitializeMatchData();
        }

        private void InitializeMatchData()
        {
            lblLeague.Text = m_match.League.Name;
            dtpMatchDate.Value = m_match.DatePlayed;
            tbHomeScore.Text = m_match.HomeScore.ToString();
            tbAwayScore.Text = m_match.AwayScore.ToString();

            cmbHomePlayer.Items.Clear();
            cmbHomePlayer.DataSource = m_match.League.Players;
            cmbHomePlayer.DisplayMember = "NameNickLast";
            cmbHomePlayer.SelectedItem = m_match.Players[0];

            //Iz nekog razloga ako u oba combo box-a stavim isti data source
            //kad selektujem item u jednoj isti se selektuje i u drugoj
            //Zbog toga postoji pomocna lista player-a.
            m_players = new List<Player>();
            GetPlayers(m_session);

            cmbAwayPlayer.Items.Clear();
            cmbAwayPlayer.DataSource = m_players;
            cmbAwayPlayer.DisplayMember = "NameNickLast";
            cmbAwayPlayer.SelectedItem = m_match.Players[1];
        }

        private void GetPlayers(ISession session)
        {
            IList<Player> players = session.CreateQuery("FROM Player").List<Player>();
            for (int i = 0; i < players.Count; i++)
                foreach (League league in players[i].Leagues)
                    if (league.Id == m_match.League.Id)
                        m_players.Add(players[i]);
        }

        private void SetAttributes(Match match)
        {
            //League cannot be changed so it is not reset
            match.DatePlayed = dtpMatchDate.Value;
            match.HomeScore = int.Parse(tbHomeScore.Text);
            match.AwayScore = int.Parse(tbAwayScore.Text);

            match.Players.Clear();
            match.Players.Add(cmbHomePlayer.SelectedItem as Player);
            match.Players.Add(cmbAwayPlayer.SelectedItem as Player);
        }

        private bool ValidateInput()
        {
            IList<string> errorMessages = new List<string>();

            if (cmbHomePlayer.SelectedItem == cmbAwayPlayer.SelectedItem)
                errorMessages.Add("Home player and away player cannot be the same person");

            DateTime pickedDate = dtpMatchDate.Value;
            if (pickedDate < m_match.League.StartDate || pickedDate > m_match.League.EndDate)
                errorMessages.Add("Match must be played within the league schedule");

            int temp;

            try
            {
                temp = int.Parse(tbHomeScore.Text);
                if (temp < 0)
                    errorMessages.Add("Home score has to be a nonnegative number");
            }
            catch (Exception)
            {
                errorMessages.Add("Home score has to be a number");
            }

            try
            {
                temp = int.Parse(tbAwayScore.Text);
                if (temp < 0)
                    errorMessages.Add("Away score has to be a nonnegative number");
            }
            catch (Exception)
            {
                errorMessages.Add("Away score has to be a number");
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
                SetAttributes(m_match);

                try
                {
                    m_session.SaveOrUpdate(m_match);
                    m_session.Flush();

                    DialogResult = DialogResult.OK;
                }
                catch (Exception saveExc)
                {
                    MessageBox.Show("Failed to save current session:\n" + saveExc.Message);
                    DialogResult = DialogResult.Cancel;
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
    }
}
