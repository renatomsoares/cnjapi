using System;
using Application.Controllers;
using Domain.DTO;
using Domain.Entities;
using Infra.UnitOfWork;
using Moq;
using Services;
namespace Tests
{
    public class LawsuitControllerFixture : BaseFixture, IDisposable
    {
        public Mock<LawsuitService> ServiceMock { get; set; }
        public LawsuitController Controller { get; set; }
        public IUnitOfWork uow;

        public LawsuitControllerFixture()
        {

            ServiceMock = GetServiceMock<LawsuitService>(uow);

            Controller = new LawsuitController();
        }

        public void Dispose() { }
    }
}
