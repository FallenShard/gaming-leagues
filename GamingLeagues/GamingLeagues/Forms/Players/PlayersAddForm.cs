﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

using NHibernate;
using GamingLeagues.Entities;

namespace GamingLeagues.Forms.Players
{
    public partial class PlayersAddForm : Form
    {
        private IList<Game> m_games;

        public PlayersAddForm()
        {
            InitializeComponent();

            GetGames();

            clbGames.Items.Clear();
            clbGames.DataSource = m_games;
            clbGames.DisplayMember = "Title";
        }

        private void GetGames()
        {
            ISession session = DataAccessLayer.DataAccessLayer.GetSession();

            m_games = session.CreateQuery("FROM Game").List<Game>();

            session.Close();
        }

        private void SetAttributes(Player player)
        {
            player.Name = tbName.Text;
            player.LastName = tbLastName.Text;
            player.NickName = tbNickName.Text;
            player.DateOfBirth = dtpBirth.Value;
            player.DateTurnedPro = dtpPro.Value;
            player.Country = tbCountry.Text;
            player.CareerEarnings = float.Parse(tbCareer.Text, CultureInfo.InvariantCulture);
            player.Gender = rbMale.Checked ? 'M' : 'F';

            // Games
            CheckedListBox.CheckedItemCollection selectedGames = clbGames.CheckedItems;

            // Iterate the selected games and add them to the new player
            player.Games.Clear();
            foreach (var item in selectedGames)
            {
                Game game = item as Game;
                player.Games.Add(game);
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
                // Create new designer
                Player player = new Player();
                SetAttributes(player);

                ISession session = DataAccessLayer.DataAccessLayer.GetSession();

                try
                {
                    // Try to save the current designer
                    session.SaveOrUpdate(player);
                    session.Flush();

                    // Everything went fine
                    this.DialogResult = DialogResult.OK;
                }
                catch (Exception saveExc)
                {
                    MessageBox.Show("Failed to save current session:\n" + saveExc.Message);
                    this.DialogResult = DialogResult.Cancel;
                }
                finally
                {
                    // Close the session and the form
                    session.Close();
                    Close();
                }
            }
        }
    }
}
