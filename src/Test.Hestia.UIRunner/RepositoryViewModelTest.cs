using System;
using System.Collections.Generic;
using FluentAssertions;
using Hestia.DAL.Mongo;
using Hestia.Model;
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
                                                  new List<File> { MockRepo.CreateFile(), MockRepo.CreateFile() },
                                                  Option<string>.None,
                                                  Option<string>.None,
                                                  Option<DateTime>.None,
                                                  Option<string>.None);
            var vm = new RepositoryViewModel(scheduler.CreateColdObservable(snapshot.AsNotification()), Mock.Of<ISnapshotPersistence>());

            scheduler.Start();

            vm.Repository
              .Files
              .Should()
              .BeEquivalentTo(snapshot.Files);
        }
    }
}
