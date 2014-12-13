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
using GamingLeagues.DataAccessLayer;
using GamingLeagues.Forms.Leagues;
using GamingLeagues.Forms.Matches;

namespace GamingLeagues.Forms.Leagues
{
    public partial class LeaguesDetailsForm : Form
    {
        private int m_leagueId;

        public LeaguesDetailsForm(int leagueId)
        {
            InitializeComponent();

            m_leagueId = leagueId;

            lbSponsors.Items.Clear();
            lbPlayers.Items.Clear();
            lbMatches.Items.Clear();
        }

        private void onLoad(object sender, EventArgs e)
        {
            if (m_leagueId == -1)
            {
                MessageBox.Show("Error receiving data on player");
                this.Close();
            }

            // Load player data into UI controls
            InitializeLeagueData();
        }

        private void InitializeLeagueData()
        {
            ISession session = DataAccessLayer.DataAccessLayer.GetSession();
            League league = session.Get<League>(m_leagueId);

            this.Text = league.Name;

            lblName.Text = league.Name;
            lblStartDate.Text = league.StartDate.ToString("dd/MM/yyyy");
            lblEndDate.Text = league.EndDate.ToString("dd/MM/yyyy");
            lblBudget.Text = league.Budget.ToString() + "$";
            lblGame.Text = league.Game.Title;

            IList<Sponsor> sponsors = league.Sponsors;
            if (sponsors.Count > 0)
            {
                lbSponsors.DataSource = sponsors;
                lbSponsors.DisplayMember = "Name";
            }

            IList<Player> players = league.Players;
            if (players.Count > 0)
            {
                lbPlayers.DataSource = players;
                lbPlayers.DisplayMember = "NameNickLast";
            }

            session.Close();
        }

        private void RefreshMatches()
        {
            //TODO
        }

        private void btnAddMatch_Click(object sender, EventArgs e)
        {
            MatchesAddForm addDesignerForm = new MatchesAddForm(m_leagueId);

            if (addDesignerForm.ShowDialog() == DialogResult.OK)
            {
                RefreshMatches();
            }
        }

        private void btnEditMatch_Click(object sender, EventArgs e)
        {

        }

        private void btnDeleteMatch_Click(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
