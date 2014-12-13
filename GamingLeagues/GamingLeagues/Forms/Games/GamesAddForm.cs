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
    public partial class GamesAddForm : Form
    {
        public GamesAddForm()
        {
            InitializeComponent();

            var suppPlatforms = new[] { "Windows", "Mac OS X", "Linux", "XBox", "PS", "Wii" };
            
            clbSupportedPlatforms.Items.Clear();
            clbSupportedPlatforms.DataSource = suppPlatforms;
        }

        private void SetAttributes(Game game)
        {
            game.Title = tbTitle.Text;
            game.Developer = tbDeveloper.Text;
            game.Genre = tbGenre.Text;
            game.ReleaseDate = dtpReleaseDate.Value;

            foreach (string platform in clbSupportedPlatforms.CheckedItems)
            {
                Platform plat = new Platform();
                plat.PlatformTitle = platform;
                plat.VideoGame = game;

                game.SupportedPlatforms.Add(plat);
            }
        }

        private bool ValidateInput()
        {
            // TO DO: check various bounds and legit cases for player
            return true;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                Game game = new Game();
                SetAttributes(game);

                ISession session = DataAccessLayer.DataAccessLayer.GetSession();

                try
                {
                    session.Save(game);
                    session.Flush();

                    DialogResult = DialogResult.OK;
                }
                catch (Exception saveExc)
                {
                    MessageBox.Show("Failed to save current session:\n" + saveExc.Message);
                    DialogResult = DialogResult.Cancel;
                }
                finally
                {
                    session.Close();
                }
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