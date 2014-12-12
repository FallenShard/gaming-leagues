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
using GamingLeagues.Forms.Leagues;

namespace GamingLeagues.Forms.Leagues
{
    public partial class LeaguesDetailsForm : Form
    {
        private League m_league;

        public LeaguesDetailsForm(League league)
        {
            InitializeComponent();

            /*lbPlayers.Items.Clear();
            lbLeagues.Items.Clear();
            lbSupportedPlatforms.Items.Clear();*/

            m_league = league;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
