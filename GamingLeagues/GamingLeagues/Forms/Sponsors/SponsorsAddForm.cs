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
    public partial class SponsorsAddForm : Form
    {
        private IList<Team> m_teams;
        private IList<League> m_leagues;

        public SponsorsAddForm()
        {
            InitializeComponent();

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
            ISession session = DataAccessLayer.DataAccessLayer.GetSession();

            m_teams = session.CreateQuery("FROM Team").List<Team>();
            m_leagues = session.CreateQuery("FROM League").List<League>();

            session.Close();
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
            return true;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                Sponsor sponsor = new Sponsor();
                SetAttributes(sponsor);

                ISession session = DataAccessLayer.DataAccessLayer.GetSession();

                try
                {
                    session.SaveOrUpdate(sponsor);
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

        private void btnCancle_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to close this window?",
                                Text,
                                MessageBoxButtons.OKCancel) == DialogResult.OK)
                Close();
        }
    }
}
