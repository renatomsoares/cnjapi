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

        private void NotEmptyConfiguration()
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
            
            //_fixture.ServiceMock
            //    .Setup(c => c
            //       .GetAll(
            //           It.IsAny<Expression<Func<Lawsuit, bool>>>(),
            //           It.IsAny<Func<IQueryable<Lawsuit>, IOrderedQueryable<Lawsuit>>>(),
            //           It.IsAny<Func<IQueryable<Lawsuit>, IIncludableQueryable<Lawsuit, object>>>(),
            //           It.IsAny<int>(),
            //           It.IsAny<int>(),
            //           It.IsAny<bool>()
            //       ))
            //    .Returns(new List<Lawsuit>() { lawsuit });
        }

        [Fact]
        public void GetByIdOkObjectResult()
        {
            NotEmptyConfiguration();

            var resultado = _fixture.Controller.GetById(_fixture.ServiceMock.Object, 1);

            Assert.IsType<OkObjectResult>(resultado);
        }
    }
}
