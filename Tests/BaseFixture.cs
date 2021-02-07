using System;
using Moq;
using Application.AutoMapper;

namespace Tests
{
    public class BaseFixture : IDisposable
    {
        public Mock<T> GetServiceMock<T>(params object[] values) where T : class
        {
            return new Mock<T>(values);
        }

        public BaseFixture()
        {
            ConfigureMap.Configure();
        }

        public void Dispose()
        {
        }
    }
}
