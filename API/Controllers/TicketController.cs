using API.Base;
using API.Context;
using API.Models;
using API.Repository.Data;
using API.Services;
using API.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly MyContext _context;

        private readonly TicketRepositories _repo;
        public TicketController(TicketRepositories repo)
        {
            _repo = repo;
        }

        [Route("CreateTicket")]
        [HttpPost]
        public async Task<ActionResult> Post([FromForm] TicketVM model)
        {
            var maxFileSize = 1048576;
            if (model.Content.ContentType != "image/png")
            {
                return BadRequest("Uploaded File Must be PNG !");
            }
            if (model.Content.Length > maxFileSize)
            {
                return BadRequest("Uploaded File Maximum Size is 1MB !");
            }
            var result = await _repo.CreateNewRequest(model);
            if (result != null)
            {
                return BadRequest(result);
            }
            return Ok("Request Successfully Created !");
        }
    }

}
