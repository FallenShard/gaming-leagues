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
            
            // Many-to-one mapping
            References(x => x.CurrentTeam);

            // Many-to-many mapping
            HasManyToMany(x => x.Games)
                .Cascade.All()
                .Table("PlaysGames");

            // One-to-many mapping
            HasMany(x => x.Rankings).Inverse().Cascade.All();

            // One-to-many mapping
            HasMany(x => x.MatchesPlayed).Cascade.AllDeleteOrphan().Inverse();
        }
    }
}
