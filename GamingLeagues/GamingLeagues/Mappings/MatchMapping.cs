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
            
            // Many-to-one mapping
            References(x => x.HomePlayer);
            References(x => x.AwayPlayer);
            References(x => x.League);
        }
    }
}
