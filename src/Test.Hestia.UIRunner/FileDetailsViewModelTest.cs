using FluentAssertions;
using Hestia.UIRunner.ViewModels;
using Xunit;

namespace Test.Hestia.UIRunner
{
    public class FileDetailsViewModelTest
    {
        [Fact]
        public void SmokeTest()
        {
            var vm = new FileDetailsViewModel();

            vm.Text
             .Should()
             .Contain("works");
        }
    }
}
