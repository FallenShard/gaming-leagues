using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamingLeagues.Entities
{
    public class Player
    {
        // Primary key
        public virtual int Id { get; protected set; }

        // Attributes
        public virtual string Name { get; set; }
        public virtual string LastName { get; set; }
        public virtual string NickName { get; set; }
        public virtual char Gender { get; set; }
        public virtual DateTime DateOfBirth { get; set; }
        public virtual string Country { get; set; }
        public virtual DateTime DateTurnedPro { get; set; }
        public virtual float CareerEarnings { get; set; }

        public virtual string NameNickLast
        { 
            get
            {
                return Name + " \"" + NickName + "\" " + LastName;
            } 
        }

        public virtual string ClanName
        {
            get
            {
                string tag = CurrentTeam == null ? "" : CurrentTeam.Tag;
                return tag + " " + NickName;
            }
        }

        // Many-to-one relationship
        public virtual Team CurrentTeam { get; set; }

        // Many-to-many relationship
        public virtual IList<Game> Games { get; set; }
        public virtual IList<League> Leagues { get; set; }

        // One-to-many relationship
        public virtual IList<Match> MatchesPlayed { get; set; }
        
        public Player()
        {
            CurrentTeam = null;
            Games = new List<Game>();
            Leagues = new List<League>();
            MatchesPlayed = new List<Match>();
        }
    }
}
