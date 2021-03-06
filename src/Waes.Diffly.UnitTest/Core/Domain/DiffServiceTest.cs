﻿using Moq;
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
        public void Add_AddingLeftWhenRightAlreadyExists_CallsGetOrAddOnReposotory()
        {
            var entity = new DiffEntity(1, DiffSide.Right, value1Base64);
            _mockRepository.Setup(x => x.GetOrAdd(1, It.IsAny<DiffEntity>())).Returns(entity);
            _service.Add(1, DiffSide.Left, value1Base64);

            _mockRepository.Verify(x => x.GetOrAdd(1, It.IsAny<DiffEntity>()));
        }

        [Fact]
        public void Add_AddingLeftWhenLeftAlreadyExists_ThrowsException()
        {
            int id = 1;
            var entity = new DiffEntity(id, DiffSide.Left, value1Base64);
            _mockRepository.Setup(x => x.GetOrAdd(id, It.IsAny<DiffEntity>())).Returns(entity);
         
            Assert.Throws<DiffDomainException>(() => _service.Add(id, DiffSide.Left, value1Base64));
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
        [InlineData(126, 127, "3,1")]
        [InlineData(123456789, 127, "0,4")] //[7,91,205,21] vs [0,0,0,127]
        [InlineData(int.MaxValue, 2130706431, "0,1")] //[127,255,255,255] vs [126,255,255,255] 
        [InlineData(int.MaxValue, 2147418367, "2,1")] //[127,255,255,255] vs [127,255,0,255] 
        [InlineData(int.MaxValue, 2130771712, "1,1|3,1")] //[127,255,255,255] vs [127,0,255,0] 
        public void Diff_TwoDifferentValuesAtLeftAndRightOfEqualLenght_ReturnsNotEqualAndCorrectDiffIndexes(int i1, int i2, string expectedDiffIndexes)
        {
            // arrange
            int id = 1;
            var bytes1 = TestHelper.GetBytes(i1);
            var bytes2 = TestHelper.GetBytes(i2);
            var intExpectedDiffIndexes = TestHelper.FromTestStringToDiffDetail(expectedDiffIndexes);

            var entity = new DiffEntity(id, bytes1, bytes2);
            _mockRepository.Setup(x => x.GetById(id)).Returns(entity);

            // act
            var actual = _service.Diff(id);

            // assert
            Assert.Equal(DiffResultType.ContentDoNotMatch, actual.Item1);
            Assert.Equal(intExpectedDiffIndexes, actual.Item2);
        }

        [Fact]
        public void Diff_TwoDifferentValuesAtLeftAndRightOfEqualLenghtByteArrays_ReturnsNotEqualAndCorrectDiffIndexes()
        {
            // arrange
            int id = 1;
            var bytes1 = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x10, 0x10, 0x10, 0x10 };
            var bytes2 = new byte[] { 0x00, 0x10, 0x00, 0x10, 0x00, 0x10, 0x00, 0x00 };

            var entity = new DiffEntity(id, bytes1, bytes2);
            _mockRepository.Setup(x => x.GetById(id)).Returns(entity);

            var expectedDiffs = new[]
            {
                new DiffDetail(1,1),
                new DiffDetail(3,2),
                new DiffDetail(6,2)
            };

            // act
            var actual = _service.Diff(id);
            Assert.Equal(DiffResultType.ContentDoNotMatch, actual.Item1);
            Assert.True(expectedDiffs.SequenceEqual(actual.Item2));
        }

        [Fact]
        public void Diff_TwoDifferentValuesAtLeftAndRightOfDifferentLenght_ReturnsNotEqualSize()
        {
            var bytes1 = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x10, 0x10, 0x10, 0x10 };
            var bytes2 = new byte[] { 0x00, 0x10, 0x00, 0x10, 0x00, 0x10, 0x00 };
            int id = 1;
            var entity = new DiffEntity(id, bytes1, bytes2);
            _mockRepository.Setup(x => x.GetById(id)).Returns(entity);

            var actual = _service.Diff(id);

            Assert.Equal(DiffResultType.SizeDoNotMatch, actual.Item1);
            Assert.Empty(actual.Item2);
        }
    }
}
