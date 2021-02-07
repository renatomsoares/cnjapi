using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Domain.DTO;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Services;
using Xunit;


namespace Tests
{
    [Collection("Shared Context")]
    public class LawsuitControllerTest : IClassFixture<LawsuitControllerFixture>
    {
        readonly LawsuitControllerFixture _fixture;

        public LawsuitControllerTest(LawsuitControllerFixture fixture)
        {
            _fixture = fixture;
        }

        private void ConfigureEmptyGetAll()
        {
            _fixture.ServiceMock
                .Setup(c => c
                   .GetAll(
                       It.IsAny<Expression<Func<Lawsuit, bool>>>(),
                       It.IsAny<Func<IQueryable<Lawsuit>, IOrderedQueryable<Lawsuit>>>(),
                       It.IsAny<Func<IQueryable<Lawsuit>, IIncludableQueryable<Lawsuit, object>>>(),
                       It.IsAny<int>(),
                       It.IsAny<int>(),
                       It.IsAny<bool>()
                   ))
                .Returns(Enumerable.Empty<Lawsuit>());
        }

        private void NotEmptyGetByIdConfigurationCreatingTheFirstRegister()
        {
            Lawsuit lawsuit = new Lawsuit
            {
                IdLawsuit = 1,
                CaseNumber = "5454887-54.5455.2.44.5555",
                CourtName = "CNJ",
                ResponsibleName = "Renato Mesquita",
                RegistrationDate = DateTime.Now
            };

            _fixture.ServiceMock.Setup(x => x.GetById(It.IsAny<Expression<Func<Lawsuit, bool>>>())).Returns(lawsuit);
        }

        private void EmptyGetByIdConfiguration()
        {
            _fixture.ServiceMock.Setup(x => x.GetById(It.IsAny<Expression<Func<Lawsuit, bool>>>())).Returns((Lawsuit)null);
        }

        [Fact]
        public void GetByIdWithNotFoundObjectResult()
        {
            EmptyGetByIdConfiguration();

            var result = _fixture.Controller.GetById(_fixture.ServiceMock.Object, 1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetByIdWithOkObjectResult()
        {
            NotEmptyGetByIdConfigurationCreatingTheFirstRegister();

            var result = _fixture.Controller.GetById(_fixture.ServiceMock.Object, 1);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void CreateWithInvalidCaseNumberFormat()
        {
            LawsuitDTO lawsuitDto = new LawsuitDTO
            {
                CaseNumber = "123-12.12.1.12.123",
                CourtName = "CNJ",
                ResponsibleName = "Renato Mesquita",
            };

            var result = _fixture.Controller.Create(_fixture.ServiceMock.Object, lawsuitDto);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void CreateWithWithTheSameCaseNumber()
        {
            LawsuitDTO lawsuitDto1 = new LawsuitDTO
            {
                CaseNumber = "1234567-12.1234.1.12.1234",
                CourtName = "CNJ",
                ResponsibleName = "Renato Mesquita",
            };

            LawsuitDTO lawsuitDto2 = new LawsuitDTO
            {
                CaseNumber = "1234567-12.1234.1.12.1234",
                CourtName = "TJ",
                ResponsibleName = "Renato Mesquita 2",
            };

            _fixture.Controller.Create(_fixture.ServiceMock.Object, lawsuitDto1);
            var result = _fixture.Controller.Create(_fixture.ServiceMock.Object, lawsuitDto2);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void DeleteWithNotFoundResult()
        {
            var result = _fixture.Controller.Delete(_fixture.ServiceMock.Object, 555);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void DeleteWithOkResult()
        {
            NotEmptyGetByIdConfigurationCreatingTheFirstRegister();

            var result = _fixture.Controller.Delete(_fixture.ServiceMock.Object, 1);

            Assert.IsType<OkResult>(result);
        }
    }
}
