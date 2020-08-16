using System;
using FluentAssertions;
using Hestia.DAL.EFCore;
using Hestia.Model.Interfaces;
using Moq;
using Xunit;

namespace Test.Hestia.DAL.EFCore
{
    public class SnapshotEFClientTest
    {
        [Fact]
        public void InsertSnapshotShouldThrowNotImplementedException()
        {
            var client = new SnapshotEFClient();

            Action act = () => client.InsertSnapshot(Mock.Of<IRepositorySnapshot>());

            act.Should()
               .Throw<NotImplementedException>();
        }

        [Fact]
        public void GetFileDetailsShouldThrowNotImplementedException()
        {
            var client = new SnapshotEFClient();

            Action act = () => client.GetFileDetails(string.Empty, string.Empty);

            act.Should()
               .Throw<NotImplementedException>();
        }

        [Fact]
        public void GetAllSnapshotsHeadersShouldThrowNotImplementedException()
        {
            var client = new SnapshotEFClient();

            Action act = () => client.GetAllSnapshotsHeaders();

            act.Should()
               .Throw<NotImplementedException>();
        }

        [Fact]
        public void GetSnapshotByIdShouldThrowNotImplementedException()
        {
            var client = new SnapshotEFClient();

            Action act = () => client.GetSnapshotById(string.Empty);

            act.Should()
               .Throw<NotImplementedException>();
        }
    }
}
