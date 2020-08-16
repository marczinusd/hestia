using System;
using System.Collections.Generic;
using FluentAssertions;
using Hestia.DAL.Interfaces;
using Hestia.Model;
using Hestia.Model.Interfaces;
using Hestia.UIRunner.ViewModels;
using LanguageExt;
using Microsoft.Reactive.Testing;
using Moq;
using Test.Hestia.Utils;
using Test.Hestia.Utils.TestData;
using Xunit;

namespace Test.Hestia.UIRunner
{
    public class RepositoryViewModelTest
    {
        [Fact]
        public void FilesShouldChangeBasedOnSnapshotPublishedOnObservable()
        {
            var scheduler = new TestScheduler();
            var snapshot = new RepositorySnapshot(string.Empty,
                                                  new List<IFile> { MockRepo.CreateFile(), MockRepo.CreateFile() },
                                                  Option<string>.None,
                                                  Option<string>.None,
                                                  Option<DateTime>.None,
                                                  Option<string>.None);
            var vm = new RepositoryViewModel(scheduler.CreateColdObservable(snapshot.AsNotification()),
                                             Mock.Of<ISnapshotPersistence>());

            scheduler.Start();

            vm.Repository
              .Files
              .Should()
              .BeEquivalentTo(snapshot.Files);
        }

        [Fact]
        public void ExecutingCommitToDatabaseCommandInvokesSnapshotPersistor()
        {
            var scheduler = new TestScheduler();
            var snapshot = new RepositorySnapshot(string.Empty,
                                                  new List<IFile> { MockRepo.CreateFile(), MockRepo.CreateFile() },
                                                  Option<string>.None,
                                                  Option<string>.None,
                                                  Option<DateTime>.None,
                                                  Option<string>.None);
            var snapshotPersistence = new Mock<ISnapshotPersistence>();
            var vm = new RepositoryViewModel(scheduler.CreateColdObservable(snapshot.AsNotification()),
                                             snapshotPersistence.Object);
            scheduler.Start();

            vm.CommitToDatabaseCommand.Execute();

            snapshotPersistence.Verify(mock =>
                                           mock.InsertSnapshot(It.Is<IRepositorySnapshot>(x => x.Equals(snapshot))), Times.Once);
        }
    }
}
