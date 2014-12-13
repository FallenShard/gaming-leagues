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
using GamingLeagues.Forms.Players;
using GamingLeagues.Forms.Sponsors;

namespace GamingLeagues.Forms.Teams
{
    public partial class TeamsDetailsForm : Form
    {
        private int m_teamId = -1;

        public TeamsDetailsForm(int teamId)
        {
            InitializeComponent();

            m_teamId = teamId;

            lbPlayers.Items.Clear();
            lbSponsors.Items.Clear();
        }

        private void TeamsDetailsForm_Load(object sender, EventArgs e)
        {
            if (m_teamId == -1)
            {
                MessageBox.Show("Error receiving data on team");
                this.Close();
            }

            // Load team data into UI controls
            InitializeTeamData();
        }

        private void InitializeTeamData()
        {
            ISession session = DataAccessLayer.DataAccessLayer.GetSession();
            Team team = session.Get<Team>(m_teamId);

            this.Text = team.Name;

            lblName.Text    = team.Name;
            lblTag.Text     = team.Tag;
            lblDate.Text    = team.DateCreated.ToString("dd/MM/yyyy");
            lblCountry.Text = team.Country;

            IList<Player> players = team.Players;
            if (players.Count > 0)
            {
                lbPlayers.DataSource = players;
                lbPlayers.DisplayMember = "NameNickLast";
            }

            IList<Sponsor> sponsors = team.Sponsors;
            if (sponsors.Count > 0)
            {
                lbSponsors.DataSource = sponsors;
                lbSponsors.DisplayMember = "Name";
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void lbPlayers_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = lbPlayers.IndexFromPoint(e.Location);
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                Player player = lbPlayers.Items[index] as Player;

                PlayersDetailsForm playerDetailsForm = new PlayersDetailsForm(player.Id);
                playerDetailsForm.Show();
            }
        }

        private void lbSponsors_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = lbSponsors.IndexFromPoint(e.Location);
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                Sponsor sponsor = lbSponsors.Items[index] as Sponsor;

                SponsorsDetailsForm sponsorDetailsForm = new SponsorsDetailsForm(sponsor.Id);
                sponsorDetailsForm.Show();
            }
        }
    }
}
