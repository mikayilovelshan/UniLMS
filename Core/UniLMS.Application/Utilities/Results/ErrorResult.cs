using System;
using System.Collections.Generic;
using System.Text;

namespace UniLMS.Application.Utilities.Results
{
    public class ErrorResult : Result
    {
        public ErrorResult(bool success) : base(false)
        {
        }

        public ErrorResult(bool success, string message) : base(false, message)
        {
        }

        public ErrorResult(string message) : base(false, message)
        {

        }
    }
}
