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
using GamingLeagues.Forms.Sponsors;

namespace GamingLeagues.Forms
{
    public partial class SponsorsForm : Form
    {
        private ISession m_session;

        private IList<Sponsor> m_sponsors;

        public SponsorsForm()
        {
            InitializeComponent();

            m_session = DataAccessLayer.DataAccessLayer.GetSession();

            lvSponsors.Clear();
            lvSponsors.Columns.Add("NAME");
            lvSponsors.Columns.Add("LOGO");
        }

        private void RefreshSponsors()
        {
            lvSponsors.Items.Clear();

            IQuery q = m_session.CreateQuery("FROM Sponsor");
            m_sponsors = q.List<Sponsor>();

            foreach (Sponsor sponsor in m_sponsors)
            {
                ListViewItem lvi = new ListViewItem(sponsor.Name);
                lvi.SubItems.Add(sponsor.Logo);
                lvi.Tag = sponsor;

                lvSponsors.Items.Add(lvi);
            }

            ListView.ColumnHeaderCollection lch = lvSponsors.Columns;
            for (int i = 0; i < lch.Count; i++)
            {
                lch[i].Width = -1;
                int dataSize = lch[i].Width;
                lch[i].Width = -2;
                int colSize = lch[i].Width;
                lch[i].Width = dataSize > colSize ? -1 : -2;
            }
        }

        private void onLoad(object sender, EventArgs e)
        {
            RefreshSponsors();
        }

        private void onClosing(object sender, FormClosingEventArgs e)
        {
            m_session.Close();
        }

        public Sponsor GetSelectedSponsor()
        {
            if (lvSponsors.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a sponsor first.", "Error");
                return null;
            }

            // Otherwise, return the first selected one
            Sponsor sponsor = (Sponsor)lvSponsors.SelectedItems[0].Tag;
            return sponsor;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            SponsorsAddForm addForm = new SponsorsAddForm();

            if (addForm.ShowDialog() == DialogResult.OK)
            {
                RefreshSponsors();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            Sponsor selectedSponsor = GetSelectedSponsor();

            if (selectedSponsor != null)
            {
                SponsorsEditForm editForm = new SponsorsEditForm(m_session, selectedSponsor);

                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    RefreshSponsors();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            Sponsor selectedSponsor = GetSelectedSponsor();

            if (selectedSponsor != null &&
                MessageBox.Show("Are you sure you want to delete the selected player?",
                                "Delete Player",
                                MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                selectedSponsor.Teams.Clear();
                m_session.SaveOrUpdate(selectedSponsor);
                m_session.Flush();

                selectedSponsor.Leagues.Clear();
                m_session.SaveOrUpdate(selectedSponsor);
                m_session.Flush();

                m_session.Delete(selectedSponsor);
                m_session.Flush();

                RefreshSponsors();
            }
        }

        private void btnDetails_Click(object sender, EventArgs e)
        {
            Sponsor selectedSponsor = GetSelectedSponsor();

            if (selectedSponsor != null)
            {
                SponsorsDetailsForm playersDetailsForm = new SponsorsDetailsForm(selectedSponsor.Id);
                playersDetailsForm.Show();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
