﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
 using Cw5.ModelsEF;

 namespace Cw5.ServicesEF
{
    public class EfDbService : IEfDbService
    {
        public IEnumerable<Student> GetPeople()
        {
            var db = new s18706Context();
            return db.Student.ToList();
        }
    }
}
