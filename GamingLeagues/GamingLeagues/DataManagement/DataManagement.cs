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

        //Inserting Player with Leagues, and vice versa creates new PlaysInLeague entity with 0 points.
        //
        //Old arguments are commented to show the changes from last version
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
                                    string tag,
                                    string country)
        {
            m_session = DataAccessLayer.DataAccessLayer.GetSession();

            Team team = new Team();
            team.Name = name;
            team.DateCreated = dateCreated;
            team.Tag = tag;
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

        #region Relations Inserting

        private void insertPlayerRelations(Player player,
                                            Team currentTeam,
                                            List<Game> games,
                                            /*List<PlaysInLeague> rankings,
                                            List<Match> matchesPlayed*/
                                            List<League> leagues)
        {
            connectPlayerTeam(player, currentTeam);

            foreach (Game game in games)
                connectPlayerGame(player, game);

            foreach (League league in leagues)
                insertPlaysInLeague(player, league);
        }

        private void insertTeamRelations(Team team,
                                        List<Player> players,
                                        List<Sponsor> sponsors)
        {
            foreach (Player player in players)
                connectPlayerTeam(player, team);

            foreach (Sponsor sponsor in sponsors)
                connectTeamSponsor(team, sponsor);
        }

        private void insertLeagueRelations(League league,
                                            Game game,
                                            List<Sponsor> sponsors,
                                            /*List<PlaysInLeague> rankings,
                                            List<Match> matches*/
                                            List<Player> players)
        {
            connectLeagueGame(league, game);

            foreach (Sponsor sponsor in sponsors)
                connectLeagueSponsor(league, sponsor);

            foreach (Player player in players)
                insertPlaysInLeague(player, league);
        }

        private void insertSponsorRelations(Sponsor sponsor,
                                            List<Team> teams,
                                            List<League> leagues)
        {
            foreach (Team team in teams)
                connectTeamSponsor(team, sponsor);

            foreach (League league in leagues)
                connectLeagueSponsor(league, sponsor);
        }

        private void insertGameRelations(Game game,
                                        List<Platform> supportedPlatforms,
                                        List<League> leagues,
                                        List<Player> players)
        {
            foreach (Platform platform in supportedPlatforms)
                connectGamePlatform(game, platform);

            foreach (League league in leagues)
                connectLeagueGame(league, game);

            foreach (Player player in players)
                connectPlayerGame(player, game);
        }

        private void insertPlatformRelations(Platform platform,
                                            Game videoGame)
        {
            connectGamePlatform(videoGame, platform);
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
                            /*List<PlaysInLeague> rankings,
                            List<Match> matchesPlayed,*/
                            List<League> leagues)
        {
            insertPlayerRelations(insertPlayerBasic(name, lastName, nickName, gender, dateOfBirth, country, dateTurnedPro, careerEarnings),
                                    currentTeam,
                                    games,
                                    /*rankings,
                                    matchesPlayed*/
                                    leagues);
        }

        public void insertTeam(string name,
                                DateTime dateCreated,
                                string tag,
                                string country,
                                List<Player> players,
                                List<Sponsor> sponsors)
        {
            insertTeamRelations(insertTeamBasic(name, dateCreated, tag, country),
                                players,
                                sponsors);
        }

        public void insertLeague(string name,
                                DateTime startDate,
                                DateTime endDate,
                                float budget,
                                Game game,
                                List<Sponsor> sponsors,
                                /*List<PlaysInLeague> rankings,
                                List<Match> matches*/
                                List<Player> players)
        {
            insertLeagueRelations(insertLeagueBasic(name, startDate, endDate, budget),
                                    game,
                                    sponsors,
                                    /*rankings,
                                    matches*/
                                    players);
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
            //match.HomePlayer = homePlayer;
            //match.AwayPlayer = awayPlayer;
            match.HomeScore = homeScore;
            match.AwayScore = awayScore;
            //match.League = league;

            m_session.SaveOrUpdate(match);
            m_session.Flush();

            m_session.Close();

            connectHomePlayerMatch(homePlayer, match);
            connectAwayPlayerMatch(awayPlayer, match);
            connectLeagueMatch(league, match);
        }

        //Starting points should be default 0
        public void insertPlaysInLeague(Player player,
                                        League league/*,
                                        int points*/)
        {
            m_session = DataAccessLayer.DataAccessLayer.GetSession();

            PlaysInLeague ranking = new PlaysInLeague();
            //playsInLeague.Player = player;
            //playsInLeague.League = league;
            //playsInLeague.Points = points;

            m_session.SaveOrUpdate(ranking);
            m_session.Flush();

            m_session.Close();

            connectPlayerRanking(player, ranking);
            connectLeagueRanking(league, ranking);
        }

        #endregion

        #endregion

        #region Connecting

        public void connectPlayerTeam(Player player, Team team)
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

        public void connectPlayerGame(Player player, Game game)
        {
            m_session = DataAccessLayer.DataAccessLayer.GetSession();

            player.Games.Add(game);
            game.Players.Add(player);

            m_session.SaveOrUpdate(player);
            m_session.Flush();

            m_session.SaveOrUpdate(game);
            m_session.Flush();

            m_session.Close();
        }

        public void connectPlayerRanking(Player player, PlaysInLeague ranking)
        {
            m_session = DataAccessLayer.DataAccessLayer.GetSession();

            player.Rankings.Add(ranking);
            ranking.Player = player;

            m_session.SaveOrUpdate(player);
            m_session.Flush();

            m_session.SaveOrUpdate(ranking);
            m_session.Flush();

            m_session.Close();
        }

        public void connectHomePlayerMatch(Player player, Match match)
        {
            m_session = DataAccessLayer.DataAccessLayer.GetSession();

            player.MatchesPlayed.Add(match);
            match.HomePlayer = player;

            m_session.SaveOrUpdate(player);
            m_session.Flush();

            m_session.SaveOrUpdate(match);
            m_session.Flush();

            m_session.Close();
        }

        public void connectAwayPlayerMatch(Player player, Match match)
        {
            m_session = DataAccessLayer.DataAccessLayer.GetSession();

            player.MatchesPlayed.Add(match);
            match.AwayPlayer = player;

            m_session.SaveOrUpdate(player);
            m_session.Flush();

            m_session.SaveOrUpdate(match);
            m_session.Flush();

            m_session.Close();
        }

        public void connectTeamSponsor(Team team, Sponsor sponsor)
        {
            m_session = DataAccessLayer.DataAccessLayer.GetSession();

            team.Sponsors.Add(sponsor);
            sponsor.Teams.Add(team);

            m_session.SaveOrUpdate(team);
            m_session.Flush();

            m_session.SaveOrUpdate(sponsor);
            m_session.Flush();

            m_session.Close();
        }

        public void connectLeagueGame(League league, Game game)
        {
            m_session = DataAccessLayer.DataAccessLayer.GetSession();

            league.Game = game;
            game.Leagues.Add(league);

            m_session.SaveOrUpdate(league);
            m_session.Flush();

            m_session.SaveOrUpdate(game);
            m_session.Flush();

            m_session.Close();
        }

        public void connectLeagueSponsor(League league, Sponsor sponsor)
        {
            m_session = DataAccessLayer.DataAccessLayer.GetSession();

            league.Sponsors.Add(sponsor);
            sponsor.Leagues.Add(league);

            m_session.SaveOrUpdate(league);
            m_session.Flush();

            m_session.SaveOrUpdate(sponsor);
            m_session.Flush();

            m_session.Close();
        }

        public void connectLeagueRanking(League league, PlaysInLeague ranking)
        {
            m_session = DataAccessLayer.DataAccessLayer.GetSession();

            league.Rankings.Add(ranking);
            ranking.League = league;

            m_session.SaveOrUpdate(league);
            m_session.Flush();

            m_session.SaveOrUpdate(ranking);
            m_session.Flush();

            m_session.Close();
        }

        public void connectLeagueMatch(League league, Match match)
        {
            m_session = DataAccessLayer.DataAccessLayer.GetSession();

            league.Matches.Add(match);
            match.League = league;

            m_session.SaveOrUpdate(league);
            m_session.Flush();

            m_session.SaveOrUpdate(match);
            m_session.Flush();

            m_session.Close();
        }

        public void connectGamePlatform(Game game, Platform platform)
        {
            m_session = DataAccessLayer.DataAccessLayer.GetSession();

            game.SupportedPlatforms.Add(platform);
            platform.VideoGame = game;

            m_session.SaveOrUpdate(game);
            m_session.Flush();

            m_session.SaveOrUpdate(platform);
            m_session.Flush();

            m_session.Close();
        }

        #endregion

        public void initializeDataBase()
        {
            //insertGame("Warcraft III", "Blizzard", new DateTime(2002, 7, 1), "Real-time strategy", null, null, null);
            //insertGame("StarCraft II", "Blizzard", new DateTime(2010, 7, 27), "Real-time strategy", null, null, null);
            //insertGame("Quake III Arena", "id Software", new DateTime(1999, 12, 2), "First-person shooter", null, null, null);
            //insertGame("PES 2015", "PES Productions", new DateTime(2014, 11, 11), "Sport", null, null, null);
            //insertGame("NBA 2K15", "Visual Concepts", new DateTime(2014, 10, 7), "Sport", null, null, null);

            insertGameBasic("Warcraft III", "Blizzard", new DateTime(2002, 7, 1), "Real-time strategy");
            insertGameBasic("StarCraft II", "Blizzard", new DateTime(2010, 7, 27), "Real-time strategy");
            insertGameBasic("Quake III Arena", "id Software", new DateTime(1999, 12, 2), "First-person shooter");
            insertGameBasic("PES 2015", "PES Productions", new DateTime(2014, 11, 11), "Sport");
            insertGameBasic("NBA 2K15", "Visual Concepts", new DateTime(2014, 10, 7), "Sport");
        }
    }
}