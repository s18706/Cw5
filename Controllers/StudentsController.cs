using System.Collections.Generic;
using System.Linq;
using Cw5.DTOs;
using Cw5.Models;
using Cw5.ModelsEF;
using Cw5.ServicesEF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cw5.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        
        private readonly IEfDbService _context;

        public StudentsController(IEfDbService context)
        {
            _context = context;
        }
        
        [HttpGet]
        public IActionResult GetStudents()
        {
            return Ok(_context.GetPeople());
        }

    }
}