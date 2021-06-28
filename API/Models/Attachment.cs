using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{ [Table("TB_M_Attachments")]
    public class Attachment
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string FileType { get; set; }
        public byte [] Content {get; set; }
        public DateTime CreatedOn { get; set; }
        public virtual Case Case { get; set; }
    }
}
