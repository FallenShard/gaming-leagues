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

namespace GamingLeagues.Forms.Leagues
{
    //CELA FORMA JE MOZDA MALO SKARABUDZENA, ALI JE SKROZ POVEZANA SA MECEVIMA(ADD, EDIT, DELETE)
    //ZA MECEVE BOLJE DA UZMEMO LISTVIEW KAO KOD IGRACA JER GA CENIM DA CE DA BUDE DOSTA LAKSE
    //SAMO SAD SA LISTVIEW IMA PROBLEM STO MI NE RADI GetSelectedMatch() POGLEDAJ AKO MOZES TO DA RESIS
    //TI AKO VIDIS JOS NEKI PROPUST JAVI PA CEMO DA RESIMO I TO
    public partial class LeaguesDetailsForm : Form
    {
        private int m_leagueId;

        public LeaguesDetailsForm(int leagueId)
        {
            InitializeComponent();

            m_leagueId = leagueId;

            lbSponsors.Items.Clear();
            lbPlayers.Items.Clear();

            lvMatches.Clear();
            lvMatches.Columns.Add("HOME PLAYER");
            lvMatches.Columns.Add("HOME SCORE");
            lvMatches.Columns.Add("AWAY SCORE");
            lvMatches.Columns.Add("AWAY PLAYER");
            lvMatches.Columns.Add("DATE PLAYED");
        }

        private void onLoad(object sender, EventArgs e)
        {
            if (m_leagueId == -1)
            {
                MessageBox.Show("Error receiving data on player");
                this.Close();
            }

            InitializeLeagueData();
        }

        private void InitializeLeagueData()
        {
            ISession session = DataAccessLayer.DataAccessLayer.GetSession();
            League league = session.Get<League>(m_leagueId);

            this.Text = league.Name;

            lblName.Text = league.Name;
            lblStartDate.Text = league.StartDate.ToString("dd/MM/yyyy");
            lblEndDate.Text = league.EndDate.ToString("dd/MM/yyyy");
            lblBudget.Text = league.Budget.ToString() + "$";
            lblGame.Text = league.Game.Title;

            IList<Sponsor> sponsors = league.Sponsors;
            if (sponsors.Count > 0)
            {
                lbSponsors.DataSource = sponsors;
                lbSponsors.DisplayMember = "Name";
            }

            IList<Player> players = league.Players;
            if (players.Count > 0)
            {
                lbPlayers.DataSource = players;
                lbPlayers.DisplayMember = "NameNickLast";
            }

            IList<Match> matches = league.Matches;
            if (matches.Count > 0)
                foreach (Match match in matches)
                {
                    ListViewItem lvi = new ListViewItem(match.Players[0].NickName);
                    lvi.SubItems.Add(match.HomeScore.ToString());
                    lvi.SubItems.Add(match.AwayScore.ToString());
                    lvi.SubItems.Add(match.Players[1].NickName);
                    lvi.SubItems.Add(match.DatePlayed.ToString("dd/MM/yyyy"));

                    lvMatches.Items.Add(lvi);
                }

            ListView.ColumnHeaderCollection lch = lvMatches.Columns;
            for (int i = 0; i < lch.Count; i++)
            {
                lch[i].Width = -1;
                int dataSize = lch[i].Width;
                lch[i].Width = -2;
                int colSize = lch[i].Width;
                lch[i].Width = dataSize > colSize ? -1 : -2;
            }

            session.Close();
        }

        private void RefreshMatches()
        {
            ISession session = DataAccessLayer.DataAccessLayer.GetSession();
            League league = session.Get<League>(m_leagueId);

            lvMatches.Items.Clear();
            IList<Match> matches = league.Matches;
            if (matches.Count > 0)
                foreach (Match match in matches)
                {
                    ListViewItem lvi = new ListViewItem(match.Players[0].NickName);
                    lvi.SubItems.Add(match.HomeScore.ToString());
                    lvi.SubItems.Add(match.AwayScore.ToString());
                    lvi.SubItems.Add(match.Players[1].NickName);
                    lvi.SubItems.Add(match.DatePlayed.ToString("dd/MM/yyyy"));

                    lvMatches.Items.Add(lvi);
                }

            session.Close();
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

        private void btnAddMatch_Click(object sender, EventArgs e)
        {
            MatchesAddForm addDesignerForm = new MatchesAddForm(m_leagueId);

            if (addDesignerForm.ShowDialog() == DialogResult.OK)
            {
                RefreshMatches();
            }
        }

        private void btnEditMatch_Click(object sender, EventArgs e)
        {
            Match selectedMatch = GetSelectedMatch();
            MatchesEditForm editMatchForm = new MatchesEditForm(selectedMatch.Id);

            if (editMatchForm.ShowDialog() == DialogResult.OK)
            {
                RefreshMatches();
            }
        }

        private void btnDeleteMatch_Click(object sender, EventArgs e)
        {
            Match selectedMatch = GetSelectedMatch();

            if (selectedMatch != null && MessageBox.Show("Are you sure you want to delete the selected match?",
                "Delete Match", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                //PROVERI OVO BARTE JER NISAM 100% UPUCEN U MAPIRANJA
                //JA SAM PROVERAVAO I FINO JE BRISAO MECEVE I IZBACIVAO IH ODAKLE TREBA
                //ALI PROVERI GA I TI ZA SVAKI SLUCAJ

                //Added to avoid lazy initialize.
                ISession session = DataAccessLayer.DataAccessLayer.GetSession();
                selectedMatch = session.Get<Match>(selectedMatch.Id);

                selectedMatch.Players.Clear();

                selectedMatch.League.Matches.Remove(selectedMatch);
                session.SaveOrUpdate(selectedMatch.League);

                session.Delete(selectedMatch);
                session.Flush();

                session.Close();

                RefreshMatches();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
