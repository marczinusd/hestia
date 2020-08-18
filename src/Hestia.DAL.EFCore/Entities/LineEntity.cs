using JetBrains.Annotations;

namespace Hestia.DAL.EFCore.Entities
{
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class LineEntity
    {
        [UsedImplicitly]
        public LineEntity()
        {
        }

        public LineEntity(string content,
                          bool isCovered,
                          int numberOfAuthors,
                          int numberOfChanges,
                          string id,
                          int lineNumber)
        {
            Content = content;
            IsCovered = isCovered;
            NumberOfAuthors = numberOfAuthors;
            NumberOfChanges = numberOfChanges;
            Id = id;
            LineNumber = lineNumber;
        }

        [UsedImplicitly] public string Id { get; set; }

        [UsedImplicitly] public int LineNumber { get; set; }

        [UsedImplicitly] public string Content { get; set; }

        [UsedImplicitly] public bool IsCovered { get; set; }

        [UsedImplicitly] public int NumberOfAuthors { get; set; }

        [UsedImplicitly] public int NumberOfChanges { get; set; }

        [UsedImplicitly] public virtual FileEntity Parent { get; set; }
    }
}
