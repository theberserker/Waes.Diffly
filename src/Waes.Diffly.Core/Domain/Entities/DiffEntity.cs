using System;
using Waes.Diffly.Core.Domain.Enums;
using Waes.Diffly.Core.Exceptions;

namespace Waes.Diffly.Core.Domain.Entities
{
    /// <summary>
    /// Entity that contains the information for performig the diff.
    /// </summary>
    public class DiffEntity
    {
        public DiffEntity(int id, DiffSide side, string base64Value)
        {
            Id = id;
            AssignSideProperty(side, base64Value);
        }

        public DiffEntity(int id, byte[] left, byte[] right)
        {
            this.Id = id;
            this.Left = left;
            this.Right = right;
        }

        /// <summary>
        /// Id of the Diff object.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Left side for the diff.
        /// </summary>
        public byte[] Left { get; set; }

        /// <summary>
        /// Right side for the diff.
        /// </summary>
        public byte[] Right { get; set; }


        /// <summary>
        /// Assigns the Left or Right property of an instance.
        /// </summary>
        /// <param name="side">What side we are diffing.</param>
        /// <param name="base64string">Base64 encoded string of the binary data submitted for diff.</param>
        public void AssignSideProperty(DiffSide side, string base64string)
        {
            AssignSideProperty(side, Convert.FromBase64String(base64string));
        }

        /// <summary>
        /// Assigns the Left or Right property of an instance.
        /// </summary>
        /// <param name="side">What side we are diffing.</param>
        /// <param name="bytes">Value for diffing.</param>
        public void AssignSideProperty(DiffSide side, byte[] bytes)
        {
            switch (side)
            {
                case DiffSide.Left:
                    Left = bytes;
                    break;
                case DiffSide.Right:
                    Right = bytes;
                    break;
                default:
                    throw new DiffDomainException($"Side provided is not supported. Side should be '{DiffSide.Left.ToString().ToLower()}' or '{DiffSide.Right.ToString().ToLower()}'.", 400);
            }
        }
    }
}
