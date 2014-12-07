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
using GamingLeagues.Forms;


namespace GamingLeagues
{
    public partial class MainForm : Form
    {
        private ISession m_session;

        public MainForm()
        {
            InitializeComponent();

            /*m_session = DataAccessLayer.DataAccessLayer.GetSession();

            Player player = new Player();
            player.Name = "Jang";
            player.LastName = "Jae-Ho";

            m_session.SaveOrUpdate(player);

            m_session.Flush();

            m_session.Close();*/
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormCollection fc = Application.OpenForms;
            bool foundForm = false;
            foreach (Form frm in fc)
            {
                if (frm is PlayersForm)
                {
                    foundForm = true;
                    frm.BringToFront();
                    break;
                }
            }
            if (!foundForm)
            {
                PlayersForm playersForm = new PlayersForm();
                playersForm.Show();
            }
        }
    }
}
