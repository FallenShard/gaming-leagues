using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentNHibernate.Mapping;

using GamingLeagues.Entities;

namespace GamingLeagues.Mappings
{
    public class PlatformMapping : ClassMap<Platform>
    {
        public PlatformMapping()
        {
            CompositeId().KeyReference(x => x.VideoGame)
                         .KeyProperty(x => x.PlatformTitle);
        }
    }
}
