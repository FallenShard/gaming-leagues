using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentNHibernate.Mapping;

using GamingLeagues.Entities;

namespace GamingLeagues.Mappings
{
    public class TeamMapping : ClassMap<Team>
    {
        public TeamMapping()
        {
            // Primary key mapping
            Id(x => x.Id);

            // Attribute mapping
            Map(x => x.Name);
            Map(x => x.DateCreated);
            Map(x => x.DateDisbanded);
            Map(x => x.Country);
            
            // One-to-many mapping
            HasMany(x => x.Players).Inverse().Cascade.All();

            // Many-to-many mapping
            HasManyToMany(x => x.Sponsors)
                .Cascade.All()
                .Table("SponsorsTeam");
        }
    }
}
