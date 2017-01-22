using System;
using System.Collections.Generic;
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
            AddOrUpdatePrivate(id, side, encodedData, ThrowIfSidePropertyHasValue);
        }

        /// <summary>
        /// Adds or updates the provided data to the repository.
        /// </summary>
        /// <param name="id">Id for the diff.</param>
        /// <param name="side">Side of diff.</param>
        /// <param name="encodedData">Base64 encoded data for diffing.</param>
        public void AddOrUpdate(int id, DiffSide side, string encodedData)
        {
            AddOrUpdatePrivate(id, side, encodedData);
        }

        /// <summary>
        /// Diffs the byte arrays stored for the provided id and retuns the result. Throws exception if any of the data is not present.
        /// TODO: This is a long running operation if not cached. Make this as a Task on background thread (Task.Factory.StartNew)!
        /// </summary>
        /// <param name="id">Id of the diff.</param>
        /// <returns></returns>
        public Tuple<DiffResultType, IEnumerable<int>> Diff(int id)
        {
            var entity = _repository.GetById(id);
            if (entity == null)
            {
                throw new DiffDataIncompleteException($"Can not perform diff because no entry was provided for this id.");
            }

            return entity.GetDiffResult();
        }

        /// <summary>
        /// Adds or updates the provided data to the repository.
        /// </summary>
        /// <param name="id">Id for the diff.</param>
        /// <param name="side">Side of diff.</param>
        /// <param name="encodedData">Base64 encoded data for diffing.</param>
        /// <param name="onUpdate">Action to perform pre-update.</param>
        private void AddOrUpdatePrivate(int id, DiffSide side, string encodedData, Action<DiffSide, DiffEntity> onUpdate = null)
        {
            var entity = new DiffEntity(id, side, encodedData);
            var repoEntity = _repository.GetOrAdd(id, entity);
            if (entity != repoEntity) // it is update of the entity in repository
            {
                onUpdate?.Invoke(side, repoEntity);
                repoEntity.AssignSideProperty(side, encodedData);
            }
        }

        private static void ThrowIfSidePropertyHasValue(DiffSide side, DiffEntity diffEntity)
        {
            if (side == DiffSide.Left && diffEntity.Left != null)
            {
                throw new DiffDomainException("Left value already set! Maybe you want to update it using PUT?");
            }
            else if (side == DiffSide.Right && diffEntity.Right != null)
            {
                throw new DiffDomainException("Right value already set! Maybe you want to update it using PUT?");
            }
        }
    }
}
