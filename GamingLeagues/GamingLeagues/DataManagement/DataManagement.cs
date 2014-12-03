using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NHibernate;
using GamingLeagues.DataAccessLayer;
using GamingLeagues.Entities;

namespace GamingLeagues.DataManagement
{
    class DataManagement
    {
        private ISession m_session;

        #region Inserting

        #region Basic Inserting

        private Player insertPlayerBasic(string name,
                                        string lastName,
                                        string nickName,
                                        char gender,
                                        DateTime dateOfBirth,
                                        string country,
                                        DateTime dateTurnedPro,
                                        float careerEarnings)
        {
            m_session = DataAccessLayer.DataAccessLayer.GetSession();

            Player player = new Player();
            player.Name = name;
            player.LastName = lastName;
            player.NickName = nickName;
            player.Gender = gender;
            player.DateOfBirth = dateOfBirth;
            player.Country = country;
            player.DateTurnedPro = dateTurnedPro;
            player.CareerEarnings = careerEarnings;

            m_session.SaveOrUpdate(player);
            m_session.Flush();

            m_session.Close();

            return player;
        }

        private Team insertTeamBasic(string name,
                                    DateTime dateCreated,
                                    DateTime dateDisbanded,
                                    string country)
        {
            m_session = DataAccessLayer.DataAccessLayer.GetSession();

            Team team = new Team();
            team.Name = name;
            team.DateCreated = dateCreated;
            team.DateDisbanded = dateDisbanded;
            team.Country = country;

            m_session.SaveOrUpdate(team);
            m_session.Flush();

            m_session.Close();

            return team;
        }

        private League insertLeagueBasic(string name,
                                        DateTime startDate,
                                        DateTime endDate,
                                        float budget)
        {
            m_session = DataAccessLayer.DataAccessLayer.GetSession();

            League league = new League();
            league.Name = name;
            league.StartDate = startDate;
            league.EndDate = endDate;
            league.Budget = budget;

            m_session.SaveOrUpdate(league);
            m_session.Flush();

            m_session.Close();

            return league;
        }

        private Sponsor insertSponsorBasic(string name,
                                            string logo)
        {
            m_session = DataAccessLayer.DataAccessLayer.GetSession();

            Sponsor sponsor = new Sponsor();
            sponsor.Name = name;
            sponsor.Logo = logo;

            m_session.SaveOrUpdate(sponsor);
            m_session.Flush();

            m_session.Close();

            return sponsor;
        }

        private Game insertGameBasic(string title,
                                    string developer,
                                    DateTime releaseDate,
                                    string genre)
        {
            m_session = DataAccessLayer.DataAccessLayer.GetSession();

            Game game = new Game();
            game.Title = title;
            game.Developer = developer;
            game.ReleaseDate = releaseDate;
            game.Genre = genre;

            m_session.SaveOrUpdate(game);
            m_session.Flush();

            m_session.Close();

            return game;
        }

        private Platform insertPlatformBasic(string platformTitle)
        {
            m_session = DataAccessLayer.DataAccessLayer.GetSession();

            Platform platform = new Platform();
            platform.PlatformTitle = platformTitle;

            m_session.SaveOrUpdate(platform);
            m_session.Flush();

            m_session.Close();

            return platform;
        }

        #endregion

        //This whole region needs to be reimplemented once all connect methods are implemented,
        //because for now relations are only one sided.
        //
        //Some arguments in some methods make no sense(like match and ranking lists in insertPlayerRelations method,
        //but the code is generically written to show the idea.
        #region Relations Inserting

        private void insertPlayerRelations(Player player,
                                            Team currentTeam,
                                            List<Game> games,
                                            List<PlaysInLeague> rankings,
                                            List<Match> matchesPlayed)
        {
            m_session = DataAccessLayer.DataAccessLayer.GetSession();

            player.CurrentTeam = currentTeam;
            player.Games = games;
            player.Rankings = rankings;
            player.MatchesPlayed = matchesPlayed;

            m_session.SaveOrUpdate(player);
            m_session.Flush();

            m_session.Close();
        }

        private void insertTeamRelations(Team team,
                                        List<Player> players,
                                        List<Sponsor> sponsors)
        {
            m_session = DataAccessLayer.DataAccessLayer.GetSession();

            team.Players = players;
            team.Sponsors = sponsors;

            m_session.SaveOrUpdate(team);
            m_session.Flush();

            m_session.Close();
        }

        private void insertLeagueRelations(League league,
                                            Game game,
                                            List<Sponsor> sponsors,
                                            List<PlaysInLeague> rankings,
                                            List<Match> matches)
        {
            m_session = DataAccessLayer.DataAccessLayer.GetSession();

            league.Game = game;
            league.Sponsors = sponsors;
            league.Rankings = rankings;
            league.Matches = matches;

            m_session.SaveOrUpdate(league);
            m_session.Flush();

            m_session.Close();
        }

        private void insertSponsorRelations(Sponsor sponsor,
                                            List<Team> teams,
                                            List<League> leagues)
        {
            m_session = DataAccessLayer.DataAccessLayer.GetSession();

            sponsor.Teams = teams;
            sponsor.Leagues = leagues;

            m_session.SaveOrUpdate(sponsor);
            m_session.Flush();

            m_session.Close();
        }

        private void insertGameRelations(Game game,
                                        List<Platform> supportedPlatforms,
                                        List<League> leagues,
                                        List<Player> players)
        {
            m_session = DataAccessLayer.DataAccessLayer.GetSession();

            game.SupportedPlatforms = supportedPlatforms;
            game.Leagues = leagues;
            game.Players = players;

            m_session.SaveOrUpdate(game);
            m_session.Flush();

            m_session.Close();
        }

