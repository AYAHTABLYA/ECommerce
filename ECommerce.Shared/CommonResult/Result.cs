using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.CommonResult
{
    public class Result
    {
        // Is Succsess
        // Is Failure
        // Errors [Code - Description - Type]

        public readonly List<Error> _errors = []; 
        public bool IsSuccess => !_errors.Any();
        public bool IsFailure => _errors.Any();

        // Return Errors As ReadOnly Collection
        public IReadOnlyList<Error> Errors => _errors;

        // No Erros => Empty CTOR
        protected Result() { }

        // One Error => 
        protected Result(Error error)
        {
            _errors.Add(error);
        }
        // More Than One Error =>
        protected Result(List<Error> errors)
        {
            _errors.AddRange(errors);
        }

        // Factory Pattern Methods With Singele Responsibility Principle To => Set values in CTORs
        public static Result Ok() => new Result();
        
        public static Result Fail(Error error) => new Result(error);

        public static Result Fail(List<Error> errors) => new Result(errors);
    }

    public class Result<TValue> : Result
    {
        private readonly TValue _value;
        public TValue Value => IsSuccess ? _value : throw new InvalidOperationException("Cannot access the value of a failed result.");
        private Result(TValue value)
        {
            _value = value;
        }
        private Result(Error error) : base(error)
        {
            _value = default!;
        }
        private Result(List<Error> errors) : base(errors)
        {
            _value = default!;
        }

        // Factory Pattern Methods With Singele Responsibility Principle To => Set values in CTORs
        public static Result<TValue> Ok(TValue value) => new Result<TValue>(value);
        public static new Result<TValue> Fail(Error error) => new Result<TValue>(error);
        public static new Result<TValue> Fail(List<Error> errors) => new Result<TValue>(errors);

        //Implicit Conversion
        public static implicit operator Result<TValue>(TValue value) => Ok(value);
        public static implicit operator Result<TValue>(Error error) => Fail(error);
        public static implicit operator Result<TValue>(List<Error> errors) => Fail(errors);
    }
}
