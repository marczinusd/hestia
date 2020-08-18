using FluentAssertions;
using Hestia.UIRunner.ViewModels;
using Microsoft.Reactive.Testing;
using Test.Hestia.Utils;
using Test.Hestia.Utils.TestData;
using Xunit;

namespace Test.Hestia.UIRunner.ViewModel
{
    public class FileDetailsViewModelTest
    {
        [Fact]
        public void FileOnViewModelShouldReflectLatestFilePublishedOnObservable()
        {
            var scheduler = new TestScheduler();
            var file = MockRepo.CreateFile();
            var vm = new FileDetailsViewModel(scheduler.CreateColdObservable(file.AsNotification()));

            scheduler.Start();

            vm.File
              .Should()
              .BeSameAs(file);
        }
    }
}
