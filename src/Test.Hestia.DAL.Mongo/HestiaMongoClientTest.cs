using Hestia.DAL.Mongo;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace Test.Hestia.DAL.Mongo
{
    public class HestiaMongoClientTest
    {
        [Fact]
        public void SmokeTest()
        {
            var clientFactoryMock = new Mock<IMongoClientFactory>();
            var clientMock = new Mock<IMongoClient>();
            clientMock.Setup(mock => mock.GetDatabase(It.IsAny<string>(), It.IsAny<MongoDatabaseSettings>()))
                      .Returns(Mock.Of<IMongoDatabase>());
            clientFactoryMock.Setup(mock => mock.CreateClient(It.IsAny<string>()))
                             .Returns(clientMock.Object);

            // ReSharper disable once UnusedVariable
            var hestiaMongoClient = new HestiaMongoClient(clientFactoryMock.Object,
                                                          string.Empty,
                                                          string.Empty,
                                                          string.Empty);
        }
    }
}
