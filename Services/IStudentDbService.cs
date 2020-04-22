using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cw5.DTOs;
using Cw5.DTOs.Requests;
using Cw5.DTOs.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Cw5.Services
{
    public interface IStudentDbService
    {
        void EnrollStudent(EnrollStudentRequest request);
        void PromoteStudents(PromoteStudentRequest request);
        bool CheckIndexNumber(string index);
        bool CheckToken(string token);
        public bool CheckUserPassword(LoginRequestDto index);
        void AddToken(string requestLogin, string refToken);
        public void ChangeToken(string refToken, string newRefToken);
    }
}
