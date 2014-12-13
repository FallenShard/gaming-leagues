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

namespace GamingLeagues.Forms.Sponsors
{
    public partial class SponsorsEditForm : Form
    {
        private ISession m_session;

        private Sponsor m_sponsor;

        private IList<Team> m_teams;
        private IList<League> m_leagues;

        public SponsorsEditForm(ISession session, Sponsor sponsor)
        {
            InitializeComponent();

            m_session = session;
            m_sponsor = sponsor;

            GetTeamsAndLeagues();

            clbTeams.Items.Clear();
            clbTeams.DataSource = m_teams;
            clbTeams.DisplayMember = "Name";

            clbLeagues.Items.Clear();
            clbLeagues.DataSource = m_leagues;
            clbLeagues.DisplayMember = "Name";
        }

        private void GetTeamsAndLeagues()
        {
            m_teams = m_session.CreateQuery("FROM Team").List<Team>();
            m_leagues = m_session.CreateQuery("FROM League").List<League>();
        }

        private void InitializeSponsorData()
        {
            tbName.Text = m_sponsor.Name;
            tbLogo.Text = m_sponsor.Slogan;

            IList<Team> sponsorTeams = m_sponsor.Teams;
            foreach (Team team in sponsorTeams)
            {
                for (int i = 0; i < clbTeams.Items.Count; i++)
                {
                    Team availableTeam = clbTeams.Items[i] as Team;
                    if (availableTeam.Id == team.Id)
                        clbTeams.SetItemChecked(i, true);
                }
            }

            IList<League> sponsprLeagues = m_sponsor.Leagues;
            foreach (League league in sponsprLeagues)
            {
                for (int i = 0; i < clbLeagues.Items.Count; i++)
                {
                    League availableLeague = clbLeagues.Items[i] as League;
                    if (availableLeague.Id == league.Id)
                        clbLeagues.SetItemChecked(i, true);
                }
            }
        }

        private void SetAttributes(Sponsor sponsor)
        {
            sponsor.Name = tbName.Text;
            sponsor.Slogan = tbLogo.Text;

            CheckedListBox.CheckedItemCollection selectedTeams = clbTeams.CheckedItems;
            sponsor.Teams.Clear();
            foreach (var item in selectedTeams)
            {
                Team team = item as Team;
                sponsor.Teams.Add(team);
            }

            CheckedListBox.CheckedItemCollection selectedLeagues = clbLeagues.CheckedItems;
            sponsor.Leagues.Clear();
            foreach (var item in selectedLeagues)
            {
                League league = item as League;
                sponsor.Leagues.Add(league);
            }
        }

        private bool ValidateInput()
        {
            //Only check list boxes in SponsorEditForm, so input is always valid
            return true;
        }

        private void onLoad(object sender, EventArgs e)
        {
            if (m_sponsor == null)
            {
                MessageBox.Show("Error receiving data on sponsor");
                this.Close();
            }

            InitializeSponsorData();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                try
                {
                    SetAttributes(m_sponsor);

                    m_session.SaveOrUpdate(m_sponsor);
                    m_session.Flush();

                    this.DialogResult = DialogResult.OK;
                }
                catch (Exception saveExc)
                {
                    MessageBox.Show("Failed to save player in current session:\n" + saveExc.Message);
                    this.DialogResult = DialogResult.Cancel;
                }
                finally
                {
                    Close();
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to close this window?",
                                Text,
                                MessageBoxButtons.OKCancel) == DialogResult.OK)
                Close();
        }
    }
}
