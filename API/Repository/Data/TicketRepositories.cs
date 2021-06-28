using API.Context;
using API.Models;
using API.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository.Data
{
    public class TicketRepositories : GeneralRepository<MyContext, Case, int>
    {
        private readonly MyContext _context;
        private readonly DbSet<TicketVM> ticket;

        public TicketRepositories(MyContext myContext) : base(myContext)
        {
            this._context = myContext;
            ticket = _context.Set<TicketVM>();
        }
        public async Task<string> CreateNewRequest(TicketVM model)
        {

            var getExtension = Path.GetExtension(model.Content.FileName);
            var fileName = Path.GetFileName(model.Content.FileName);
            var fileContent = new byte[0];
            using (var ms = new MemoryStream())
            {
                await model.Content.CopyToAsync(ms);

                fileContent = ms.ToArray();
            }

            var objfiles = new Case()
            {
                PriorityId = 1,
                StartTime = DateTime.Now,
                StatusCodeId = 1,
                Subject = model.Subject,
                Description = model.Description,
                CategoryId = model.CategoryId
            };
            _context.Cases.Add(objfiles);
            _context.SaveChanges();
            var attach = new Models.Attachment()
            {
                Id = objfiles.Id,
                Name = fileName,
                FileType = getExtension,
                CreatedOn = DateTime.Now,
                Content = fileContent
            };
            _context.Attachments.Add(attach);
            var solving = new Solve()
            {
                Id = objfiles.Id
            };
            var result = await _context.SaveChangesAsync();
            if (result < 0)
            {
                return "Server Error !";
            }
            return null;
        }
    }
}
