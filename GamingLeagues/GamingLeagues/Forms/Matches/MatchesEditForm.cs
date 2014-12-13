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

        public MatchesEditForm(ISession session, Match match)
        {
            InitializeComponent();

            m_session = session;
            m_match = match;
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

            cmbAwayPlayer.Items.Clear();
            cmbAwayPlayer.DataSource = m_match.League.Players;
            cmbAwayPlayer.DisplayMember = "NameNickLast";
            cmbAwayPlayer.SelectedItem = m_match.Players[1];
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
