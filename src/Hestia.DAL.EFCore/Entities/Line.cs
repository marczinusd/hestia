using JetBrains.Annotations;

namespace Hestia.DAL.EFCore.Entities
{
    public class Line
    {
        [UsedImplicitly]
        public Line()
        {
        }

        public Line(string content,
                    bool isCovered,
                    int numberOfAuthors,
                    int numberOfChanges,
                    string id,
                    int lineNumber,
                    string fileId,
                    string conditionCoverage,
                    bool isBranched,
                    int hitCount)
        {
            Content = content;
            IsCovered = isCovered;
            NumberOfAuthors = numberOfAuthors;
            NumberOfChanges = numberOfChanges;
            Id = id;
            LineNumber = lineNumber;
            FileId = fileId;
            ConditionCoverage = conditionCoverage;
            IsBranched = isBranched;
            HitCount = hitCount;
        }

        [UsedImplicitly] public string Id { get; set; }

        [UsedImplicitly] public int LineNumber { get; set; }

        [UsedImplicitly] public string Content { get; set; }

        [UsedImplicitly] public bool IsCovered { get; set; }

        [UsedImplicitly] public int NumberOfAuthors { get; set; }

        [UsedImplicitly] public int NumberOfChanges { get; set; }

        [UsedImplicitly] public File File { get; set; }

        [UsedImplicitly] public string FileId { get; set; }

        [UsedImplicitly] public string ConditionCoverage { get; set; }

        [UsedImplicitly] public bool IsBranched { get; set; }

        [UsedImplicitly] public int HitCount { get; set; }
    }
}
