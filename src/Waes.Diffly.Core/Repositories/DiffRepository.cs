using System.Collections.Concurrent;
using Waes.Diffly.Core.Domain.Entities;
using Waes.Diffly.Core.Interfaces.Repositories;

namespace Waes.Diffly.Core.Repositories
{
    public class DiffRepository : IDiffRepository
    {
        private static ConcurrentDictionary<int, DiffEntity> _dictionary = new ConcurrentDictionary<int, DiffEntity>();

        public DiffEntity AddOrUpdate(DiffEntity entity)
        {
            return _dictionary.AddOrUpdate(entity.Id, entity, (key, oldValue) => entity);
        }

        public DiffEntity GetById(int id)
        {
            return _dictionary.ContainsKey(id) ? _dictionary[id] :  null;
        }

        public DiffEntity GetOrAdd(int id, DiffEntity entity)
        {
            return _dictionary.GetOrAdd(id, entity);
        }
    }
}
