﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.Utilities.Results
{
    public interface IDataResult
    {
        public interface IDataResult<T> : IResult 
        {
            T Data { get; }
        }
    }
}
