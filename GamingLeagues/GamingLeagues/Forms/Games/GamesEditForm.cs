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

namespace GamingLeagues.Forms.Games
{
    public partial class GamesEditForm : Form
    {
        private ISession m_session;

        private Game m_game;

        private string[] suppPlatforms = new[] { "Windows", "Mac OS X", "Linux", "XBox", "PlayStation", "Wii" };

        public GamesEditForm(ISession session, Game game)
        {
            InitializeComponent();

            m_session = session;
            m_game = game;

            clbSupportedPlatforms.DataSource = suppPlatforms;
        }

        private void onLoad(object sender, EventArgs e)
        {
            if (m_game == null)
            {
                MessageBox.Show("Error receiving data on game");
                this.Close();
            }

            InitializeGameData();
        }

        private void InitializeGameData()
        {
            tbTitle.Text         = m_game.Title;
            tbDeveloper.Text     = m_game.Developer;
            dtpReleaseDate.Value = m_game.ReleaseDate;
            tbGenre.Text         = m_game.Genre;

            IList<Platform> platforms = m_game.SupportedPlatforms;

            foreach (Platform plat in platforms)
            {
                for (int i = 0; i < clbSupportedPlatforms.Items.Count; i++)
                {
                    if (plat.PlatformTitle == (clbSupportedPlatforms.Items[i] as string))
                        clbSupportedPlatforms.SetItemChecked(i, true);
                }
            }
        }

        private void SetAttributes(Game game)
        {
            game.Title       = tbTitle.Text;
            game.Developer   = tbDeveloper.Text;
            game.ReleaseDate = dtpReleaseDate.Value;
            game.Genre       = tbGenre.Text;

            game.SupportedPlatforms.Clear();
            m_session.Update(game);
            m_session.Flush();

            foreach (string plat in clbSupportedPlatforms.CheckedItems)
            {
                Platform platform = new Platform();
                platform.PlatformTitle = plat;
                platform.VideoGame = game;

                game.SupportedPlatforms.Add(platform);
            }
        }

        private bool ValidateInput()
        {
            IList<string> errorMessages = new List<string>();

            if (tbTitle.Text.Length == 0 || tbTitle.Text.Length > 40)
                errorMessages.Add("Game name should be 1-40 characters long");

            if (tbDeveloper.Text.Length == 0 || tbDeveloper.Text.Length > 40)
                errorMessages.Add("Game developer name should be 1-40 characters long");

            DateTime releaseDate = dtpReleaseDate.Value;
            if (releaseDate > DateTime.Now)
                errorMessages.Add("Game cannot be released in the future");

            if (tbGenre.Text.Length == 0 || tbGenre.Text.Length > 30)
                errorMessages.Add("Game genre should be 1-30 characters long");

            if (errorMessages.Count == 0)
                return true;
            else
            {
                string message = "The following errors have been found: " + Environment.NewLine + Environment.NewLine;
                foreach (string error in errorMessages)
                    message += "  -  " + error + Environment.NewLine;

                MessageBox.Show(message, Text);
                return false;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                SetAttributes(m_game);

                try
                {
                    m_session.SaveOrUpdate(m_game);
                    m_session.Flush();

                    DialogResult = DialogResult.OK;
                }
                catch (Exception saveExc)
                {
                    MessageBox.Show("Failed to save current session:\n" + saveExc.Message);
                    DialogResult = DialogResult.Cancel;
                }

                Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to close this window?",
                Text, MessageBoxButtons.OKCancel) == DialogResult.OK)
                Close();
        }
    }
}
