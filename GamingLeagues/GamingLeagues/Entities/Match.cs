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

        // Many-to-one relationship
        public virtual Player HomePlayer { get; set; }
        public virtual Player AwayPlayer { get; set; }
        public virtual League League { get; set; }

        public Match()
        {
            HomePlayer = null;
            AwayPlayer = null;
            League = null;
        }
    }
}
