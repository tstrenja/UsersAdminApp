using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAdmin.Core.Model
{
    public class UserDTO
    {
        public Guid? Id { get; set; }

        [Required(ErrorMessage = "Molimo unesite ime")]
        [StringLength(30, MinimumLength = 3)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Molimo unesite prezime")]
        [StringLength(30, MinimumLength = 3)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Molimo unesite login")]
        [StringLength(30, MinimumLength = 3)]
        public string UserName { get; set; }


        [Required(ErrorMessage = "Molimo unesite lozinku")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public int? LoginCount { get; set; }
    }
}
