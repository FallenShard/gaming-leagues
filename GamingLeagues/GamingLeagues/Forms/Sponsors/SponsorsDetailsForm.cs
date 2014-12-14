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
using GamingLeagues.Forms.Teams;
using GamingLeagues.Forms.Leagues;

namespace GamingLeagues.Forms.Sponsors
{
    public partial class SponsorsDetailsForm : Form
    {
        private int m_sponsorId = -1;

        public SponsorsDetailsForm(int sponsorId)
        {
            InitializeComponent();

            m_sponsorId = sponsorId;

            lbTeams.Items.Clear();
            lbLeagues.Items.Clear();
        }

        private void onLoad(object sender, EventArgs e)
        {
            if (m_sponsorId == -1)
            {
                MessageBox.Show("Error receiving data on player");
                this.Close();
            }

            InitializeSponsorData();
        }

        private void InitializeSponsorData()
        {
            ISession session = DataAccessLayer.DataAccessLayer.GetSession();
            Sponsor sponsor = session.Get<Sponsor>(m_sponsorId);

            Text = sponsor.Name;

            lblName.Text = sponsor.Name;
            lblLogo.Text = sponsor.Slogan;

            IList<Team> teams = sponsor.Teams;
            if (teams.Count > 0)
            {
                lbTeams.DataSource = teams;
                lbTeams.DisplayMember = "Name";
            }

            IList<League> leagues = sponsor.Leagues;
            if (leagues.Count > 0)
            {
                lbLeagues.DataSource = leagues;
                lbLeagues.DisplayMember = "Name";
            }

            session.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void lbTeams_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = lbTeams.IndexFromPoint(e.Location);
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                Team team = lbTeams.Items[index] as Team;

                TeamsDetailsForm teamDetailsForm = new TeamsDetailsForm(team.Id);
                teamDetailsForm.Show();
            }
        }

        private void lbLeagues_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = lbLeagues.IndexFromPoint(e.Location);
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                League league = lbLeagues.Items[index] as League;

                LeaguesDetailsForm leagueDetailsForm = new LeaguesDetailsForm(league.Id);
                leagueDetailsForm.Show();
            }
        }
    }
}
