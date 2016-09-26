using System;
using System.Collections.Concurrent;
using Waes.Diffly.Core.Domain.Entities;
using Waes.Diffly.Core.Domain.Enums;
using Waes.Diffly.Core.Interfaces.Repositories;

namespace Waes.Diffly.Core.Repositories
{
    public class DiffRepository : IDiffRepository
    {
        private static ConcurrentDictionary<int, DiffEntity> _dictionary = new ConcurrentDictionary<int, DiffEntity>();

        public void AddOrUpdate(int id, Func<DiffEntity> addFactory, Func<DiffEntity, DiffEntity> updateAction)
        {
            _dictionary.AddOrUpdate(id, addFactory(), (key, oldValue) => updateAction(oldValue));
        }

        public DiffEntity GetById(int id)
        {
            return _dictionary.ContainsKey(id) ? _dictionary[id] :  null;
        }
    }
}
