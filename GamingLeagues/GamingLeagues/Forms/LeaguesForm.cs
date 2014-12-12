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
        private DataManagement.DataManagement m_dataManager;

        private IList<League> m_leagues;

        public LeaguesForm()
        {
            InitializeComponent();

            m_dataManager = new DataManagement.DataManagement();

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

            m_leagues = m_dataManager.getLeagues();

            // Iterate and add data from the players
            foreach (League league in m_leagues)
            {
                ListViewItem lvi = new ListViewItem(league.Name);
                lvi.SubItems.Add(league.StartDate.ToString("dd/MM/yyyy"));
                lvi.SubItems.Add(league.EndDate.ToString("dd/MM/yyyy"));
                lvi.SubItems.Add(league.Budget.ToString());
                lvi.Tag = league;

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

        private League getSelectedLeague()
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

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            League selectedLeague = getSelectedLeague();

            if (selectedLeague != null &&
                MessageBox.Show("Are you sure you want to delete selected league?",
                                "Delete League",
                                MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                m_dataManager.deleteLeague(selectedLeague);
                RefreshLeagues();
            }
        }

        private void btnDetails_Click(object sender, EventArgs e)
        {
            League selectedLeague = getSelectedLeague();

            if (selectedLeague != null)
            {
                LeaguesDetailsForm detailsForm = new LeaguesDetailsForm(selectedLeague);
                detailsForm.Show();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion
    }
}