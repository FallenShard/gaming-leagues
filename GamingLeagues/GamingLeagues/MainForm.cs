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
using GamingLeagues.DataAccessLayer;
using GamingLeagues.Entities;


namespace GamingLeagues
{
    public partial class MainForm : Form
    {
        private DataManagement.DataManagement m_dataManager;

        public MainForm()
        {
            InitializeComponent();
        }

        private void onLoad(object sender, EventArgs e)
        {
            m_dataManager = new DataManagement.DataManagement();

            m_dataManager.initializeDataBase();
        }
    }
}
