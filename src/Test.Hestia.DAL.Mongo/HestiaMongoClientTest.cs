using System;
using System.Collections.Generic;
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
            var collectionMock = new Mock<IMongoCollection<RepositorySnapshotEntity>>();
            var collectionWrapperMock = new Mock<IMongoCollectionWrapper<RepositorySnapshotEntity>>();
            dbMock.Setup(mock => mock.GetCollection<RepositorySnapshotEntity>(It.IsAny<string>(),
                                                                              It.IsAny<MongoCollectionSettings>()))
                  .Returns(collectionMock.Object);
            clientMock.Setup(mock => mock.GetDatabase(It.IsAny<string>(), It.IsAny<MongoDatabaseSettings>()))
                      .Returns(dbMock.Object);
            clientFactoryMock.Setup(mock => mock.CreateClient())
                             .Returns(clientMock.Object);

            // ReSharper disable once UnusedVariable
            var hestiaMongoClient = new SnapshotMongoClient(clientFactoryMock.Object,
                                                                  c => collectionWrapperMock.Object,
                                                                  string.Empty);

            scheduler.Start(() => hestiaMongoClient.GetAllSnapshots());
            scheduler.Start(() => hestiaMongoClient.GetSnapshotById("1"));
            hestiaMongoClient.InsertSnapshot(new RepositorySnapshot(1,
                                                                    new List<File>(),
                                                                    Option<string>.None,
                                                                    Option<string>.None,
                                                                    Option<DateTime>.None));

            collectionWrapperMock.Verify(mock => mock.Find(It.IsAny<Func<RepositorySnapshotEntity, bool>>()),
                                         Times.Exactly(2));
            collectionWrapperMock.Verify(mock => mock.InsertOne(It.IsAny<RepositorySnapshotEntity>()), Times.Once);
        }
    }
}
