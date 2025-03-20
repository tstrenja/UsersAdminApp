using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAdmin.Core.Model
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Molimo unesite login")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Molimo unesite lozinka")]
        public string Password { get; set; }
    }
}
