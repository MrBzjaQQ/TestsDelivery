﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsPortal.DAL.Repositories.Tests
{
    public interface ITestsRepository
    {
        void CreateTest(Models.Tests.Test test);
    }
}
