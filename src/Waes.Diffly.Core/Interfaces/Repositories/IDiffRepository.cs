﻿using System;
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
        void AddOrUpdate(int id, Func<DiffEntity> addFactory, Func<DiffEntity, DiffEntity> updateAction);

        
        //void AddOrUpdate(DiffEntity entity);
    }
}
