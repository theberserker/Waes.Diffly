using System;
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

        public void Add(int id, DiffSide side, string encodedData)
        {
            var value = _repository.GetById(id);
            if (value == null)
            {
                value = new DiffEntity(id, side, encodedData);
            }
            else
            {
                if (side == DiffSide.Left && value.Left != null)
                {
                    throw new DiffDomainException("Left value already set!", 409);
                }
                else if (side == DiffSide.Right && value.Right != null)
                {
                    throw new DiffDomainException("Right value already set!", 409);
                }
                value.AssignSideProperty(side, encodedData, true);
            }
        }

        public void AddOrUpdate(int id, DiffSide side, string encodedData)
        {
            var value = _repository.GetById(id);
            if (value == null)
            {
                value = new DiffEntity(id, side, encodedData);
            }
            else
            {
                value.AssignSideProperty(side, encodedData, true);
            }
        }
    }
}
