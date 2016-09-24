using System;
using System.Collections.Concurrent;
using Waes.Diffly.Api.Dtos;
using Waes.Diffly.Core.Domain.Enums;
using Waes.Diffly.Core.Interfaces.Repositories;

namespace Waes.Diffly.Core.Repositories
{
    public class DiffRepository : IDiffRepository
    {
        public string GetById(int id, DiffSide side)
        {
            var repo = RepositoryProvider.Get(side);
            if (repo.ContainsKey(id))
            {
                return repo[id];
            }

            return null;
        }

        public void Add(int id, DiffSide side, string value)
        {
            RepositoryProvider.Get(side)[id] = value;
        }

        public static class RepositoryProvider
        {
            private static ConcurrentDictionary<int, string> _repoLeft = new ConcurrentDictionary<int, string>();
            private static ConcurrentDictionary<int, string> _repoRight = new ConcurrentDictionary<int, string>();

            internal static ConcurrentDictionary<int, string> Get(DiffSide side)
            {
                if (side == DiffSide.Unknown)
                {
                    throw new ArgumentException($"Repository does not support the {DiffSide.Unknown.ToString()}.");
                }
                return side == DiffSide.Left ? _repoLeft : _repoRight;
            }
        }
    }
}
