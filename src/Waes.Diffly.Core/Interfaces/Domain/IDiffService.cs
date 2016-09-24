using Waes.Diffly.Core.Domain.Enums;

namespace Waes.Diffly.Core.Interfaces.Domain
{
    public interface IDiffService
    {
        void Add(int id, DiffSide side, string encodedData);
        void AddOrUpdate(int id, DiffSide side, string encodedData);
    }

}
