using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamingLeagues.Entities
{
    public class Match
    {
        // Primary key
        public virtual int Id { get; protected set; }

        // Attributes
        public virtual DateTime DatePlayed { get; set; }
        public virtual int HomeScore { get; set; }
        public virtual int AwayScore { get; set; }

        // Many-to-many relationship
        public virtual IList<Player> Players { get; set; }

        // Many-to-one relationship
        public virtual League League { get; set; }

        public Match()
        {
            Players = new List<Player>();
            League = null;
        }
    }
}
