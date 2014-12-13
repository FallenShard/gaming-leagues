using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentNHibernate.Mapping;

using GamingLeagues.Entities;

namespace GamingLeagues.Mappings
{
    public class PlayerMapping : ClassMap<Player>
    {
        public PlayerMapping()
        {
            // Primary key mapping
            Id(x => x.Id);

            // Attribute mapping
            Map(x => x.Name);
            Map(x => x.LastName);
            Map(x => x.NickName);
            Map(x => x.Gender);
            Map(x => x.DateOfBirth);
            Map(x => x.Country);
            Map(x => x.DateTurnedPro);
            Map(x => x.CareerEarnings);
            
            // Many-to-one mapping to current team
            References(x => x.CurrentTeam).Column("CurrentTeamID");

            // Many-to-many mapping to games
            HasManyToMany(x => x.Games)
                .Table("PlaysGames")
                .ParentKeyColumn("PlayerID").ChildKeyColumn("GameID")
                .Cascade.All();

            // Many-to-many mapping to leagues
            HasManyToMany(x => x.Leagues)
                .Table("PlaysInLeague")
                .ParentKeyColumn("PlayerID").ChildKeyColumn("LeagueID")
                .Cascade.All()
                .Inverse();

            // Many-to-many mapping to matches
            HasManyToMany(x => x.MatchesPlayed)
                .Table("PlaysMatch")
                .ParentKeyColumn("PlayerID").ChildKeyColumn("MatchID")
                .Cascade.AllDeleteOrphan()
                .Inverse();
        }
    }
}
