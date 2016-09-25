using System;
using System.Collections.Generic;
using System.Linq;
using Waes.Diffly.Api.Dtos;
using Waes.Diffly.Api.Dtos.Enums;
using Waes.Diffly.Core.Domain.Entities;
using Waes.Diffly.Core.Domain.Enums;
using Waes.Diffly.Core.Exceptions;
using Waes.Diffly.Core.Interfaces.Domain;
using Waes.Diffly.Core.Interfaces.Repositories;

namespace Waes.Diffly.Core.Domain
{
    public class DiffService : IDiffService
    {
        private readonly IDiffRepository _repository;

        public DiffService(IDiffRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Adds the provided data to the repository. Throws exception if the value is already present.
        /// </summary>
        /// <param name="id">Id for the diff.</param>
        /// <param name="side">Side of diff.</param>
        /// <param name="encodedData">Base64 encoded data for diffing.</param>
        public void Add(int id, DiffSide side, string encodedData)
        {
            Add(id, side, encodedData, allowUpdateSideProperty: false);
        }

        /// <summary>
        /// Adds or updates the provided data to the repository.
        /// </summary>
        /// <param name="id">Id for the diff.</param>
        /// <param name="side">Side of diff.</param>
        /// <param name="encodedData">Base64 encoded data for diffing.</param>
        public void AddOrUpdate(int id, DiffSide side, string encodedData)
        {
            Add(id, side, encodedData, allowUpdateSideProperty: true);
        }

        /// <summary>
        /// Diffs the byte arrays stored for the provided id and retuns the result. Throws exception if any of the data is not present.
        /// </summary>
        /// <param name="id">Id of the diff.</param>
        /// <returns></returns>
        public Tuple<DiffResultType, IEnumerable<int>> Diff(int id)
        {
            var entity = _repository.GetById(id);
            if (entity?.Left == null || entity?.Right == null)
            {
                throw new DiffDomainException("Can not diff because not both diff sides were provided.", 400);
            }
            if (entity.Left.Length != entity.Right.Length)
            {
                return new Tuple<DiffResultType, IEnumerable<int>>(DiffResultType.NotEqualSize, Enumerable.Empty<int>());
            }

            var diff = FindByteArrayDiff(entity.Left, entity.Right);
            var resultType = diff.Any() ? DiffResultType.NotEqual : DiffResultType.Equal;

            return new Tuple<DiffResultType, IEnumerable<int>>(resultType, diff);
        }

        /// <summary>
        /// Adds or updates the provided data to the repository.
        /// </summary>
        /// <param name="id">Id for the diff.</param>
        /// <param name="side">Side of diff.</param>
        /// <param name="encodedData">Base64 encoded data for diffing.</param>
        /// <param name="allowUpdateSideProperty">Notes if updating of the value is possible, throws exception otherwise.</param>
        private void Add(int id, DiffSide side, string encodedData, bool allowUpdateSideProperty)
        {
            var entity = _repository.GetById(id);
            if (entity == null)
            {
                entity = new DiffEntity(id, side, encodedData);
                _repository.AddOrUpdate(entity);
            }
            else
            {
                if (!allowUpdateSideProperty)
                {
                    ThrowIfSidePropertyHasValue(side, entity);
                }
                entity.AssignSideProperty(side, encodedData);
            }
        }


        /// <summary>
        /// Finds the diffrences in the provided same lenght byte arrays.
        /// </summary>
        /// <param name="bytes1">First set of bytes.</param>
        /// <param name="bytes2">Second set of bytes.</param>
        /// <returns></returns>
        public static IEnumerable<int> FindByteArrayDiff(byte[] bytes1, byte[] bytes2)
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

        private static void ThrowIfSidePropertyHasValue(DiffSide side, DiffEntity diffEntity)
        {
            if (side == DiffSide.Left && diffEntity.Left != null)
            {
                throw new DiffDomainException("Left value already set!", 400);
            }
            else if (side == DiffSide.Right && diffEntity.Right != null)
            {
                throw new DiffDomainException("Right value already set!", 400);
            }
        }
    }
}
