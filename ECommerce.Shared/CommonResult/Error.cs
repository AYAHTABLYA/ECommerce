using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.CommonResult
{
    public class Error
    {

        public string Code { get; }
        public string Desctiption { get; }
        public ErrorType Type { get; }
        private Error(string code, string desctiption, ErrorType type)
        {
            Code = code;
            Desctiption = desctiption;
            Type = type;
        }

        // Factory Pattern Methods With Singele Responsibility Principle

        public static Error Failure(string code = "General Failure", string desctiption = "General Failure Occured!")
        {
            return new Error(code, desctiption, ErrorType.Failure);
        }

        public static Error Validation(string code = "Validation Error", string desctiption = "Validation Error Occured!")
        {
            return new Error(code, desctiption, ErrorType.Validation);
        }

        public static Error NotFound(string code = "Not Found", string desctiption = "Resource Not Found!")
        {
            return new Error(code, desctiption, ErrorType.NotFound);
        }

        public static Error Unauthorized(string code = "Unauthorized", string desctiption = "Unauthorized Access!")
        {
            return new Error(code, desctiption, ErrorType.Unauthorized);
        }

        public static Error Forbidden(string code = "Forbidden", string desctiption = "Access Forbidden!")
        {
            return new Error(code, desctiption, ErrorType.Forbidden);
        }

        public static Error InvalidCredentials(string code = "Invalid Credentials", string desctiption = "Invalid Credentials Provided!")
        {
            return new Error(code, desctiption, ErrorType.InvalidCredentials);
        }

    }
}
