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

            // Many-to-one mapping
            References(x => x.Game);

            // Many-to-many mapping
            HasManyToMany(x => x.Sponsors)
                .Cascade.All()
                .Table("SponsorsLeague");

            // Many-to-many mapping
            //HasManyToMany(x => x.Players)
            //    .Cascade.All()
            //    .Table("CompetesIn");

            // One-to-many
            HasMany(x => x.Matches).Inverse().Cascade.All();
        }
    }
}