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
using GamingLeagues.Forms.Games;

namespace GamingLeagues.Forms
{
    public partial class GamesForm : Form
    {
        private ISession m_session;

        private IList<Game> m_games;

        public GamesForm()
        {
            InitializeComponent();

            m_session = DataAccessLayer.DataAccessLayer.GetSession();

            lvGames.Clear();
            lvGames.Columns.Add("TITLE");
            lvGames.Columns.Add("DEVELOPER");
            lvGames.Columns.Add("RELEASE DATE");
            lvGames.Columns.Add("GENRE");
        }

        private void RefreshGames()
        {
            // Clear the items inside the listView
            lvGames.Items.Clear();

            // Grab the players with a query from the open session
            IQuery q = m_session.CreateQuery("FROM Game");
            m_games = q.List<Game>();

            // Iterate and add data from the players
            foreach (Game game in m_games)
            {
                ListViewItem lvi = new ListViewItem(game.Title);
                lvi.SubItems.Add(game.Developer);
                lvi.SubItems.Add(game.ReleaseDate.ToString("dd/MM/yyyy"));
                lvi.SubItems.Add(game.Genre);
                lvi.Tag = game;

                lvGames.Items.Add(lvi);
            }

            // Adjust initial column widths
            ListView.ColumnHeaderCollection lch = lvGames.Columns;
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

        private void addGame_Click(object sender, EventArgs e)
        {
            GamesAddForm addForm = new GamesAddForm();

            if (addForm.ShowDialog() == DialogResult.OK)
                RefreshGames();
        }

        private void editGame_Click(object sender, EventArgs e)
        {
            //To be implemented
        }

        private void deleteGame_Click(object sender, EventArgs e)
        {
            //To be implemented
        }

        private void detailsGame_Click(object sender, EventArgs e)
        {
            //To be implemented
        }

        private void closeGames_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion
    }
}
