using FluentAssertions;
using Hestia.UIRunner.ViewModels;
using Xunit;

namespace Test.Hestia.UIRunner
{
    public class RepositoryViewModelTest
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
