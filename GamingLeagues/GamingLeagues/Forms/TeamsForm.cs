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
using GamingLeagues.Forms.Teams;

namespace GamingLeagues.Forms
{
    public partial class TeamsForm : Form
    {
        private ISession m_session;

        private IList<Team> m_teams;

        public TeamsForm()
        {
            InitializeComponent();

            m_session = DataAccessLayer.DataAccessLayer.GetSession();

            lvTeams.Clear();
            lvTeams.Columns.Add("NAME");
            lvTeams.Columns.Add("TAG");
            lvTeams.Columns.Add("DATE CREATED");
            lvTeams.Columns.Add("COUNTRY");
        }

        private void RefreshTeams()
        {
            // Clear the items inside the listView
            lvTeams.Items.Clear();

            // Grab the teams with a query from the open session
            IQuery q = m_session.CreateQuery("FROM Team");
            m_teams = q.List<Team>();

            // Iterate and add data from the players
            foreach (Team team in m_teams)
            {
                ListViewItem lvi = new ListViewItem(team.Name);
                lvi.SubItems.Add(team.Tag);
                lvi.SubItems.Add(team.DateCreated.ToString("dd/MM/yyyy"));
                lvi.SubItems.Add(team.Country);
                lvi.Tag = team;

                lvTeams.Items.Add(lvi);
            }

            // Adjust initial column widths
            ListView.ColumnHeaderCollection lch = lvTeams.Columns;
            for (int i = 0; i < lch.Count; i++)
            {
                lch[i].Width = -1;
                int dataSize = lch[i].Width;
                lch[i].Width = -2;
                int colSize = lch[i].Width;
                lch[i].Width = dataSize > colSize ? -1 : -2;
            }
        }

        private void TeamsForm_Load(object sender, EventArgs e)
        {
            // Initial display
            RefreshTeams();
        }

        private void TeamsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_session.Close();
        }

        public Team GetSelectedTeam()
        {
            // If no designers have been selected, display error message
            if (lvTeams.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a team first.", "Error");
                return null;
            }

            // Otherwise, return the first selected one
            Team team = (Team)lvTeams.SelectedItems[0].Tag;
            return team;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            TeamsAddForm addTeamForm = new TeamsAddForm();

            // Show the created form as a dialog
            if (addTeamForm.ShowDialog() == DialogResult.OK)
            {
                // Refresh the listView if user confirmed the addition
                RefreshTeams();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            // Get the selected player
            Team selTeam = GetSelectedTeam();

            if (selTeam != null)
            {
                //TeamsEditForm editTeamForm = new TeamsEditForm(m_session, selTeam);

                //if (editTeamForm.ShowDialog() == DialogResult.OK)
                //{
                    // Refresh the listView if user confirmed the edit
                //    RefreshTeams();
                //}
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Get the selected player
            Team selTeam = GetSelectedTeam();

            if (selTeam != null && MessageBox.Show("Are you sure you want to delete the selected team?",
                "Delete Team", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                // Remove all links to sponsors
                selTeam.Sponsors.Clear();
                m_session.SaveOrUpdate(selTeam);
                m_session.Flush();

                // Remove all associations with players
                foreach (Player player in selTeam.Players)
                    player.CurrentTeam = null;
                selTeam.Players.Clear();
                m_session.SaveOrUpdate(selTeam);
                m_session.Flush();

                // Delete the selected designer
                m_session.Delete(selTeam);
                m_session.Flush();

                RefreshTeams();
            }
        }

        private void btnDetails_Click(object sender, EventArgs e)
        {
            // Get the selected team
            Team selTeam = GetSelectedTeam();

            if (selTeam != null)
            {
                //TeamsDetailsForm teamDetailsForm = new TeamsDetailsForm(selTeam.Id);

                //teamDetailsForm.Show();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
