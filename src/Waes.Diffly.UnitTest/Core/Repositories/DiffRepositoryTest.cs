using System;
using System.Text;
using Waes.Diffly.Core.Domain.Entities;
using Waes.Diffly.Core.Domain.Enums;
using Waes.Diffly.Core.Interfaces.Repositories;
using Waes.Diffly.Core.Repositories;
using Xunit;

namespace Waes.Diffly.UnitTest.Core.Repositories
{
    public class DiffRepositoryTest
    {
        private static byte[] value1 = TestHelper.GetBytes(192); // 11000000 (0xc0)
        private static string value1Base64 = TestHelper.GetBase64EncodedBytes(192); // 11000000 (0xc0)

        private readonly IDiffRepository _repo;

        public DiffRepositoryTest()
        {
            _repo = new DiffRepository();

        }

        //    [Fact]
        //    public void Add_AddsEncodedValueToLeft_DiffEntityIsCreatedAndHasValueInLeft()
        //    {
        //        var entity = new DiffEntity(1, DiffSide.Left, value1Base64);
        //        _repo.AddOrUpdate(entity);

        //        var actual = _repo.GetById(1);

        //        Assert.Equal(value1, actual.Left);
        //    }

        //    [Fact]
        //    public void Add_AddsEncodedValueOnlyToLeft_ValueIsNotPresentInRight()
        //    {
        //        var entity = new DiffEntity(1, DiffSide.Left, value1Base64);
        //        _repo.AddOrUpdate(entity);

        //        var actual = _repo.GetById(1);

        //        Assert.Equal(null, actual.Right);
        //    }
    }
}
