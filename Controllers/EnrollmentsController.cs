using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Cw5.DTOs;
using Cw5.DTOs.Requests;
using Cw5.DTOs.Responses;
using Cw5.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Cw5.Controllers
{
    [Route("api/enrollments")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private IStudentDbService _service;

        public EnrollmentsController(IStudentDbService service)
        {
            _service = service;
        }
        //
        // public IConfiguration Configuration { get; set; }
        // public EnrollmentsController(IConfiguration configuration)
        // {
        //     Configuration = configuration;
        // }


        [HttpPost]
        [Authorize(Roles = "employee")]
        public IActionResult EnrollStudent(EnrollStudentRequest request)
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
            _service.EnrollStudent(request);
        }

        [HttpPost("promotions")]
        [Authorize(Roles = "employee")]
        public IActionResult PromoteStudent(PromoteStudentRequest request)
        {
            _service.PromoteStudents(request);
            return Ok();
        }
        
        [HttpPost("login")]
        public IActionResult Login(LoginRequestDto request)
        {

            if (!_service.CheckUserPassword(request))
                return Ok("User or password incorrect");
            
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Name, "admin"),
                new Claim(ClaimTypes.Role, "employee"),
                new Claim(ClaimTypes.Role, "student")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Asjdsaivj22kaljoasdjoiasojiasojiaoijasojiadsojidoijasoij"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
            (
                issuer: "Gakko",
                audience: "Students",
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken=Guid.NewGuid()
            });
        }
    }
}