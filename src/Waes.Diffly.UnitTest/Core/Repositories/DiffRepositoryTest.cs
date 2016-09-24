using System;
using Waes.Diffly.Core.Domain.Enums;
using Waes.Diffly.Core.Interfaces.Repositories;
using Waes.Diffly.Core.Repositories;
using Xunit;

namespace Waes.Diffly.UnitTest.Core.Repositories
{
    public class DiffRepositoryTest
    {
        IDiffRepository _repo;

        public DiffRepositoryTest()
        {
            _repo = new DiffRepository();

        }

        [Fact]
        public void Add_AddsValueToLeft_ValueIsPresentInRepositoryAfterTheAdd()
        {
            string value = Convert.ToString(10, 2);
            _repo.Add(1, DiffSide.Left, value);

            string actual = _repo.GetById(1, DiffSide.Left);

            Assert.Equal(value, actual);
        }

        [Fact]
        public void Add_AddsValueToLeft_ValueIsPresentInRight()
        {
            string value = Convert.ToString(10, 2);
            _repo.Add(1, DiffSide.Left, value);

            string actual = _repo.GetById(1, DiffSide.Right);

            Assert.Equal(null, actual);
        }
    }
}
