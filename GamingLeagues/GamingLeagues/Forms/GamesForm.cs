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

        private void onLoad(object sender, EventArgs e)
        {
            RefreshGames();
        }

        private void RefreshGames()
        {
            lvGames.Items.Clear();

            // Grab the teams with a query from the open session
            IQuery q = m_session.CreateQuery("FROM Game");
            m_games = q.List<Game>();

            bool colorizer = false;
            foreach (Game game in m_games)
            {
                ListViewItem lvi = new ListViewItem(game.Title);
                lvi.SubItems.Add(game.Developer);
                lvi.SubItems.Add(game.ReleaseDate.ToString("dd/MM/yyyy"));
                lvi.SubItems.Add(game.Genre);
                lvi.Tag = game;
                if (colorizer == false)
                    lvi.BackColor = Color.Orange;
                else
                    lvi.BackColor = Color.Moccasin;
                colorizer = !colorizer;

                lvGames.Items.Add(lvi);
            }

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

        private Game GetSelectedGame()
        {
            if (lvGames.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a game first.", "Error");
                return null;
            }

            Game game = (Game)lvGames.SelectedItems[0].Tag;
            return game;
        }

        #region Button clicks

        private void btnAdd_Click(object sender, EventArgs e)
        {
            GamesAddForm addForm = new GamesAddForm();

            if (addForm.ShowDialog() == DialogResult.OK)
                RefreshGames();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            Game selectedGame = GetSelectedGame();

            if (selectedGame != null)
            {
                GamesEditForm editForm = new GamesEditForm(m_session, selectedGame);

                if (editForm.ShowDialog() == DialogResult.OK)
                    RefreshGames();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            Game selectedGame = GetSelectedGame();

            if (selectedGame != null &&
                MessageBox.Show("Are you sure you want to delete selected game?",
                                "Delete Game",
                                MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                selectedGame.Leagues.Clear();
                selectedGame.Players.Clear();

                m_session.SaveOrUpdate(selectedGame);
                m_session.Flush();

                m_session.Delete(selectedGame);
                m_session.Flush();

                RefreshGames();
            }
        }

        private void btnDetails_Click(object sender, EventArgs e)
        {
            Game selectedGame = GetSelectedGame();

            if (selectedGame != null)
            {
                GamesDetailsForm detailsForm = new GamesDetailsForm(selectedGame.Id);
                detailsForm.Show();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion

        private void lvGames_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // Get the selected game
            Game selGame = GetSelectedGame();

            if (selGame != null)
            {
                GamesDetailsForm gameDetailsForm = new GamesDetailsForm(selGame.Id);
                gameDetailsForm.Show();
            }
        }

        private void GamesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_session.Close();
        }
    }
}