using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamingLeagues.Entities
{
    public class Player
    {
        public virtual int Id { get; protected set; }
        public virtual string Name { get; set; }
        public virtual string LastName { get; set; }
    }
}
