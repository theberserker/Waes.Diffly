using System;
using System.Collections.Generic;
using System.Linq;
using Waes.Diffly.Api.Dtos.Enums;
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

        public Tuple<DiffResultType, IEnumerable<int>> _diffResult;

        public Tuple<DiffResultType, IEnumerable<int>> DiffResult
        {
            get
            {
                if (_diffResult == null)
                {
                    _diffResult = GetDiff();
                }

                return _diffResult;
            }
        }

        private Tuple<DiffResultType, IEnumerable<int>> GetDiff()
        {
            ThrowIfAnyDiffPropertyNull();

            if (Left.Length != Right.Length)
            {
                return new Tuple<DiffResultType, IEnumerable<int>>(DiffResultType.NotEqualSize, Enumerable.Empty<int>());
            }

            var diff = FindByteArrayDiff(Left, Right);
            var resultType = diff.Any() ? DiffResultType.NotEqual : DiffResultType.Equal;

            return new Tuple<DiffResultType, IEnumerable<int>>(resultType, diff);
        }

        /// <summary>
        /// Finds the diffrences in the provided same lenght byte arrays.
        /// </summary>
        /// <param name="bytes1">First set of bytes.</param>
        /// <param name="bytes2">Second set of bytes.</param>
        /// <returns></returns>
        public IEnumerable<int> FindByteArrayDiff(byte[] bytes1, byte[] bytes2)
        {
            if (bytes1.Length != bytes2.Length)
            {
                throw new ArgumentException("Byte arrays are of different length. Method expects same lenght arguments.");
            }

            for (int i = 0; i < bytes1.Length; i++)
            {
                if (bytes1[i] != bytes2[i])
                {
                    yield return i;
                }
            }
        }

        /// <summary>
        /// Assigns the Left or Right property of an instance.
        /// </summary>
        /// <param name="side">What side we are diffing.</param>
        /// <param name="base64string">Base64 encoded string of the binary data submitted for diff.</param>
        public void AssignSideProperty(DiffSide side, string base64string)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(base64string);
                AssignSideProperty(side, bytes);
            }
            catch (FormatException ex)
            {
                throw new DiffDomainException("The provided string was not in the Base64 format", ex);
            }
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
                    throw new DiffDomainException($"Side provided is not supported. Side should be '{DiffSide.Left.ToString().ToLower()}' or '{DiffSide.Right.ToString().ToLower()}'.");
            }
        }

        private void ThrowIfAnyDiffPropertyNull()
        {
            if (Left == null)
            {
                throw new DiffDomainException("Can not diff because left side was not provided.");
            }
            if (Right == null)
            {
                throw new DiffDomainException("Can not diff because right side was not provided.");
            }
        }
    }
}
