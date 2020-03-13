using System;
using Hestia.DAL.Mongo;
using Hestia.DAL.Mongo.Model;
using Hestia.DAL.Mongo.Wrappers;
using Hestia.Model;
using LanguageExt;
using Microsoft.Reactive.Testing;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace Test.Hestia.DAL.Mongo
{
    public class HestiaMongoClientTest
    {
        // TODO fill in details while implementing
        [Fact]
        public void SmokeTest()
        {
            var scheduler = new TestScheduler();
            var clientFactoryMock = new Mock<IMongoClientFactory>();
            var clientMock = new Mock<IMongoClient>();
            var dbMock = new Mock<IMongoDatabase>();
            var collectionMock = new Mock<IMongoCollection<RepositoryEntity>>();
            var collectionWrapperMock = new Mock<IMongoCollectionWrapper<RepositoryEntity>>();
            dbMock.Setup(mock => mock.GetCollection<RepositoryEntity>(It.IsAny<string>(),
                                                                      It.IsAny<MongoCollectionSettings>()))
                  .Returns(collectionMock.Object);
            clientMock.Setup(mock => mock.GetDatabase(It.IsAny<string>(), It.IsAny<MongoDatabaseSettings>()))
                      .Returns(dbMock.Object);
            clientFactoryMock.Setup(mock => mock.CreateClient(It.IsAny<string>()))
                             .Returns(clientMock.Object);

            // ReSharper disable once UnusedVariable
            var hestiaMongoClient = new HestiaMongoClient(clientFactoryMock.Object,
                                                          c => collectionWrapperMock.Object,
                                                          string.Empty,
                                                          string.Empty,
                                                          string.Empty);

            scheduler.Start(() => hestiaMongoClient.GetAllRepos());
            scheduler.Start(() => hestiaMongoClient.GetRepoById("1"));
            hestiaMongoClient.AddRepository(new Repository(1,
                                                           string.Empty,
                                                           Option<RepositorySnapshot[]>.None,
                                                           Option<string>.None,
                                                           Option<string>.None));

            collectionWrapperMock.Verify(mock => mock.Find(It.IsAny<Func<RepositoryEntity, bool>>()), Times.Exactly(2));
            collectionWrapperMock.Verify(mock => mock.InsertOne(It.IsAny<RepositoryEntity>()), Times.Once);
        }
    }
}
