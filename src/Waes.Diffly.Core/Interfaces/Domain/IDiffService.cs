using System;
using System.Collections.Generic;
using Waes.Diffly.Api.Dtos.Enums;
using Waes.Diffly.Core.Domain.Enums;

namespace Waes.Diffly.Core.Interfaces.Domain
{
    public interface IDiffService
    {
        void Add(int id, DiffSide side, string encodedData);
        void AddOrUpdate(int id, DiffSide side, string encodedData);
        Tuple<DiffResultType, IEnumerable<int>> Diff(int id);
    }

}
