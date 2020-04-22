using System.Collections.Generic;
using Cw5.DTOs;
using Cw5.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cw5.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetStudents()
        {
            var list = new List<ShortStudent>();
            list.Add(new ShortStudent
            {
                IdStudent = 1,
                Name = "Andrzej"
            });
            list.Add(new ShortStudent
            {
                IdStudent = 3,
                Name = "Wieslaw"
            });
            return Ok(list);
        }

    }
}