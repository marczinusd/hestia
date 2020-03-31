using System.Collections.Generic;
using System.Collections.ObjectModel;
using Hestia.Model;
using Hestia.Model.Stats;
using LanguageExt;

namespace Hestia.UIRunner.ViewModels
{
    public class RepositoryViewModel : ViewModelBase
    {
        public RepositoryViewModel()
        {
            Files = new ObservableCollection<File>(GenerateMockPeopleTable());
        }

        public string Text => "Repository view works";

        public ObservableCollection<File> Files { get; set; }

        private IEnumerable<File> GenerateMockPeopleTable()
        {
            var defaultPeople = new List<File>()
            {
                new File("bla", "cs", "cs", new List<SourceLine>(), Option<FileGitStats>.None, Option<FileCoverageStats>.None),
                new File("bla", "cs", "cs", new List<SourceLine>(), Option<FileGitStats>.None, Option<FileCoverageStats>.None),
                new File("bla", "cs", "cs", new List<SourceLine>(), Option<FileGitStats>.None, Option<FileCoverageStats>.None),
                new File("bla", "cs", "cs", new List<SourceLine>(), Option<FileGitStats>.None, Option<FileCoverageStats>.None),
                new File("bla", "cs", "cs", new List<SourceLine>(), Option<FileGitStats>.None, Option<FileCoverageStats>.None),
                new File("bla", "cs", "cs", new List<SourceLine>(), Option<FileGitStats>.None, Option<FileCoverageStats>.None),
                new File("bla", "cs", "cs", new List<SourceLine>(), Option<FileGitStats>.None, Option<FileCoverageStats>.None),
                new File("bla", "cs", "cs", new List<SourceLine>(), Option<FileGitStats>.None, Option<FileCoverageStats>.None),
            };

            return defaultPeople;
        }
    }
}
