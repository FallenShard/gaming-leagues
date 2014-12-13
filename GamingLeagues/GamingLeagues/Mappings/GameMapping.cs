using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentNHibernate.Mapping;

using GamingLeagues.Entities;

namespace GamingLeagues.Mappings
{
    public class GameMapping : ClassMap<Game>
    {
        public GameMapping()
        {
            // Primary key mapping
            Id(x => x.Id);

            // Attribute mapping
            Map(x => x.Title);
            Map(x => x.Developer);
            Map(x => x.ReleaseDate);
            Map(x => x.Genre);

            // Multi-value attribute mapping to platforms
            HasMany(x => x.SupportedPlatforms)
                .KeyColumn("GameID")
                .Cascade.AllDeleteOrphan()
                .Inverse();

            // Many-to-many mapping to players
            HasManyToMany(x => x.Players)
                .Table("PlaysGames")
                .ParentKeyColumn("GameID").ChildKeyColumn("PlayerID")
                .Cascade.All()
                .Inverse();

            // One-to-many mapping to leagues
            HasMany(x => x.Leagues)
                .KeyColumn("GameID")
                .Cascade.All()
                .Inverse();
        }
    }
}
