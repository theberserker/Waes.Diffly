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
        DiffEntity AddOrUpdate(DiffEntity entity);

        /// <summary>
        /// Gets or adds the entity.
        /// </summary>
        /// <param name="id">Id of the entity.</param>
        /// <param name="entity">The diff entity.</param>
        /// <returns>The entity from repository if exists or the given entity argument that was added.</returns>
        DiffEntity GetOrAdd(int id, DiffEntity entity);
    }
}
