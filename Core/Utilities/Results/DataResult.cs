﻿using Core.Entities.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Text;
using static Core.Entities.Utilities.Results.IDataResult;

namespace Core.Utilities.Results
{
    public class DataResult<T> : Result, IDataResult<T>
    {
        public DataResult(T data, bool success, string message) : base(success, message)
        {
            Data = data;
        }

        public DataResult(T data, bool success) : base(success)
        {
            Data = data;
        }
        public T Data { get; }
    }
}
