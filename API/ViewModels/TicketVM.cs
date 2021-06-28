using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels
{
    public class TicketVM
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public int CategoryId { get; set; }
        public int StatusCodeId { get; set; }
        public int PriorityId { get; set; }
        public DateTime Time { get; set; }
        public string Name { get; set; }
        public string FileType { get; set; }
        public IFormFile Content { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
