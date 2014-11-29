using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamingLeagues.Entities
{
    public class Platform
    {
        // Primary key
        //public virtual int Id { get; protected set; }

        public virtual Game VideoGame { get; set; }
        public virtual string PlatformTitle { get; set; }

        #region NHibernate Composite Key Requirements
        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            var otherPlatform = obj as Platform;
            if (otherPlatform == null) return false;

            if (VideoGame == otherPlatform.VideoGame && PlatformTitle == otherPlatform.PlatformTitle)
                return true;

            return false;
        }
        public override int GetHashCode()
        {
            return (VideoGame.Title.ToString() + "|" + PlatformTitle).GetHashCode();
        }
        #endregion

        public Platform()
        {
            VideoGame = null;
        }
    }
}
