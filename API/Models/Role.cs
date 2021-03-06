using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    [Table("TB_Role")]
    public class Role: IdentityRole
    {
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
