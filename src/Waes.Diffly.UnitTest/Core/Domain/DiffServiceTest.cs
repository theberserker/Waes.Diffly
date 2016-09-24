using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Waes.Diffly.Core.Domain;
using Waes.Diffly.Core.Domain.Entities;
using Waes.Diffly.Core.Domain.Enums;
using Waes.Diffly.Core.Domain.Helpers;
using Waes.Diffly.Core.Exceptions;
using Waes.Diffly.Core.Interfaces.Domain;
using Waes.Diffly.Core.Interfaces.Repositories;
using Xunit;

namespace Waes.Diffly.UnitTest.Core.Domain
{
    public class DiffServiceTest
    {
        private static string value1 = Convert.ToString(192, 2); // 11000000 (0xc0)
        private static string value1Base64 = StringHelper.ToAsciiBase64(value1);

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
            _mockRepository.Setup(x => x.Add(It.IsAny<DiffEntity>()));
            _service.Add(1, DiffSide.Left, value1);

            _mockRepository.Verify(x => x.Add(It.IsAny<DiffEntity>()));
        }

        [Fact]
        public void Add_AddingIdAndSideThatAlreadyExists_ThrowsException()
        {
            int id = 1;
            var entity = new DiffEntity(id, DiffSide.Left, value1);
            
            _mockRepository.Setup(x => x.GetById(id)).Returns(entity);
            _mockRepository.Setup(x => x.Add(It.IsAny<DiffEntity>()));

            Assert.Throws<DiffDomainException>(() => _service.Add(id, DiffSide.Left, value1));
        }

    }
}
