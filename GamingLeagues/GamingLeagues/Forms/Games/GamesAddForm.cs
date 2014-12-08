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
        private DataManagement.DataManagement m_dataManager;

        public GamesAddForm()
        {
            InitializeComponent();
            m_dataManager = new DataManagement.DataManagement();
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
                    // Try to save the current designer
                    m_dataManager.insertGame(tbTitle.Text,
                                            tbDeveloper.Text,
                                            dtpReleaseDate.Value,
                                            tbGenre.Text,
                                            new List<Platform>(),
                                            new List<League>(),
                                            new List<Player>());


                    // Everything went fine
                    this.DialogResult = DialogResult.OK;
                }
                catch (Exception saveExc)
                {
                    MessageBox.Show("Failed to save current session:\n" + saveExc.Message);
                    this.DialogResult = DialogResult.Cancel;
                }
            }
        }
    }
}
