﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
 using Cw5.ModelsEF;

namespace Cw5.ServicesEF
{
    public interface IEfDbService
    {
        public IEnumerable<Student> GetPeople();
    }
}
