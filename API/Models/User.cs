using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    [Table("TB_User")]
    public class User : IdentityUser
    {
        public virtual ICollection <UserRole> UserRoles { get; set; }
    }
}
