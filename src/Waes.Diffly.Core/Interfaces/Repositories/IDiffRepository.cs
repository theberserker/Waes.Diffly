using System;
using Waes.Diffly.Api.Dtos;
using Waes.Diffly.Core.Domain.Entities;
using Waes.Diffly.Core.Domain.Enums;

namespace Waes.Diffly.Core.Interfaces.Repositories
{
    public interface IDiffRepository
    {

        /// <summary>
        /// Gets the DiffEntity for the diffing for the specified id.
        /// </summary>
        /// <param name="id">Identifier for the objects to diff.</param>
        /// <returns>The value.</returns>
        DiffEntity GetById(int id);

        /// <summary>
        /// Adds item to the repository.
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="addFactory">Factory to create entity.</param>
        /// <param name="updateAction">Update action on existing entity.</param>
        void AddOrUpdate(int id, Func<DiffEntity> addFactory, Func<DiffEntity, DiffEntity> updateAction = null);

        
        //void AddOrUpdate(DiffEntity entity);
    }
}