        private void insertPlatformRelations(Platform platform,
                                            Game videoGame)
        {
            m_session = DataAccessLayer.DataAccessLayer.GetSession();

            platform.VideoGame = videoGame;

            m_session.SaveOrUpdate(platform);
            m_session.Flush();

            m_session.Close();
        }

        #endregion

        #region Complete Insertion

        public void insertPlayer(string name,
                            string lastName,
                            string nickName,
                            char gender,
                            DateTime dateOfBirth,
                            string country,
                            DateTime dateTurnedPro,
                            float careerEarnings,
                            Team currentTeam,
                            List<Game> games,
                            List<PlaysInLeague> rankings,
                            List<Match> matchesPlayed)
        {
            insertPlayerRelations(insertPlayerBasic(name, lastName, nickName, gender, dateOfBirth, country, dateTurnedPro, careerEarnings),
                                    currentTeam,
                                    games,
                                    rankings,
                                    matchesPlayed);
        }

        public void insertTeam(string name,
                                DateTime dateCreated,
                                DateTime dateDisbanded,
                                string country,
                                List<Player> players,
                                List<Sponsor> sponsors)
        {
            insertTeamRelations(insertTeamBasic(name, dateCreated, dateDisbanded, country),
                                players,
                                sponsors);
        }

        public void insertLeague(string name,
                                DateTime startDate,
                                DateTime endDate,
                                float budget,
                                Game game,
                                List<Sponsor> sponsors,
                                List<PlaysInLeague> rankings,
                                List<Match> matches)
        {
            insertLeagueRelations(insertLeagueBasic(name, startDate, endDate, budget),
                                    game,
                                    sponsors,
                                    rankings,
                                    matches);
        }

        public void insertSponsor(string name,
                                    string logo,
                                    List<Team> teams,
                                    List<League> leagues)
        {
            insertSponsorRelations(insertSponsorBasic(name, logo),
                                    teams,
                                    leagues);
        }
         
        public void insertGame(string title,
                                string developer,
                                DateTime releaseDate,
                                string genre,
                                List<Platform> supportedPlatforms,
                                List<League> leagues,
                                List<Player> players)
        {
            insertGameRelations(insertGameBasic(title, developer, releaseDate, genre),
                                supportedPlatforms,
                                leagues,
                                players);
        }

        public void insertPlatform(string platformTitle,
                                    Game videoGame)
        {
            insertPlatformRelations(insertPlatformBasic(platformTitle),
                                    videoGame);
        }

        public void insertMatch(DateTime datePlayed,
                                Player homePlayer,
                                Player awayPlayer,
                                int homeScore,
                                int awayScore,
                                League league)
        {
            m_session = DataAccessLayer.DataAccessLayer.GetSession();

            Match match = new Match();
            match.DatePlayed = datePlayed;
            match.HomePlayer = homePlayer;
            match.AwayPlayer = awayPlayer;
            match.HomeScore = homeScore;
            match.AwayScore = awayScore;
            match.League = league;

            m_session.SaveOrUpdate(match);
            m_session.Flush();

            m_session.Close();
        }

        public void insertPlaysInLeague(Player player,
                                        League league,
                                        int points)
        {
            m_session = DataAccessLayer.DataAccessLayer.GetSession();

            PlaysInLeague playsInLeague = new PlaysInLeague();
            playsInLeague.Player = player;
            playsInLeague.League = league;
            playsInLeague.Points = points;

            m_session.SaveOrUpdate(playsInLeague);
            m_session.Flush();

            m_session.Close();
        }

        #endregion

        #endregion

        #region Connecting

        //An example of what the connecting methods for entity should look like.
        #region Connecting Player

        public void connect(Player player, Team team)
        {
            m_session = DataAccessLayer.DataAccessLayer.GetSession();

            player.CurrentTeam = team;
            team.Players.Add(player);

            m_session.SaveOrUpdate(player);
            m_session.Flush();

            m_session.SaveOrUpdate(team);
            m_session.Flush();

            m_session.Close();
        }

        public void connect(Player player, List<Game> games)
        {
            m_session = DataAccessLayer.DataAccessLayer.GetSession();

            foreach (Game g in games)
            {
                g.Players.Add(player);
                player.Games.Add(g);

                m_session.SaveOrUpdate(g);
                m_session.Flush();
            }

            m_session.SaveOrUpdate(player);
            m_session.Flush();

            m_session.Close();
        }

        //This method actually makes no sense.
        public void connect(Player player, List<PlaysInLeague> rankings)
        {
            m_session = DataAccessLayer.DataAccessLayer.GetSession();

            foreach (PlaysInLeague r in rankings)
            {
                r.Player = player;
                player.Rankings.Add(r);

                m_session.SaveOrUpdate(r);
                m_session.Flush();
            }

            m_session.SaveOrUpdate(player);
            m_session.Flush();

            m_session.Close();
        }

        //This method actually makes no sense, so the player will always be home player.
        public void connect(Player player, List<Match> matchesPlayed)
        {
            m_session = DataAccessLayer.DataAccessLayer.GetSession();

            foreach (Match m in matchesPlayed)
            {
                m.HomePlayer = player;
                player.MatchesPlayed.Add(m);

                m_session.SaveOrUpdate(m);
                m_session.Flush();
            }

            m_session.SaveOrUpdate(player);
            m_session.Flush();

            m_session.Close();
        }

        #endregion

        #endregion
    }
}