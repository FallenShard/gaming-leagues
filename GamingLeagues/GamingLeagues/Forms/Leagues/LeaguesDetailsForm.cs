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
using GamingLeagues.DataAccessLayer;
using GamingLeagues.Forms.Leagues;
using GamingLeagues.Forms.Matches;
using GamingLeagues.Forms.Players;
using GamingLeagues.Forms.Sponsors;
using System.Collections;

namespace GamingLeagues.Forms.Leagues
{
    //CELA FORMA JE MOZDA MALO SKARABUDZENA, ALI JE SKROZ POVEZANA SA MECEVIMA(ADD, EDIT, DELETE)
    //ZA MECEVE BOLJE DA UZMEMO LISTVIEW KAO KOD IGRACA JER GA CENIM DA CE DA BUDE DOSTA LAKSE
    //SAMO SAD SA LISTVIEW IMA PROBLEM STO MI NE RADI GetSelectedMatch() POGLEDAJ AKO MOZES TO DA RESIS
    //TI AKO VIDIS JOS NEKI PROPUST JAVI PA CEMO DA RESIMO I TO
    public partial class LeaguesDetailsForm : Form
    {
        private int m_leagueId;

        private ISession m_session;

        private League m_league;

        private IList<Player> m_players;
        private IList<Match> m_matches;

        public LeaguesDetailsForm(int leagueId)
        {
            InitializeComponent();

            m_leagueId = leagueId;

            m_session = DataAccessLayer.DataAccessLayer.GetSession();

            lvPlayers.Clear();
            lvPlayers.Columns.Add("POINTS");
            lvPlayers.Columns.Add("NICKNAME");
            lvPlayers.Columns.Add("FIRST NAME");
            lvPlayers.Columns.Add("LAST NAME");
            lvPlayers.Columns.Add("TEAM");
            lvPlayers.Columns.Add("BIRTH DATE");
            lvPlayers.Columns.Add("COUNTRY");
            lvPlayers.ListViewItemSorter = new ListViewItemComparer(0);

            lvMatches.Clear();
            lvMatches.Columns.Add("HOME PLAYER");
            lvMatches.Columns.Add("HOME SCORE");
            lvMatches.Columns.Add("AWAY SCORE");
            lvMatches.Columns.Add("AWAY PLAYER");
            lvMatches.Columns.Add("DATE PLAYED");

            lbSponsors.Items.Clear();
        }

        private void RefreshMatches()
        {
            // Clear the items inside the listView
            lvMatches.Items.Clear();

            // Grab the matches with a query from the open session
            //m_matches = m_session.CreateQuery("Select Match FROM League l JOIN l.Matches Matches WHERE l.Id = :id")
            //    .SetParameter("id", m_leagueId).List<Match>();
            m_matches = m_league.Matches;

            // Iterate and add data from the players
            bool colorizer = false;
            foreach (Match match in m_matches)
            {
                ListViewItem lvi = new ListViewItem(match.Players[0].ClanName);
                lvi.SubItems.Add(match.HomeScore.ToString());
                lvi.SubItems.Add(match.AwayScore.ToString());
                lvi.SubItems.Add(match.Players[1].ClanName);
                lvi.SubItems.Add(match.DatePlayed.ToString("dd/MM/yyyy"));
                lvi.Tag = match;
                if (colorizer == false)
                    lvi.BackColor = Color.Orange;
                else
                    lvi.BackColor = Color.Moccasin;
                colorizer = !colorizer;

                lvMatches.Items.Add(lvi);
            }

            // Adjust initial column widths
            ListView.ColumnHeaderCollection lch = lvMatches.Columns;
            for (int i = 0; i < lch.Count; i++)
            {
                lch[i].Width = -1;
                int dataSize = lch[i].Width;
                lch[i].Width = -2;
                int colSize = lch[i].Width;
                lch[i].Width = dataSize > colSize ? -1 : -2;
            }
        }

        private void RefreshPlayers()
        {
            // Clear the items inside the listView
            lvPlayers.Items.Clear();

            // Grab the players with a query from the open session
            //m_players = m_session.CreateQuery("Select Player FROM League l JOIN l.Players Player WHERE l.Id = :id")
            //    .SetParameter("gameid", m_leagueId).List<Player>();
            m_players = m_league.Players;

            // Iterate and add data from the players
            foreach (Player pl in m_players)
            {
                int playerPoints = CalcPlayerPoints(pl, m_league);
                ListViewItem lvi = new ListViewItem(playerPoints.ToString());
                lvi.SubItems.Add(pl.ClanName);
                lvi.SubItems.Add(pl.Name);
                lvi.SubItems.Add(pl.LastName);
                lvi.SubItems.Add(pl.CurrentTeam != null ? pl.CurrentTeam.Name : "--None--");
                lvi.SubItems.Add(pl.DateOfBirth.ToString("dd/MM/yyyy"));
                lvi.SubItems.Add(pl.Country);
                lvi.Tag = pl;
                

                lvPlayers.Items.Add(lvi);
            }

            lvPlayers.Sort();

            bool colorizer = false;
            foreach (ListViewItem lvi in lvPlayers.Items)
            {
                if (colorizer == false)
                    lvi.BackColor = Color.Orange;
                else
                    lvi.BackColor = Color.Moccasin;
                colorizer = !colorizer;
            }

            // Adjust initial column widths
            ListView.ColumnHeaderCollection lch = lvPlayers.Columns;
            for (int i = 0; i < lch.Count; i++)
            {
                lch[i].Width = -1;
                int dataSize = lch[i].Width;
                lch[i].Width = -2;
                int colSize = lch[i].Width;
                lch[i].Width = dataSize > colSize ? -1 : -2;
            }
        }

