using DynamicData.Binding;
using Hestia.Model;
using Hestia.Model.Stats;
using LanguageExt;

namespace Hestia.UIRunner.ViewModels
{
    public class FileDetailsViewModel : ViewModelBase
    {
        public FileDetailsViewModel()
        {
            Lines = new ObservableCollectionExtended<SourceLine>(new[]
            {
                new SourceLine(1,
                               "bla",
                               Option<LineCoverageStats>.None,
                               Option<LineGitStats>.None),
                new SourceLine(2,
                               "bla",
                               Option<LineCoverageStats>.None,
                               Option<LineGitStats>.None),
                new SourceLine(3,
                               "bla",
                               Option<LineCoverageStats>.None,
                               Option<LineGitStats>.None),
                new SourceLine(4,
                               "bla",
                               Option<LineCoverageStats>.None,
                               Option<LineGitStats>.None),
                new SourceLine(5,
                               "bla",
                               Option<LineCoverageStats>.None,
                               Option<LineGitStats>.None),
                new SourceLine(6,
                               "bla",
                               Option<LineCoverageStats>.None,
                               Option<LineGitStats>.None),
            });
        }

        public string Text => "File details works";

        public IObservableCollection<SourceLine> Lines
        {
            get;
            set;
        }
    }
}
