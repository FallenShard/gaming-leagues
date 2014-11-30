using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamingLeagues.Entities
{
    public class League
    {
        // Primary key
        public virtual int Id { get; protected set; }

        // Attributes
        public virtual string Name { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime EndDate { get; set; }
        public virtual float Budget { get; set; }

        // Many-to-one relationship
        public virtual Game Game { get; set; }

        // Many-to-many relationship
        public virtual IList<Sponsor> Sponsors { get; set; }

        // One-to-many relationship
        public virtual IList<PlaysInLeague> Rankings { get; set; }
        public virtual IList<Match> Matches { get; set; }

        public League()
        {
            Game = null;
            Sponsors = new List<Sponsor>();
            Rankings = new List<PlaysInLeague>();
            Matches = new List<Match>();
        }
    }
}