        private void onLoad(object sender, EventArgs e)
        {
            if (m_leagueId == -1)
            {
                MessageBox.Show("Error receiving data on league");
                this.Close();
            }

            InitializeLeagueData();
            RefreshPlayers();
            RefreshMatches();

            IList<Sponsor> sponsors = m_league.Sponsors;
            if (sponsors.Count > 0)
            {
                lbSponsors.DataSource = sponsors;
                lbSponsors.DisplayMember = "Name";
            }
        }

        private void InitializeLeagueData()
        {
            m_league = m_session.Get<League>(m_leagueId);

            this.Text = m_league.Name;

            lblName.Text = m_league.Name;
            lblStartDate.Text = m_league.StartDate.ToString("dd/MM/yyyy");
            lblEndDate.Text = m_league.EndDate.ToString("dd/MM/yyyy");
            lblBudget.Text = m_league.Budget.ToString() + " $";
            lblGame.Text = m_league.Game.Title;

            IList<Sponsor> sponsors = m_league.Sponsors;
            if (sponsors.Count > 0)
            {
                lbSponsors.DataSource = sponsors;
                lbSponsors.DisplayMember = "Name";
            }
        }

        private void btnAddMatch_Click(object sender, EventArgs e)
        {
            MatchesAddForm addMatchForm = new MatchesAddForm(m_leagueId);

            if (addMatchForm.ShowDialog() == DialogResult.OK)
            {
                m_session.Refresh(m_league);
                RefreshMatches();
                RefreshPlayers();
            }
        }

        private void btnEditMatch_Click(object sender, EventArgs e)
        {
            Match selMatch = GetSelectedMatch();

            if (selMatch != null)
            {
                MatchesEditForm editMatchForm = new MatchesEditForm(m_session, selMatch);

                if (editMatchForm.ShowDialog() == DialogResult.OK)
                {
                    m_session.Refresh(m_league);
                    RefreshMatches();
                    RefreshPlayers();
                }
            }
            
        }

        private void btnDeleteMatch_Click(object sender, EventArgs e)
        {
            Match selMatch = GetSelectedMatch();

            if (selMatch != null && MessageBox.Show("Are you sure you want to delete the selected match?",
                "Delete Match", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                m_league.Matches.Remove(selMatch);
                selMatch.League = null;
                selMatch.Players.Clear();
                m_session.SaveOrUpdate(selMatch);
                m_session.Flush();

                m_session.Delete(selMatch);
                m_session.Flush();

                m_session.Refresh(m_league);
                RefreshMatches();
                RefreshPlayers();
            }
        }

        //OVA F-JA UVEK VRACA NULL POGLEDAJ AKO UMES DA RESIS JA NISAM USPEO
        //DOK JE BIO LISTBOX SVE JE RADILO OK TAKO DA EDIT I DELATE RADE OK
        //PRETPOSTAVLJAM DA JE TO ZBOG TOGA STO NE USPE DA KASTUJE TAG U MATCH
        public Match GetSelectedMatch()
        {
            if (lvMatches.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a match first.", "Error");
                return null;
            }

            Match match = (Match)lvMatches.SelectedItems[0].Tag;
            return match;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            m_session.SaveOrUpdate(m_league);
            m_session.Close();

            Close();
        }

        public Player GetSelectedPlayer()
        {
            // If no designers have been selected, display error message
            if (lvPlayers.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a player first.", "Error");
                return null;
            }

            // Otherwise, return the first selected one
            Player player = (Player)lvPlayers.SelectedItems[0].Tag;
            return player;
        }

        private void lvPlayers_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // Get the selected player
            Player selPlayer = GetSelectedPlayer();

            if (selPlayer != null)
            {
                PlayersDetailsForm playersDetailsForm = new PlayersDetailsForm(selPlayer.Id);
                playersDetailsForm.Show();
            }
        }

        private void lbSponsors_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = lbSponsors.IndexFromPoint(e.Location);
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                Sponsor sponsor = lbSponsors.Items[index] as Sponsor;

                SponsorsDetailsForm sponsorDetailsForm = new SponsorsDetailsForm(sponsor.Id);
                sponsorDetailsForm.Show();
            }
        }

        private int CalcPlayerPoints(Player player, League league)
        {
            int result = 0;

            for (int i = 0; i < league.Matches.Count; i++)
            {
                if (league.Matches[i].Players[0].Id == player.Id)
                {
                    if (league.Matches[i].HomeScore > league.Matches[i].AwayScore)
                        result += 3;
                    else if (league.Matches[i].HomeScore == league.Matches[i].AwayScore)
                        result += 1;
                }
                else if (league.Matches[i].Players[1].Id == player.Id)
                {
                    if (league.Matches[i].AwayScore > league.Matches[i].HomeScore)
                        result += 3;
                    else if (league.Matches[i].AwayScore == league.Matches[i].HomeScore)
                        result += 1;
                }
            }

            return result;
        }

        class ListViewItemComparer : IComparer
        {
            private int col;
            public ListViewItemComparer()
            {
                col=0;
            }
            public ListViewItemComparer(int column) 
            {
                col=column;
            }
            public int Compare(object x, object y) 
            {
                int left = Int32.Parse(((ListViewItem)x).SubItems[col].Text);
                int right = Int32.Parse(((ListViewItem)y).SubItems[col].Text);

                if (left > right) return -1;
                else if (left == right) return 0;
                else return 1;
            }
        }
    }
}
