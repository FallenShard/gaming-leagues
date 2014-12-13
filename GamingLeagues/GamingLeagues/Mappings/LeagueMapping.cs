using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentNHibernate.Mapping;

using GamingLeagues.Entities;

namespace GamingLeagues.Mappings
{
    class LeagueMapping : ClassMap<League>
    {
        public LeagueMapping()
        {
            // Primary key mapping
            Id(x => x.Id);

            // Attribute mapping
            Map(x => x.Name);
            Map(x => x.StartDate);
            Map(x => x.EndDate);
            Map(x => x.Budget);

            // Many-to-one mapping to games
            References(x => x.Game)
                .Column("GameID");

            // Many-to-many mapping to sponsors
            HasManyToMany(x => x.Sponsors)
                .Table("SponsorsLeague")
                .ParentKeyColumn("LeagueID").ChildKeyColumn("SponsorID")
                .Cascade.All()
                .Inverse();

            // Many-to-many mapping to players
            HasManyToMany(x => x.Players)
                .Table("PlaysInLeague")
                .ParentKeyColumn("LeagueID").ChildKeyColumn("PlayerID")
                .Cascade.All();

            // One-to-many mapping to matches
            HasMany(x => x.Matches)
                .KeyColumn("LeagueID")
                .Cascade.All()
                .Inverse();
        }
    }
}