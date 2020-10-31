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
using Serilog;
using Xunit;
using static LanguageExt.Prelude;

namespace Test.Hestia.WebService
{
    public class FilesControllerTest
    {
        private const string FileId = "fileId";

        private static readonly IEnumerable<File> Files = new[]
        {
            new File(string.Empty,
                     1,
                     2,
                     3,
                     new List<Line>(),
                     "id",
                     null)
        };

        [Fact]
        public void GetFileDetailsByIdReturnsOkIfFileDetailsCouldBeFound()
        {
            var fileRetrieval = new Mock<IFileRetrieval>();
            var controller = new FilesController(fileRetrieval.Object, Mock.Of<ILogger>());
            fileRetrieval.Setup(mock => mock.GetFileDetails(It.IsAny<string>()))
                         .Returns(Some(Files.First().AsModel()));

            var result = controller.GetFileDetailsById(FileId);

            result.Result
                  .Should()
                  .BeOfType<OkObjectResult>();
            result.Result.As<OkObjectResult>()
                  .Value.As<IFileEntity>()
                  .Id
                  .Should()
                  .Be(Files.First().Id);
        }

        [Fact]
        public void GetFileDetailsByIdReturnsNotFoundIfFileDetailsWasNotFound()
        {
            var fileRetrieval = new Mock<IFileRetrieval>();
            var controller = new FilesController(fileRetrieval.Object, Mock.Of<ILogger>());
            fileRetrieval.Setup(mock => mock.GetFileDetails(It.IsAny<string>()))
                         .Returns(Option<IFileEntity>.None);

            var result = controller.GetFileDetailsById(FileId);

            result.Result
                  .Should()
                  .BeOfType<NotFoundResult>();
        }
    }
}
