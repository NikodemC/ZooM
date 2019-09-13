using System;
using System.Collections.Generic;
using System.Text;
using ZooM.Core.Exceptions;

namespace ZooM.Application.Exceptions.Employee
{
    class WrongDateException : DomainException
    {
        public WrongDateException(int YOB)
            : base($"A Year {YOB} is not within allowed 1930-2001 range") { }
    }
}
