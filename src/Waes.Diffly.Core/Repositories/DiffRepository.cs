using System;
using System.Collections.Concurrent;
using Waes.Diffly.Api.Dtos;
using Waes.Diffly.Core.Domain.Entities;
using Waes.Diffly.Core.Domain.Enums;
using Waes.Diffly.Core.Interfaces.Repositories;

namespace Waes.Diffly.Core.Repositories
{
    public class DiffRepository : IDiffRepository
    {
        private static ConcurrentDictionary<int, DiffEntity> _dictionary = new ConcurrentDictionary<int, DiffEntity>();

        public void Add(DiffEntity entity)
        {
            _dictionary.AddOrUpdate(entity.Id, entity, (key, oldValue) => entity);
        }

        public DiffEntity GetById(int id)
        {
            return _dictionary.ContainsKey(id) ? _dictionary[id] :  null;
        }
    }
}
