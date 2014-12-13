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
        //private ISession m_session;

        private League m_league;

        public MatchesAddForm(/*ISession session, */League league)
        {
            InitializeComponent();

            //m_session = session;
            m_league = league;

            //GetPlayers();

            cmbHomePlayer.Items.Clear();
            cmbHomePlayer.DataSource = m_league.Players;
            cmbHomePlayer.DisplayMember = "NameNickLast";

            cmbAwayPlayer.Items.Clear();
            cmbAwayPlayer.DataSource = m_league.Players;
            cmbAwayPlayer.DisplayMember = "NameNickLast";
        }

        /*private void GetPlayers()
        {
            IList<Player> players = m_session.CreateQuery("FROM Player").List<Player>();
            for (int i = 0; i < players.Count; i++)
                foreach (League league in players[i].Leagues)
                    if (league.Id == m_league.Id)
                        m_players.Add(players[i]);
        }*/

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

            DateTime pickedDate = dtpMatchDate.Value;
            if (DateTime.Now < pickedDate)
                errorMessages.Add("Match cannot be played in the future.");

            int temp;

            try
            {
                temp = int.Parse(tbHomeScore.Text);
                if (temp < 0)
                    errorMessages.Add("Home score has to be a positive number");
            }
            catch (Exception)
            {
                errorMessages.Add("Career earnings has to be a number");
            }

            try
            {
                temp = int.Parse(tbAwayScore.Text);
                if (temp < 0)
                    errorMessages.Add("Away score has to be a positive number");
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

                    this.DialogResult = DialogResult.OK;
                }
                catch (Exception saveExc)
                {
                    MessageBox.Show("Failed to save player in current session:\n" + saveExc.Message);
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
