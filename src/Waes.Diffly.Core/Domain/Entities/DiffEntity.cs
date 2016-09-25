using System;
using Waes.Diffly.Core.Domain.Enums;
using Waes.Diffly.Core.Domain.Helpers;
using Waes.Diffly.Core.Exceptions;

namespace Waes.Diffly.Core.Domain.Entities
{
    public class DiffEntity
    {
        private static Func<string, string> _decodeFunc = ConvertHelper.FromAsciiBase64;

        public DiffEntity(int id, DiffSide side, string base64Value)
        {
            Id = id;
            string decoded = _decodeFunc(base64Value);

            AssignSideProperty(side, decoded, false);
        }

        public int Id { get; set; }
        public string Left { get; set; }
        public string Right { get; set; }


        /// <summary>
        /// Assigns the Left or Right property of an instance.
        /// </summary>
        /// <param name="side">What side we are diffing.</param>
        /// <param name="diffValue">Value for diffing.</param>
        /// <param name="decode">Notes if the diffValue should be decoded.</param>
        public void AssignSideProperty(DiffSide side, string diffValue, bool decode)
        {
            switch (side)
            {
                case DiffSide.Left:
                    Left = decode ? _decodeFunc(diffValue) : diffValue;
                    break;
                case DiffSide.Right:
                    Right = decode ? _decodeFunc(diffValue) : diffValue;
                    break;
                default:
                    throw new DiffDomainException($"Side provided is not supported. Side should be '{DiffSide.Left.ToString().ToLower()}' or '{DiffSide.Right.ToString().ToLower()}'.", 400);
            }
        }
    }
}
