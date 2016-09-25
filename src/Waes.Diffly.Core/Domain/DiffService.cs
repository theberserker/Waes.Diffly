using System;
using Waes.Diffly.Api.Dtos;
using Waes.Diffly.Api.Dtos.Enums;
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
            AddPrivate(id, side, encodedData, allowUpdateSideProperty: false);
        }

        public void AddOrUpdate(int id, DiffSide side, string encodedData)
        {
            AddPrivate(id, side, encodedData, allowUpdateSideProperty: true);
        }

        public DiffResultDto Diff(int id)
        {
            var entity = _repository.GetById(id);
            if (entity?.Left == null || entity?.Right == null)
            {
                throw new DiffDomainException("Can not diff because not both diff sides were provided.", 400);
            }

            if (entity.Left.Length != entity.Right.Length)
            {
                return new DiffResultDto(DiffResultType.NotEqualSize);
            }


            throw new NotImplementedException("TODO: Jutri - kako binary diff narediti?");

        }

        private void AddPrivate(int id, DiffSide side, string encodedData, bool allowUpdateSideProperty)
        {
            var entity = _repository.GetById(id);
            if (entity == null)
            {
                entity = new DiffEntity(id, side, encodedData);
                _repository.Add(entity);
            }
            else
            {
                if (!allowUpdateSideProperty)
                {
                    ThrowIfSidePropertyHasValue(side, entity);
                }
                entity.AssignSideProperty(side, encodedData, true);
            }
        }

        private void ThrowIfSidePropertyHasValue(DiffSide side, DiffEntity diffEntity)
        {
            if (side == DiffSide.Left && diffEntity.Left != null)
            {
                throw new DiffDomainException("Left value already set!", 400);
            }
            else if (side == DiffSide.Right && diffEntity.Right != null)
            {
                throw new DiffDomainException("Right value already set!", 400);
            }
        }
    }
}
