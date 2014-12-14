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
    public partial class MatchesAddForm : Form
    {
        private League m_league;
        private IList<Player> m_players;

        public MatchesAddForm(/*ISession session, League league*/int leagueId)
        {
            InitializeComponent();

            ISession session = DataAccessLayer.DataAccessLayer.GetSession();
            m_league = session.Get<League>(leagueId);
            m_players = new List<Player>();

            //Iz nekog razloga ako u oba combo box-a stavim isti data source
            //kad selektujem item u jednoj isti se selektuje i u drugoj
            //Zbog toga postoji pomocna lista player-a.
            GetPlayers(session);

            cmbHomePlayer.Items.Clear();
            cmbHomePlayer.DataSource = m_league.Players;
            cmbHomePlayer.DisplayMember = "NameNickLast";

            cmbAwayPlayer.Items.Clear();
            cmbAwayPlayer.DataSource = m_players;
            cmbAwayPlayer.DisplayMember = "NameNickLast";

            session.Close();
        }

        private void GetPlayers(ISession session)
        {
            IList<Player> players = session.CreateQuery("FROM Player").List<Player>();
            for (int i = 0; i < players.Count; i++)
                foreach (League league in players[i].Leagues)
                    if (league.Id == m_league.Id)
                        m_players.Add(players[i]);
        }

        private void SetAttributes(Match match)
        {
            match.DatePlayed = dtpMatchDate.Value;
            match.HomeScore = int.Parse(tbHomeScore.Text);
            match.AwayScore = int.Parse(tbAwayScore.Text);

            match.Players.Clear();
            match.Players.Add(cmbHomePlayer.SelectedItem as Player);
            match.Players.Add(cmbAwayPlayer.SelectedItem as Player);

            match.League = m_league;
        }

        private bool ValidateInput()
        {
            IList<string> errorMessages = new List<string>();

            if (cmbHomePlayer.SelectedItem == cmbAwayPlayer.SelectedItem)
                errorMessages.Add("Home player and away player cannot be the same person");

            DateTime pickedDate = dtpMatchDate.Value;
            if (pickedDate < m_league.StartDate || pickedDate > m_league.EndDate)
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
                Match match = new Match();
                SetAttributes(match);

                ISession session = DataAccessLayer.DataAccessLayer.GetSession();

                try
                {
                    session.SaveOrUpdate(match);
                    session.Flush();

                    session.SaveOrUpdate(m_league);
                    session.Flush();

                    this.DialogResult = DialogResult.OK;
                }
                catch (Exception saveExc)
                {
                    MessageBox.Show("Failed to save match in current session:\n" + saveExc.Message);
                    this.DialogResult = DialogResult.Cancel;
                }
                finally
                {
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
