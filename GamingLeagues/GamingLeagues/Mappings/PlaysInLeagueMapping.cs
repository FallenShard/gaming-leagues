using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentNHibernate.Mapping;

using GamingLeagues.Entities;

namespace GamingLeagues.Mappings
{
    class PlaysInLeagueMapping : ClassMap<PlaysInLeague>
    {
        public PlaysInLeagueMapping()
        {
            // Key & Many-to-one mapping
            CompositeId().KeyReference(x => x.Player)
                         .KeyReference(x => x.League);

            // Attribute mapping
            Map(x => x.Points);
        }
    }
}
