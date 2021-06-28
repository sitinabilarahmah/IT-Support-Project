using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    [Table("TB_M_Solves")]
    public class Solve
    {
        [Key]
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string Comment { get; set; }
        public bool StatusNameCS { get; set; }
        public bool StatusNameITS { get; set; }
        public bool StatusNameSD { get; set; }
        public virtual Case Case { get; set; }
        
    }
}
