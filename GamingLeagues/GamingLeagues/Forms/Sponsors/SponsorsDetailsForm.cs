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

            Text = sponsor.Name + " - " + sponsor.Logo;

            lblName.Text = sponsor.Name;
            lblLogo.Text = sponsor.Logo;

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

        private void btnCancle_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
