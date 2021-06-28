using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    [Table("TB_M_Cases")]
    public class Case
    {
        [Key]
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Review { get; set; }
        public int CategoryId { get; set; }
        public int StatusCodeId { get; set; }
        public int PriorityId { get; set; }
        public virtual Category Category { get; set; }
        public virtual StatusCode StatusCode { get; set; }
        public virtual Priority Priority { get; set; }
        public virtual Solve Solve { get; set; }
        public virtual Attachment Attachment { get; set; }

    }
}
