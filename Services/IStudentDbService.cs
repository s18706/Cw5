using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cw5.DTOs.Requests;
using Cw5.DTOs.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Cw5.Services
{
    public interface IStudentDbService
    {
        void EnrollStudent(EnrollStudentRequest request);
        void PromoteStudents(int semester, string studies);
    }
}
