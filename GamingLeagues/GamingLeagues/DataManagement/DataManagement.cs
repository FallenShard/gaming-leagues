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
    public class DataManagement
    {
        private ISession m_session;

        public DataManagement()
        {
            m_session = DataAccessLayer.DataAccessLayer.GetSession();
        }

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

            return player;
        }

        private Team insertTeamBasic(string name,
                                    DateTime dateCreated,
                                    string tag,
                                    string country)
        {
            Team team = new Team();
            team.Name = name;
            team.DateCreated = dateCreated;
            team.Tag = tag;
            team.Country = country;

            m_session.SaveOrUpdate(team);
            m_session.Flush();

            return team;
        }

        private League insertLeagueBasic(string name,
                                        DateTime startDate,
                                        DateTime endDate,
                                        float budget)
        {
            League league = new League();
            league.Name = name;
            league.StartDate = startDate;
            league.EndDate = endDate;
            league.Budget = budget;

            m_session.SaveOrUpdate(league);
            m_session.Flush();

            return league;
        }

        private Sponsor insertSponsorBasic(string name,
                                            string slogan)
        {
            Sponsor sponsor = new Sponsor();
            sponsor.Name = name;
            sponsor.Slogan = slogan;

            m_session.SaveOrUpdate(sponsor);
            m_session.Flush();

            return sponsor;
        }

        private Game insertGameBasic(string title,
                                    string developer,
                                    DateTime releaseDate,
                                    string genre)
        {
            Game game = new Game();
            game.Title = title;
            game.Developer = developer;
            game.ReleaseDate = releaseDate;
            game.Genre = genre;

            m_session.SaveOrUpdate(game);
            m_session.Flush();

            return game;
        }

        private Platform insertPlatformBasic(string platformTitle)
        {
            Platform platform = new Platform();
            platform.PlatformTitle = platformTitle;

            m_session.SaveOrUpdate(platform);
            m_session.Flush();

            return platform;
        }

        #endregion

        #region Relations Inserting

        private void insertPlayerRelations(Player player,
                                            Team currentTeam,
                                            List<Game> games,
                                            List<League> leagues)
        {
            if (currentTeam != null)
                connectPlayerTeam(player, currentTeam);

            foreach (Game game in games)
                connectPlayerGame(player, game);

            //foreach (League league in leagues)
            //    insertPlaysInLeague(player, league, 0);
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
                                            List<Player> players)
        {
            if (game != null)
                connectLeagueGame(league, game);

            foreach (Sponsor sponsor in sponsors)
                connectLeagueSponsor(league, sponsor);

            //foreach (Player player in players)
            //    insertPlaysInLeague(player, league, 0);
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
            if (videoGame != null)
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
                            List<League> leagues)
        {
            insertPlayerRelations(insertPlayerBasic(name, lastName, nickName, gender, dateOfBirth, country, dateTurnedPro, careerEarnings),
                                    currentTeam,
                                    games,
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
                                List<Player> players)
        {
            insertLeagueRelations(insertLeagueBasic(name, startDate, endDate, budget),
                                    game,
                                    sponsors,
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

        //public void insertMatch(DateTime datePlayed,
        //                        Player homePlayer,
        //                        Player awayPlayer,
        //                        int homeScore,
        //                        int awayScore,
        //                        League league)
        //{
        //    Match match = new Match();
        //    match.DatePlayed = datePlayed;
        //    match.HomeScore = homeScore;
        //    match.AwayScore = awayScore;

        //    m_session.SaveOrUpdate(match);
        //    m_session.Flush();

        //    connectHomePlayerMatch(homePlayer, match);
        //    connectAwayPlayerMatch(awayPlayer, match);
        //    connectLeagueMatch(league, match);
        //}

        //public void insertPlaysInLeague(Player player,
        //                                League league,
        //                                int points)
        //{
        //    PlaysInLeague ranking = new PlaysInLeague();
        //    ranking.Player = player;
        //    ranking.League = league;
        //    ranking.Points = points;

        //    m_session.SaveOrUpdate(ranking);
        //    m_session.Flush();

        //    connectPlayerRanking(player, ranking);
        //    connectLeagueRanking(league, ranking);
        //}

        #endregion

        #endregion

        #region Relations

        #region Connecting

        public void connectPlayerTeam(Player player, Team team)
        {
            player.CurrentTeam = team;
            team.Players.Add(player);

            m_session.SaveOrUpdate(player);
            m_session.Flush();

            m_session.SaveOrUpdate(team);
            m_session.Flush();
        }

        public void connectPlayerGame(Player player, Game game)
        {
            player.Games.Add(game);
            game.Players.Add(player);

            m_session.SaveOrUpdate(player);
            m_session.Flush();

            m_session.SaveOrUpdate(game);
            m_session.Flush();
        }

        //public void connectPlayerRanking(Player player, PlaysInLeague ranking)
        //{
        //    player.Rankings.Add(ranking);
        //    ranking.Player = player;

        //    m_session.SaveOrUpdate(player);
        //    m_session.Flush();

        //    m_session.SaveOrUpdate(ranking);
        //    m_session.Flush();
        //}

        //public void connectHomePlayerMatch(Player player, Match match)
        //{
        //    player.MatchesPlayed.Add(match);
        //    match.HomePlayer = player;

        //    m_session.SaveOrUpdate(player);
        //    m_session.Flush();

        //    m_session.SaveOrUpdate(match);
        //    m_session.Flush();
        //}

        //public void connectAwayPlayerMatch(Player player, Match match)
        //{
        //    player.MatchesPlayed.Add(match);
        //    match.AwayPlayer = player;

        //    m_session.SaveOrUpdate(player);
        //    m_session.Flush();

        //    m_session.SaveOrUpdate(match);
        //    m_session.Flush();
        //}

        public void connectTeamSponsor(Team team, Sponsor sponsor)
        {
            team.Sponsors.Add(sponsor);
            sponsor.Teams.Add(team);

            m_session.SaveOrUpdate(team);
            m_session.Flush();

            m_session.SaveOrUpdate(sponsor);
            m_session.Flush();
        }

        public void connectLeagueGame(League league, Game game)
        {
            league.Game = game;
            game.Leagues.Add(league);

            m_session.SaveOrUpdate(league);
            m_session.Flush();

            m_session.SaveOrUpdate(game);
            m_session.Flush();
        }

        public void connectLeagueSponsor(League league, Sponsor sponsor)
        {
            league.Sponsors.Add(sponsor);
            sponsor.Leagues.Add(league);

            m_session.SaveOrUpdate(league);
            m_session.Flush();

            m_session.SaveOrUpdate(sponsor);
            m_session.Flush();
        }

        public void connectLeagueRanking(League league, Player player)
        {
            league.Players.Add(player);
            player.Leagues.Add(league);

            m_session.SaveOrUpdate(league);
            m_session.Flush();

            m_session.SaveOrUpdate(player);
            m_session.Flush();
        }

        public void connectLeagueMatch(League league, Match match)
        {
            league.Matches.Add(match);
            match.League = league;

            m_session.SaveOrUpdate(league);
            m_session.Flush();

            m_session.SaveOrUpdate(match);
            m_session.Flush();
        }

        public void connectGamePlatform(Game game, Platform platform)
        {
            game.SupportedPlatforms.Add(platform);
            platform.VideoGame = game;

            m_session.SaveOrUpdate(game);
            m_session.Flush();

            m_session.SaveOrUpdate(platform);
            m_session.Flush();
        }

        #endregion

        #region Disconnecting

        public void disconnectPlayerTeam(Player player, Team team)
        {
            player.CurrentTeam = null;
            team.Players.Remove(player);

            m_session.SaveOrUpdate(player);
            m_session.Flush();

            m_session.SaveOrUpdate(team);
            m_session.Flush();
        }

        public void disconnectPlayerGame(Player player, Game game)
        {
            player.Games.Remove(game);
            game.Players.Remove(player);

            m_session.SaveOrUpdate(player);
            m_session.Flush();

            m_session.SaveOrUpdate(game);
            m_session.Flush();
        }

        //public void disconnectPlayerRanking(Player player, PlaysInLeague ranking)
        //{
        //    player.Rankings.Remove(ranking);
        //    ranking.Player = null;

        //    m_session.SaveOrUpdate(player);
        //    m_session.Flush();

        //    m_session.SaveOrUpdate(ranking);
        //    m_session.Flush();
        //}

        //public void disconnectHomePlayerMatch(Player player, Match match)
        //{
        //    player.MatchesPlayed.Remove(match);
        //    match.HomePlayer = null;

        //    m_session.SaveOrUpdate(player);
        //    m_session.Flush();

        //    m_session.SaveOrUpdate(match);
        //    m_session.Flush();
        //}

        //public void disconnectAwayPlayerMatch(Player player, Match match)
        //{
        //    player.MatchesPlayed.Remove(match);
        //    match.AwayPlayer = null;

        //    m_session.SaveOrUpdate(player);
        //    m_session.Flush();

        //    m_session.SaveOrUpdate(match);
        //    m_session.Flush();
        //}

        public void disconnectTeamSponsor(Team team, Sponsor sponsor)
        {
            team.Sponsors.Remove(sponsor);
            sponsor.Teams.Remove(team);

            m_session.SaveOrUpdate(team);
            m_session.Flush();

            m_session.SaveOrUpdate(sponsor);
            m_session.Flush();
        }

        public void disconnectLeagueGame(League league, Game game)
        {
            league.Game = null;
            if (game != null)
                game.Leagues.Remove(league);

            m_session.SaveOrUpdate(league);
            m_session.Flush();

            m_session.SaveOrUpdate(game);
            m_session.Flush();
        }

        public void disconnectLeagueSponsor(League league, Sponsor sponsor)
        {
            league.Sponsors.Remove(sponsor);
            sponsor.Leagues.Remove(league);

            m_session.SaveOrUpdate(league);
            m_session.Flush();

            m_session.SaveOrUpdate(sponsor);
            m_session.Flush();
        }

        //public void disconnectLeagueRanking(League league, Player player)
        //{
        //    league.Players.Remove(ranking);
        //    ranking.League = null;

        //    m_session.SaveOrUpdate(league);
        //    m_session.Flush();

        //    m_session.SaveOrUpdate(ranking);
        //    m_session.Flush();
        //}

        public void disconnectLeagueMatch(League league, Match match)
        {
            league.Matches.Remove(match);
            match.League = null;

            m_session.SaveOrUpdate(league);
            m_session.Flush();

            m_session.SaveOrUpdate(match);
            m_session.Flush();
        }

        public void disconnectGamePlatform(Game game, Platform platform)
        {
            game.SupportedPlatforms.Remove(platform);
            platform.VideoGame = null;

            m_session.SaveOrUpdate(game);
            m_session.Flush();

            m_session.SaveOrUpdate(platform);
            m_session.Flush();
        }

        #endregion

        #endregion

        #region Reading

        public IList<Player> getPlayers()
        {
            return m_session.CreateQuery("FROM Player").List<Player>();
        }

        public IList<Team> getTeams()
        {
            return m_session.CreateQuery("FROM Team").List<Team>();
        }

        public IList<League> getLeagues()
        {
            return m_session.CreateQuery("FROM League").List<League>();
        }

        public IList<Sponsor> getSponsors()
        {
            return m_session.CreateQuery("FROM Sponsor").List<Sponsor>();
        }

        public IList<Game> getGames()
        {
            return m_session.CreateQuery("FROM Game").List<Game>();
        }

        public IList<Platform> getPlatforms()
        {
            return m_session.CreateQuery("FROM Platform").List<Platform>();
        }

        #endregion

        #region Updating

        //TODO: Match and Ranking update
        public void updatePlayer(Player player,
                                string name,
                                string lastName,
                                string nickName,
                                char gender,
                                DateTime dateOfBirth,
                                string country,
                                DateTime dateTurnedPro,
                                float careerEarnings,
                                Team currentTeam,
                                List<Game> games)
        {
            player.Name = name;
            player.LastName = lastName;
            player.NickName = nickName;
            player.Gender = gender;
            player.DateOfBirth = dateOfBirth;
            player.Country = country;
            player.DateTurnedPro = dateTurnedPro;
            player.CareerEarnings = careerEarnings;

            disconnectPlayerTeam(player, player.CurrentTeam);
            connectPlayerTeam(player, currentTeam);

            //foreach (Game game in player.Games)
            //    disconnectPlayerGame(player, game);
            player.Games.Clear();
            foreach (Game game in games)
                connectPlayerGame(player, game);

            m_session.SaveOrUpdate(player);
            m_session.Flush();
        }

        public void updateTeam(Team team,
                                string name,
                                DateTime dateCreated,
                                string tag,
                                string country,
                                List<Player> players,
                                List<Sponsor> sponsors)
        {
            team.Name = name;
            team.DateCreated = dateCreated;
            team.Tag = tag;
            team.Country = country;

            //foreach (Player player in team.Players)
            //    disconnectPlayerTeam(player, team);
            team.Players.Clear();
            foreach (Player player in players)
                connectPlayerTeam(player, team);

            //foreach (Sponsor sponsor in team.Sponsors)
            //    disconnectTeamSponsor(team, sponsor);
            team.Sponsors.Clear();
            foreach (Sponsor sponsor in sponsors)
                connectTeamSponsor(team, sponsor);

            m_session.SaveOrUpdate(team);
            m_session.Flush();
        }

        //TODO: Match and Ranking update
        public void updateLeague(League league,
                                string name,
                                DateTime startDate,
                                DateTime endDate,
                                float budget,
                                Game game,
                                List<Sponsor> sponsors,
                                List<Player> players)
        {
            league.Name = name;
            league.StartDate = startDate;
            league.EndDate = endDate;
            league.Budget = budget;

            disconnectLeagueGame(league, league.Game);
            connectLeagueGame(league, game);

            //foreach (Sponsor sponsor in league.Sponsors)
            //    disconnectLeagueSponsor(league, sponsor);
            league.Sponsors.Clear();
            foreach (Sponsor sponsor in sponsors)
                connectLeagueSponsor(league, sponsor);

            m_session.SaveOrUpdate(league);
            m_session.Flush();
        }

        public void updateSponsor(Sponsor sponsor,
                                    string name,
                                    string logo,
                                    List<Team> teams,
                                    List<League> leagues)
        {
            sponsor.Name = name;
            sponsor.Slogan = logo;

            //foreach (Team team in sponsor.Teams)
            //    disconnectTeamSponsor(team, sponsor);
            sponsor.Teams.Clear();
            foreach (Team team in teams)
                connectTeamSponsor(team, sponsor);

            //foreach (League league in sponsor.Leagues)
            //    disconnectLeagueSponsor(league, sponsor);
            sponsor.Leagues.Clear();
            foreach (League league in leagues)
                connectLeagueSponsor(league, sponsor);

            m_session.SaveOrUpdate(sponsor);
            m_session.Flush();
        }

        public void updateGame(Game game,
                                string title,
                                string developer,
                                DateTime releaseDate,
                                string genre,
                                List<Platform> supportedPlatforms,
                                List<League> leagues,
                                List<Player> players)
        {
            game.Title = title;
            game.Developer = developer;
            game.ReleaseDate = releaseDate;
            game.Genre = genre;

            //foreach (Platform platform in game.SupportedPlatforms)
            //    disconnectGamePlatform(game, platform);
            game.SupportedPlatforms.Clear();
            foreach (Platform platform in supportedPlatforms)
                connectGamePlatform(game, platform);

            //foreach (League league in game.Leagues)
            //    disconnectLeagueGame(league, game);
            game.Leagues.Clear();
            foreach (League league in leagues)
                connectLeagueGame(league, game);

            //foreach (Player player in game.Players)
            //    disconnectPlayerGame(player, game);
            game.Players.Clear();
            foreach (Player player in players)
                connectPlayerGame(player, game);

            m_session.SaveOrUpdate(game);
            m_session.Flush();
        }

        public void updatePlatform(Platform platform,
                                    string platformTitle,
                                    Game videoGame)
        {
            platform.PlatformTitle = platformTitle;

            disconnectGamePlatform(platform.VideoGame, platform);
            connectGamePlatform(videoGame, platform);

            m_session.SaveOrUpdate(platform);
            m_session.Flush();
        }

        //public void updateMatch(Match match,
        //                        DateTime datePlayed,
        //                        Player homePlayer,
        //                        Player awayPlayer,
        //                        int homeScore,
        //                        int awayScore,
        //                        League league)
        //{
        //    match.DatePlayed = datePlayed;
        //    match.HomeScore = homeScore;
        //    match.AwayScore = awayScore;

        //    disconnectHomePlayerMatch(match.HomePlayer, match);
        //    connectHomePlayerMatch(homePlayer, match);

        //    disconnectAwayPlayerMatch(match.AwayPlayer, match);
        //    connectAwayPlayerMatch(awayPlayer, match);

        //    disconnectLeagueMatch(match.League, match);
        //    connectLeagueMatch(league, match);

        //    m_session.SaveOrUpdate(match);
        //    m_session.Flush();
        //}

        //public void insertPlaysInLeague(PlaysInLeague ranking,
        //                                Player player,
        //                                League league,
        //                                int points)
        //{
        //    ranking.Points = points;
            
        //    disconnectPlayerRanking(ranking.Player, ranking);
        //    connectPlayerRanking(player, ranking);

        //    disconnectLeagueRanking(ranking.League, ranking);
        //    connectLeagueRanking(league, ranking);

        //    m_session.SaveOrUpdate(ranking);
        //    m_session.Flush();
        //}

        #endregion

        #region Deleting

        public void deletePlayer(Player player)
        {
            disconnectPlayerTeam(player, player.CurrentTeam);

            //foreach (Game game in player.Games)
            //    connectPlayerGame(player, game);

            //foreach (Match match in player.MatchesPlayed)
            //    if (match.HomePlayer == player)
            //        disconnectHomePlayerMatch(player, match);
            //    else
            //        disconnectAwayPlayerMatch(player, match);

            //foreach (PlaysInLeague ranking in player.Rankings)
            //    disconnectPlayerRanking(player, ranking);

            player.Games.Clear();
            player.MatchesPlayed.Clear();
            player.Leagues.Clear();

            m_session.Delete(player);
            m_session.Flush();
        }

        public void deleteTeam(Team team)
        {
            //foreach (Player player in team.Players)
            //    disconnectPlayerTeam(player, team);

            //foreach (Sponsor sponsor in team.Sponsors)
            //    disconnectTeamSponsor(team, sponsor);

            team.Players.Clear();
            team.Sponsors.Clear();

            m_session.Delete(team);
            m_session.Flush();
        }

        public void deleteLeague(League league)
        {
            disconnectLeagueGame(league, league.Game);

            //foreach (Sponsor sponsor in league.Sponsors)
            //    disconnectLeagueSponsor(league, sponsor);

            //foreach (Match match in league.Matches)
            //    disconnectLeagueMatch(league, match);

            //foreach (PlaysInLeague ranking in league.Rankings)
            //    disconnectLeagueRanking(league, ranking);

            league.Sponsors.Clear();
            league.Matches.Clear();
            league.Players.Clear();

            m_session.Delete(league);
            m_session.Flush();
        }

        public void deleteSponsor(Sponsor sponsor)
        {
            //foreach (Team team in sponsor.Teams)
            //    disconnectTeamSponsor(team, sponsor);

            //foreach (League league in sponsor.Leagues)
            //    connectLeagueSponsor(league, sponsor);

            sponsor.Teams.Clear();
            sponsor.Leagues.Clear();

            m_session.Delete(sponsor);
            m_session.Flush();
        }

        public void deleteGame(Game game)
        {
            //foreach (Platform platform in game.SupportedPlatforms)
            //    disconnectGamePlatform(game, platform);

            //foreach (League league in game.Leagues)
            //    disconnectLeagueGame(league, game);

            //foreach (Player player in game.Players)
            //    disconnectPlayerGame(player, game);

            List<Player> currPlayers = new List<Player>(game.Players);

            foreach (Player player in currPlayers)
                disconnectPlayerGame(player, game);

            game.SupportedPlatforms.Clear();
            game.Leagues.Clear();
            game.Players.Clear();

            m_session.Delete(game);
            m_session.Flush();
        }

        public void deletePlatform(Platform platform)
        {
            disconnectGamePlatform(platform.VideoGame, platform);

            m_session.Delete(platform);
            m_session.Flush();
        }

        //public void deleteMatch(Match match)
        //{
        //    disconnectHomePlayerMatch(match.HomePlayer, match);
        //    disconnectAwayPlayerMatch(match.AwayPlayer, match);
        //    disconnectLeagueMatch(match.League, match);

        //    m_session.Delete(match);
        //    m_session.Flush();
        //}

        //public void deletePlaysInLeague(PlaysInLeague ranking)
        //{
        //    disconnectPlayerRanking(ranking.Player, ranking);
        //    //disconnectLeagueRanking(ranking.League, ranking);

        //    m_session.Delete(ranking);
        //    m_session.Flush();
        //}

        #endregion

        public void initializeDataBase()
        {
            insertGameBasic("Warcraft III", "Blizzard", new DateTime(2002, 7, 1), "Real-time strategy");
            insertGameBasic("StarCraft II", "Blizzard", new DateTime(2010, 7, 27), "Real-time strategy");
            insertGameBasic("Quake III Arena", "id Software", new DateTime(1999, 12, 2), "First-person shooter");
            insertGameBasic("PES 2015", "PES Productions", new DateTime(2014, 11, 11), "Sport");
            insertGameBasic("NBA 2K15", "Visual Concepts", new DateTime(2014, 10, 7), "Sport");
            insertGameBasic("Dota 2", "Valve", new DateTime(2011, 4, 7), "MOBA");

            insertTeamBasic("Natus Vincere", new DateTime(2009, 12, 1), "NaVi'", "Ukraine");
            insertTeamBasic("Evil Geniuses", new DateTime(1999, 05, 12), "EG.", "United States");
            insertTeamBasic("Team Liquid", new DateTime(2000, 10, 3), "Liquid'", "Netherlands");
            insertTeamBasic("KT Rolster", new DateTime(1999, 03, 17), "KT.", "South Korea");

            insertPlayerBasic("Ilyes", "Satouri", "Stephano", 'M', new DateTime(1993, 03, 12), "France", new DateTime(2008, 06, 23), 248631.39f);
            insertPlayerBasic("Daniel", "Ishutin", "Dendi", 'M', new DateTime(1989, 12, 30), "Ukraine", new DateTime(2005, 02, 13), 569495.84f);
            insertPlayerBasic("Jang", "Min Chul", "MC", 'M', new DateTime(1991, 06, 17), "South Korea", new DateTime(2007, 12, 3), 460500);


            insertSponsorBasic("Steelseries", "Winning never felt this good.");
            insertSponsorBasic("Nvidia", "The way it's meant to be played");
            insertSponsorBasic("AMD", "Gaming evolved");
            insertSponsorBasic("Razer", "By gamers. For gamers.");

            insertLeagueBasic("StarLeague", new DateTime(2013, 04, 12), new DateTime(2013, 06, 12), 150000);
            insertLeagueBasic("DreamLeague", new DateTime(2013, 08, 5), new DateTime(2014, 2, 5), 250000);
            insertLeagueBasic("Republic of Games", new DateTime(2014, 10, 12), new DateTime(2013, 11, 12), 50000);
            insertLeagueBasic("Ladder Series", new DateTime(2012, 7, 22), new DateTime(2013, 8, 23), 500000);
        }
    }
}