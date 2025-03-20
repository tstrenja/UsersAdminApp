using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAdmin.Infrastructure.Model;

namespace UserAdmin.Core.Model
{
    public class LogDTO
    {
        public Guid Id { get; set; }
        public string Browser { get; set; }
        public DateTime Date { get; set; }
        public Guid UserId { get; set; } 
        public UserDTO User { get; set; }
    }
}
