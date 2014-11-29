using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamingLeagues.Entities
{
    public class Team
    {
        // Primary key
        public virtual int Id { get; protected set; }

        // Attributes
        public virtual string Name { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual DateTime DateDisbanded { get; set; }
        public virtual string Country { get; set; }

        // One-to-many relationship
        public virtual IList<Player> Players { get; set; }
        
        // Many-to-many relationship
        public virtual IList<Sponsor> Sponsors { get; set; }

        public Team()
        {
            Players = new List<Player>();
            Sponsors = new List<Sponsor>();
        }
    }
}
