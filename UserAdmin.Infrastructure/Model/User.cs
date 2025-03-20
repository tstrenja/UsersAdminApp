using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAdmin.Infrastructure.Model
{
    public  class User
    {
        public Guid Id { get; set; }
        public  string FirstName { get; set; }
        public  string LastName { get; set; }
        public  string UserName { get; set; }
        public  string Password { get; set; }
        public int LoginCount { get; set; }

        public virtual ICollection<Logs> Logs { get; set; }

    }
}
