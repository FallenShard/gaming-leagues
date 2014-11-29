using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentNHibernate.Mapping;

using GamingLeagues.Entities;

namespace GamingLeagues.Mappings
{
    public class SponsorMapping : ClassMap<Sponsor>
    {
        public SponsorMapping()
        {
            // Primary key mapping
            Id(x => x.Id);

            // Attribute mapping
            Map(x => x.Name);
            Map(x => x.Logo);
            
            // Many-to-many mapping
            HasManyToMany(x => x.Teams)
                .Cascade.All()
                .Table("SponsorsTeam");

            // Many-to-many mapping
            // HasManyToMany(x => x.Leagues)
            //    .Cascade.All()
            //    .Table("SponsorsLeague");
        }
    }
}
