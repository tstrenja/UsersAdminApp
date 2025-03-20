using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAdmin.Infrastructure.Model
{
    public class Logs
    {
        public Guid Id { get; set; }
        public string Browser { get; set; }
        public DateTime Date { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
    }
}
