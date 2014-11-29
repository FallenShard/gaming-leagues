using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamingLeagues.Entities
{
    public class Game
    {
        // Primary key
        public virtual int Id { get; protected set; }

        // Attributes
        public virtual string Title { get; set; }
        public virtual string Developer { get; set; }
        public virtual DateTime ReleaseDate { get; set; }
        public virtual string Genre { get; set; }
        
        // Multi-valued attribute
        public virtual IList<Platform> SupportedPlatforms { get; set; }

        // Many-to-one relationship
        // public virtual IList<League> Leagues { get; set; }

        public Game()
        {
            SupportedPlatforms = new List<Platform>();
        }
    }
}
