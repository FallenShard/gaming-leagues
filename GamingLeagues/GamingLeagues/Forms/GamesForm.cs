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
        private DataManagement.DataManagement m_dataManager;

        private IList<Game> m_games;

        public GamesForm()
        {
            InitializeComponent();

            m_dataManager = new DataManagement.DataManagement();

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

            m_games = m_dataManager.getGames();

            foreach (Game game in m_games)
            {
                ListViewItem lvi = new ListViewItem(game.Title);
                lvi.SubItems.Add(game.Developer);
                lvi.SubItems.Add(game.ReleaseDate.ToString("dd/MM/yyyy"));
                lvi.SubItems.Add(game.Genre);
                lvi.Tag = game;

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

        private Game getSelectedGame()
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
            Game selectedGame = getSelectedGame();

            if (selectedGame != null)
            {
                GamesEditForm editForm = new GamesEditForm(selectedGame, m_dataManager);

                if (editForm.ShowDialog() == DialogResult.OK)
                    RefreshGames();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            Game selectedGame = getSelectedGame();

            if (selectedGame != null &&
                MessageBox.Show("Are you sure you want to delete selected game?",
                                "Delete Game",
                                MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                m_dataManager.deleteGame(selectedGame);
                RefreshGames();
            }
        }

        private void btnDetails_Click(object sender, EventArgs e)
        {
            Game selectedGame = getSelectedGame();

            if (selectedGame != null)
            {
                GamesDetailsForm detailsForm = new GamesDetailsForm(selectedGame);
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