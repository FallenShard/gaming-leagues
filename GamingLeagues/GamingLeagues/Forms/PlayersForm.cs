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
using GamingLeagues.Forms.Players;


namespace GamingLeagues.Forms
{
    public partial class PlayersForm : Form
    {
        private ISession m_session;

        private IList<Player> m_players;

        public PlayersForm()
        {
            InitializeComponent();

            m_session = DataAccessLayer.DataAccessLayer.GetSession();

            lvPlayers.Clear();
            lvPlayers.Columns.Add("NICKNAME");
            lvPlayers.Columns.Add("FIRST NAME");
            lvPlayers.Columns.Add("LAST NAME");
            lvPlayers.Columns.Add("BIRTH DATE");
            lvPlayers.Columns.Add("COUNTRY");
            lvPlayers.Columns.Add("DATE TURNED PRO");
            lvPlayers.Columns.Add("CAREER EARNINGS");
        }

        private void RefreshPlayers()
        {
            // Clear the items inside the listView
            lvPlayers.Items.Clear();

            // Grab the players with a query from the open session
            IQuery q = m_session.CreateQuery("FROM Player");
            m_players = q.List<Player>();

            // Iterate and add data from the players
            foreach (Player pl in m_players)
            {
                ListViewItem lvi = new ListViewItem(pl.NickName);
                lvi.SubItems.Add(pl.Name);
                lvi.SubItems.Add(pl.LastName);
                lvi.SubItems.Add(pl.DateOfBirth.ToString("dd/MM/yyyy"));
                lvi.SubItems.Add(pl.Country);
                lvi.SubItems.Add(pl.DateTurnedPro.ToString("dd/MM/yyyy"));
                lvi.SubItems.Add(pl.CareerEarnings.ToString());
                lvi.Tag = pl;

                lvPlayers.Items.Add(lvi);
            }

            // Adjust initial column widths
            ListView.ColumnHeaderCollection lch = lvPlayers.Columns;
            for (int i = 0; i < lch.Count; i++)
            {
                lch[i].Width = -1;
                int dataSize = lch[i].Width;
                lch[i].Width = -2;
                int colSize = lch[i].Width;
                lch[i].Width = dataSize > colSize ? -1 : -2;
            }
        }

        private void addPlayer_Click(object sender, EventArgs e)
        {
            PlayersAddForm addDesignerForm = new PlayersAddForm();

            // Show the created form as a dialog
            if (addDesignerForm.ShowDialog() == DialogResult.OK)
            {
                // Refresh the listView if user confirmed the addition
                RefreshPlayers();
            }
        }
    }
}
