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
    public partial class PlatformsAddForm : Form
    {
        private DataManagement.DataManagement m_dataManager;

        private Game m_game;
        public PlatformsAddForm(Game game, DataManagement.DataManagement dataManager)
        {
            InitializeComponent();

            m_dataManager = dataManager;
            m_game = game;
        }
        private bool ValidateInput()
        {
            // TO DO: check various bounds and legit cases for player
            return true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                try
                {
                    m_dataManager.insertPlatform(tbPlatformTitle.Text,
                                                m_game);
                    DialogResult = DialogResult.OK;
                }
                catch (Exception saveExc)
                {
                    MessageBox.Show("Failed to save current session:\n" + saveExc.Message);
                    DialogResult = DialogResult.Cancel;
                }
            }
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
