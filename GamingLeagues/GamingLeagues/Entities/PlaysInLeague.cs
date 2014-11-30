using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamingLeagues.Entities
{
    class PlaysInLeague
    {
        // Key attributes
        // Many-to-one relationship
        public virtual Player Player { get; set; }
        public virtual League League { get; set; }

        // Attributes
        public virtual int Points { get; set; }

        #region NHibernate Composite Key Requirements
        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            var otherPlaysInLeague = obj as PlaysInLeague;
            if (otherPlaysInLeague == null) return false;

            if (Player == otherPlaysInLeague.Player && League == otherPlaysInLeague.League)
                return true;

            return false;
        }
        public override int GetHashCode()
        {
            return (Player.ToString() + "|" + League.ToString()).GetHashCode();
        }
        #endregion

        public PlaysInLeague()
        {
            Player = null;
            League = null;
        }
    }
}
