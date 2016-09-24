using Waes.Diffly.Api.Dtos;
using Waes.Diffly.Core.Domain.Enums;

namespace Waes.Diffly.Core.Interfaces.Repositories
{
    public interface IDiffRepository
    {
        /// <summary>
        /// Gets the value for the diffing for the specified id and side.
        /// </summary>
        /// <param name="id">Identifier for the object to diff.</param>
        /// <param name="side">Side for the diff.</param>
        /// <returns>The value.</returns>
        string GetById(int id, DiffSide side);

        /// <summary>
        /// Adds item to the repository.
        /// </summary>
        /// <param name="id">Identifier for the object to diff.</param>
        /// <param name="side">Side for the diff.</param>
        /// <param name="value">Value to be added.</param>
        void Add(int id, DiffSide side, string value);

        //DiffRequestDto Find(int id);
        //DiffRequestDto Remove(int id);
        //void Update(DiffRequestDto item);
    }
}
