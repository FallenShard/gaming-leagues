using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamingLeagues.Entities
{
    public class Sponsor
    {
        // Primary key
        public virtual int Id { get; protected set; }

        public virtual string Name { get; set; }
        public virtual string Logo { get; set; }

        // Many-to-many relationship
        public virtual IList<Team> Teams { get; set; }
        public virtual IList<League> Leagues { get; set; }

        public Sponsor()
        {
            Teams = new List<Team>();
            Leagues = new List<League>();
        }
    }
}
