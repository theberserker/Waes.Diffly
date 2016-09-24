using System;

namespace Waes.Diffly.Core.Exceptions
{
    // TODO: Make this Serialzable? .NET Core is not happy yet with the [SerializableAttribute]... More: https://github.com/dotnet/coreclr/issues/1203

    /// <summary>
    /// Exception that is thrown if error occours in domain logic for diffing. 
    /// In case error codes are set between HTTP valid values for errors (4xx - Client Error or 5xx Client Error), these will be propagated as response by API.
    /// </summary>
    public class DiffDomainException : Exception
    {
        public DiffDomainException(string message) : base(message)
        {
        }

        public DiffDomainException(string message, int code) : base(message)
        {
            this.ErrorCode = code;
        }

        public int? ErrorCode { get; private set; }

        public bool ReflectAsHttpErrorCode => ErrorCode.HasValue && ErrorCode.Value >= 400 && ErrorCode < 600;
    }
}
