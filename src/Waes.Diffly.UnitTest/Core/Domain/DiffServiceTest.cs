using Moq;
using System;
using System.Linq;
using Waes.Diffly.Api.Dtos.Enums;
using Waes.Diffly.Core.Domain;
using Waes.Diffly.Core.Domain.Entities;
using Waes.Diffly.Core.Domain.Enums;
using Waes.Diffly.Core.Exceptions;
using Waes.Diffly.Core.Interfaces.Domain;
using Waes.Diffly.Core.Interfaces.Repositories;
using Xunit;

namespace Waes.Diffly.UnitTest.Core.Domain
{
    public class DiffServiceTest
    {
        private static string value1Base64 = TestHelper.GetBase64EncodedBytes(192); // 11000000 (0xc0)
        
        private readonly Mock<IDiffRepository> _mockRepository;
        private readonly IDiffService _service;

        public DiffServiceTest()
        {
            _mockRepository = new Mock<IDiffRepository>();
            _service = new DiffService(_mockRepository.Object);
        }

        [Fact]
        public void Add_AddingEntity_CallsAddOnReposotory()
        {
            int id = 1;
            _mockRepository.Setup(x => x.AddOrUpdate(id, It.IsAny<Func<DiffEntity>>(), It.IsAny<Func<DiffEntity, DiffEntity>>()));
            _service.Add(id, DiffSide.Left, value1Base64);

            _mockRepository.Verify(x => x.AddOrUpdate(id, It.IsAny<Func<DiffEntity>>(), It.IsAny<Func<DiffEntity, DiffEntity>>()));
        }

        [Fact]
        public void Diff_TwoSameValuesAtLeftAndRight_ReturnsEqual()
        {
            var bytes = TestHelper.GetBytes(1234567890);
            int id = 1;
            var entity = new DiffEntity(id, bytes, bytes);
            _mockRepository.Setup(x => x.GetById(id)).Returns(entity);

            var actual = _service.Diff(id);

            Assert.Equal(DiffResultType.Equal, actual.Item1);
            Assert.Empty(actual.Item2);
        }

        /// <summary>
        /// Tests Diff method for DiffResultType.NotEqual scenario with multiple test cases.
        /// </summary>
        /// <param name="i1">First Int32 (4-bytes) to convert to byte array and compare.</param>
        /// <param name="i2">Second Int32 (4-bytes) to convert to byte array and compare.</param>
        /// <param name="expectedDiffIndexes">Comma-separated diff inexes expected to differ.</param>
        [Theory]
        [InlineData(126, 127, "3")]
        [InlineData(123456789, 127, "0,1,2,3")]
        [InlineData(123456789, 21, "0,1,2")]
        public void Diff_TwoDifferentValuesAtLeftAndRightOfEqualLenght_ReturnsNotEqualAndCorrectDiffIndexes(int i1, int i2, string expectedDiffIndexes)
        {
            // arrange
            var bytes1 = TestHelper.GetBytes(i1);
            var bytes2 = TestHelper.GetBytes(i2);
            int id = 1;
            var intExpectedDiffIndexes = expectedDiffIndexes.Split(',').Select(x => int.Parse(x));

            var entity = new DiffEntity(id, bytes1, bytes2);
            _mockRepository.Setup(x => x.GetById(id)).Returns(entity);

            // act
            var actual = _service.Diff(id);

            // assert
            Assert.Equal(DiffResultType.NotEqual, actual.Item1);
            Assert.Equal(intExpectedDiffIndexes, actual.Item2);
        }


        [Fact]
        public void Diff_TwoDifferentValuesAtLeftAndRightOfDifferentLenght_ReturnsNotEqualSize()
        {
            var bytes1 = TestHelper.GetBytes("ABC");
            var bytes2 = TestHelper.GetBytes("AB");
            int id = 1;
            var entity = new DiffEntity(id, bytes1, bytes2);
            _mockRepository.Setup(x => x.GetById(id)).Returns(entity);

            var actual = _service.Diff(id);

            Assert.Equal(DiffResultType.NotEqualSize, actual.Item1);
            Assert.Empty(actual.Item2);
        }
    }
}
