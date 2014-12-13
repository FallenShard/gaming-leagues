using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using NHibernate;
using GamingLeagues.DataAccessLayer;
using GamingLeagues.Entities;
using GamingLeagues.Forms;


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

            if (DataAccessLayer.DataAccessLayer.NeedsInitialization())
            {
                m_dataManager.initializeDataBase();
            }
        }

        private void btnPlayers_Click(object sender, EventArgs e)
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

        private void btnTeams_Click(object sender, EventArgs e)
        {
            FormCollection fc = Application.OpenForms;
            bool foundForm = false;
            foreach (Form frm in fc)
            {
                if (frm is TeamsForm)
                {
                    foundForm = true;
                    frm.BringToFront();
                    break;
                }
            }
            if (!foundForm)
            {
                TeamsForm teamsForm = new TeamsForm();
                teamsForm.Show();
            }
        }

        private void btnLeagues_Click(object sender, EventArgs e)
        {
            FormCollection fc = Application.OpenForms;
            bool foundForm = false;
            foreach (Form frm in fc)
            {
                if (frm is LeaguesForm)
                {
                    foundForm = true;
                    frm.BringToFront();
                    break;
                }
            }
            if (!foundForm)
            {
                LeaguesForm leaguesForm = new LeaguesForm();
                leaguesForm.Show();
            }
        }

        private void btnSponsors_Click(object sender, EventArgs e)
        {
            FormCollection fc = Application.OpenForms;
            bool foundForm = false;
            foreach (Form frm in fc)
            {
                if (frm is SponsorsForm)
                {
                    foundForm = true;
                    frm.BringToFront();
                    break;
                }
            }
            if (!foundForm)
            {
                SponsorsForm sponsorsForm = new SponsorsForm();
                sponsorsForm.Show();
            }
        }

        private void btnGames_Click(object sender, EventArgs e)
        {
            FormCollection fc = Application.OpenForms;
            bool foundForm = false;
            foreach (Form frm in fc)
            {
                if (frm is GamesForm)
                {
                    foundForm = true;
                    frm.BringToFront();
                    break;
                }
            }
            if (!foundForm)
            {
                GamesForm gamesForm = new GamesForm();
                gamesForm.Show();
            }
        }
    }
}
