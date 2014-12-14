using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentNHibernate.Mapping;

using GamingLeagues.Entities;

namespace GamingLeagues.Mappings
{
    class MatchMapping : ClassMap<Match>
    {
        public MatchMapping()
        {
            // Primary key mapping
            Id(x => x.Id);

            // Attribute mapping
            Map(x => x.DatePlayed);
            Map(x => x.HomeScore);
            Map(x => x.AwayScore);
            
            // Many-to-one mapping to leagues
            References(x => x.League)
                .Column("LeagueID");

            // Many-to-many mapping to players
            HasManyToMany(x => x.Players)
                .Table("PlaysMatch")
                .ParentKeyColumn("MatchID").ChildKeyColumn("PlayerID")
                .Cascade.SaveUpdate();
        }
    }
}
