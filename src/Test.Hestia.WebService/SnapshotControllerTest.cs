using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Hestia.DAL.EFCore.Adapters;
using Hestia.DAL.EFCore.Entities;
using Hestia.DAL.Interfaces;
using Hestia.WebService.Controllers;
using LanguageExt;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using static LanguageExt.Prelude;

namespace Test.Hestia.WebService
{
    public class SnapshotControllerTest
    {
        private const string SnapshotId = "snapshotId";
        private const string FileId = "fileId";

        private static readonly Snapshot Snapshot =
            new Snapshot(new[]
                         {
                             new File(string.Empty,
                                      1,
                                      2,
                                      3,
                                      new List<Line>(),
                                      "id")
                         },
                         "hash",
                         DateTime.MinValue,
                         "name",
                         null!,
                         5,
                         1);

        [Fact]
        public void GetAllRepositories()
        {
            var snapshotRetrieval = new Mock<ISnapshotRetrieval>();
            var controller = new SnapshotsController(snapshotRetrieval.Object,
                                                     Mock.Of<IFileRetrieval>());

            controller.GetAllSnapshots();

            snapshotRetrieval.Verify(mock => mock.GetAllSnapshotsHeaders(), Times.Once);
        }

        [Fact]
        public void GetSnapshotByIdInvokesReturnOkIfSnapshotIsFound()
        {
            var snapshotRetrieval = new Mock<ISnapshotRetrieval>();
            snapshotRetrieval.Setup(mock => mock.GetSnapshotById(It.IsAny<string>()))
                             .Returns(Some(Snapshot.AsModel()));
            var controller = new SnapshotsController(snapshotRetrieval.Object,
                                                     Mock.Of<IFileRetrieval>());

            var result = controller.GetSnapshotById(SnapshotId);

            result.Result
                  .Should()
                  .BeOfType<OkObjectResult>();
        }

        [Fact]
        public void GetSnapshotByIdInvokesReturnNotFoundIfSnapshotIsNotFound()
        {
            var snapshotRetrieval = new Mock<ISnapshotRetrieval>();
            var controller = new SnapshotsController(snapshotRetrieval.Object,
                                                     Mock.Of<IFileRetrieval>());
            snapshotRetrieval.Setup(mock => mock.GetSnapshotById(It.IsAny<string>()))
                             .Returns(Option<IRepositorySnapshotEntity>.None);

            var result = controller.GetSnapshotById(SnapshotId);

            result.Result
                  .Should()
                  .BeOfType<NotFoundResult>();
        }

        [Fact]
        public void GetAllFileHeadersReturnsOkIfFileHeadersForSnapshotCouldBeRetrieved()
        {
            var snapshotRetrieval = new Mock<ISnapshotRetrieval>();
            var controller = new SnapshotsController(snapshotRetrieval.Object,
                                                     Mock.Of<IFileRetrieval>());
            snapshotRetrieval.Setup(mock => mock.GetSnapshotById(It.IsAny<string>()))
                             .Returns(Some(Snapshot.AsModel()));

            var result = controller.GetAllFileHeaders(SnapshotId);

            result.Result
                  .Should()
                  .BeOfType<OkObjectResult>();
        }

        [Fact]
        public void GetAllFileHeadersReturnsNotFoundIfSnapshotCouldNotBeFound()
        {
            var snapshotRetrieval = new Mock<ISnapshotRetrieval>();
            var controller = new SnapshotsController(snapshotRetrieval.Object,
                                                     Mock.Of<IFileRetrieval>());
            snapshotRetrieval.Setup(mock => mock.GetSnapshotById(It.IsAny<string>()))
                             .Returns(Option<IRepositorySnapshotEntity>.None);

            var result = controller.GetSnapshotById(SnapshotId);

            result.Result
                  .Should()
                  .BeOfType<NotFoundResult>();
        }

        [Fact]
        public void GetFileDetailsByIdReturnsOkIfFileDetailsCouldBeFound()
        {
            var fileRetrieval = new Mock<IFileRetrieval>();
            var controller = new SnapshotsController(Mock.Of<ISnapshotRetrieval>(),
                                                     fileRetrieval.Object);
            fileRetrieval.Setup(mock => mock.GetFileDetails(It.IsAny<string>(), It.IsAny<string>()))
                         .Returns(Some(Snapshot.Files.First()
                                               .AsModel()));

            var result = controller.GetFileDetailsById(SnapshotId, FileId);

            result.Result
                  .Should()
                  .BeOfType<OkObjectResult>();
            result.Result.As<OkObjectResult>()
                  .Value.As<IFileEntity>()
                  .Id
                  .Should()
                  .Be(Snapshot.Files.First()
                              .Id);
        }

        [Fact]
        public void GetFileDetailsByIdReturnsNotFoundIfFileDetailsWasNotFound()
        {
            var fileRetrieval = new Mock<IFileRetrieval>();
            var controller = new SnapshotsController(Mock.Of<ISnapshotRetrieval>(),
                                                     fileRetrieval.Object);
            fileRetrieval.Setup(mock => mock.GetFileDetails(It.IsAny<string>(), It.IsAny<string>()))
                         .Returns(Option<IFileEntity>.None);

            var result = controller.GetFileDetailsById(SnapshotId, FileId);

            result.Result
                  .Should()
                  .BeOfType<NotFoundResult>();
        }
    }
}
