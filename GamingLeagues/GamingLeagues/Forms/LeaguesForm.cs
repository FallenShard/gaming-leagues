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

        private void RefreshLeagues()
        {
            // Clear the items inside the listView
            lvLeagues.Items.Clear();

            // Grab the players with a query from the open session
            IQuery q = m_session.CreateQuery("FROM League");
            m_leagues = q.List<League>();

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

        #region Button clicks

        private void addLeague_Click(object sender, EventArgs e)
        {
            LeaguesAddForm addForm = new LeaguesAddForm();

            if (addForm.ShowDialog() == DialogResult.OK)
                RefreshLeagues();
        }

        private void editLeague_Click(object sender, EventArgs e)
        {

        }

        private void deleteLeague_Click(object sender, EventArgs e)
        {

        }

        private void detailsLeague_Click(object sender, EventArgs e)
        {

        }

        private void closeLeagues_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion
    }
}