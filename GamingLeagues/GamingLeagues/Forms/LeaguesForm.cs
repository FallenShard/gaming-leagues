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
using GamingLeagues.Forms.Leagues;

namespace GamingLeagues.Forms
{
    public partial class LeaguesForm : Form
    {
        private ISession m_session;

        private IList<League> m_leagues;

        public LeaguesForm()
        {
            InitializeComponent();

            m_session = DataAccessLayer.DataAccessLayer.GetSession();

            lvLeagues.Clear();
            lvLeagues.Columns.Add("NAME");
            lvLeagues.Columns.Add("START DATE");
            lvLeagues.Columns.Add("END DATE");
            lvLeagues.Columns.Add("BUDGET");
        }

        private void onLoad(object sender, EventArgs e)
        {
            RefreshLeagues();
        }

        private void RefreshLeagues()
        {
            lvLeagues.Items.Clear();

            m_leagues = m_session.CreateQuery("FROM League").List<League>();

            // Iterate and add data from the leagues
            bool colorizer = false;
            foreach (League league in m_leagues)
            {
                ListViewItem lvi = new ListViewItem(league.Name);
                lvi.SubItems.Add(league.StartDate.ToString("dd/MM/yyyy"));
                lvi.SubItems.Add(league.EndDate.ToString("dd/MM/yyyy"));
                lvi.SubItems.Add(league.Budget.ToString());
                lvi.Tag = league;
                if (colorizer == false)
                    lvi.BackColor = Color.Orange;
                else
                    lvi.BackColor = Color.Moccasin;
                colorizer = !colorizer;

                lvLeagues.Items.Add(lvi);
            }

            // Adjust initial column widths
            ListView.ColumnHeaderCollection lch = lvLeagues.Columns;
            for (int i = 0; i < lch.Count; i++)
            {
                lch[i].Width = -1;
                int dataSize = lch[i].Width;
                lch[i].Width = -2;
                int colSize = lch[i].Width;
                lch[i].Width = dataSize > colSize ? -1 : -2;
            }
        }

        private League GetSelectedLeague()
        {
            if (lvLeagues.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a league first.", "Error");
                return null;
            }

            League league = (League)lvLeagues.SelectedItems[0].Tag;
            return league;
        }

        #region Button clicks

        private void btnAdd_Click(object sender, EventArgs e)
        {
            LeaguesAddForm addForm = new LeaguesAddForm();

            if (addForm.ShowDialog() == DialogResult.OK)
                RefreshLeagues();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            League selLeague = GetSelectedLeague();

            if (selLeague != null)
            {
                LeaguesEditForm editLeague = new LeaguesEditForm(m_session, selLeague);

                if (editLeague.ShowDialog() == DialogResult.OK)
                {
                    // Refresh the listView if user confirmed the edit
                    RefreshLeagues();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            League selectedLeague = GetSelectedLeague();

            if (selectedLeague != null &&
                MessageBox.Show("Are you sure you want to delete selected league?",
                                "Delete League",
                                MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                selectedLeague.Matches.Clear();
                selectedLeague.Players.Clear();

                foreach (Sponsor sponsor in selectedLeague.Sponsors)
                    sponsor.Leagues.Remove(selectedLeague);
                selectedLeague.Sponsors.Clear();

                m_session.SaveOrUpdate(selectedLeague);
                m_session.Flush();

                m_session.Delete(selectedLeague);
                m_session.Flush();

                RefreshLeagues();
            }
        }

        private void btnDetails_Click(object sender, EventArgs e)
        {
            League selectedLeague = GetSelectedLeague();

            if (selectedLeague != null)
            {
                LeaguesDetailsForm detailsForm = new LeaguesDetailsForm(selectedLeague.Id);
                detailsForm.Show();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion

        private void LeaguesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_session.Close();
        }

        private void lvLeagues_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            League selectedLeague = GetSelectedLeague();

            if (selectedLeague != null)
            {
                LeaguesDetailsForm detailsForm = new LeaguesDetailsForm(selectedLeague.Id);
                detailsForm.Show();
            }
        }
    }
}