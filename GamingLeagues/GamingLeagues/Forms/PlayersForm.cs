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
            lvPlayers.Columns.Add("GENDER");
            lvPlayers.Columns.Add("BIRTH DATE");
            lvPlayers.Columns.Add("COUNTRY");
            lvPlayers.Columns.Add("DATE TURNED PRO");
            lvPlayers.Columns.Add("CAREER EARNINGS");
            lvPlayers.Columns.Add("TEAM");
        }

        private void RefreshPlayers()
        {
            // Clear the items inside the listView
            lvPlayers.Items.Clear();

            // Grab the players with a query from the open session
            IQuery q = m_session.CreateQuery("FROM Player");
            m_players = q.List<Player>();

            // Iterate and add data from the players
            bool colorizer = false;
            foreach (Player pl in m_players)
            {
                ListViewItem lvi = new ListViewItem(pl.NickName);
                lvi.SubItems.Add(pl.Name);
                lvi.SubItems.Add(pl.LastName);
                lvi.SubItems.Add(pl.Gender.ToString());
                lvi.SubItems.Add(pl.DateOfBirth.ToString("dd/MM/yyyy"));
                lvi.SubItems.Add(pl.Country);
                lvi.SubItems.Add(pl.DateTurnedPro.ToString("dd/MM/yyyy"));
                lvi.SubItems.Add(pl.CareerEarnings.ToString() + " $");
                lvi.SubItems.Add(pl.CurrentTeam != null ? pl.CurrentTeam.Name : "--None--");
                lvi.Tag = pl;
                if (colorizer == false)
                    lvi.BackColor = Color.Orange;
                else
                    lvi.BackColor = Color.Moccasin;
                colorizer = !colorizer;

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

        private void PlayersForm_Load(object sender, EventArgs e)
        {
            // Initial display
            RefreshPlayers();
        }

        private void PlayersForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_session.Close();
        }

        public Player GetSelectedPlayer()
        {
            // If no designers have been selected, display error message
            if (lvPlayers.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a player first.", "Error");
                return null;
            }

            // Otherwise, return the first selected one
            Player player = (Player)lvPlayers.SelectedItems[0].Tag;
            return player;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            PlayersAddForm addDesignerForm = new PlayersAddForm();

            // Show the created form as a dialog
            if (addDesignerForm.ShowDialog() == DialogResult.OK)
            {
                // Refresh the listView if user confirmed the addition
                RefreshPlayers();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            // Get the selected player
            Player selPlayer = GetSelectedPlayer();

            if (selPlayer != null)
            {
                PlayersEditForm editPlayerForm = new PlayersEditForm(m_session, selPlayer);

                if (editPlayerForm.ShowDialog() == DialogResult.OK)
                {
                    // Refresh the listView if user confirmed the edit
                    RefreshPlayers();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Get the selected player
            Player selPlayer = GetSelectedPlayer();

            if (selPlayer != null && MessageBox.Show("Are you sure you want to delete the selected player?",
                "Delete Player", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                // Remove all links to matches played
                selPlayer.MatchesPlayed.Clear();

                // Remove all games associated with player
                selPlayer.Games.Clear();

                selPlayer.CurrentTeam = null;

                // Remove this player from all leagues
                foreach (League league in selPlayer.Leagues)
                {
                    league.Players.Remove(selPlayer);
                    m_session.SaveOrUpdate(league);
                    m_session.Flush();
                }
                selPlayer.Leagues.Clear();

                m_session.SaveOrUpdate(selPlayer);
                m_session.Flush();

                // Delete the selected designer
                m_session.Delete(selPlayer);
                m_session.Flush();

                RefreshPlayers();
            }
        }

        private void btnDetails_Click(object sender, EventArgs e)
        {
            // Get the selected player
            Player selPlayer = GetSelectedPlayer();

            if (selPlayer != null)
            {
                PlayersDetailsForm playersDetailsForm = new PlayersDetailsForm(selPlayer.Id);

                playersDetailsForm.Show();
            }
        }

        private void lvPlayers_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // Get the selected player
            Player selPlayer = GetSelectedPlayer();

            if (selPlayer != null)
            {
                PlayersDetailsForm playersDetailsForm = new PlayersDetailsForm(selPlayer.Id);

                playersDetailsForm.Show();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
