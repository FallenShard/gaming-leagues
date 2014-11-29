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
        private ISession m_session;

        public MainForm()
        {
            InitializeComponent();

            m_session = DataAccessLayer.DataAccessLayer.GetSession();

            Player player = new Player();
            player.Name = "Jang";
            player.LastName = "Jae-Ho";

            m_session.SaveOrUpdate(player);

            m_session.Flush();

            m_session.Close();
        }
    }
}
