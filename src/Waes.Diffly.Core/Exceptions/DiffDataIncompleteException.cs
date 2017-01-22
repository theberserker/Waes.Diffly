namespace Waes.Diffly.Core.Exceptions
{
    public class DiffDataIncompleteException : DiffDomainException
    {
        /// <summary>
        /// Exception is thrown when diff data is incomplete and therefore diff can not be performed.
        /// </summary>
        /// <param name="message"></param>
        public DiffDataIncompleteException(string message) : base(message)
        {
        }
    }
}
