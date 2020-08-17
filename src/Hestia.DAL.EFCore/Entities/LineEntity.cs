using Hestia.DAL.Interfaces;

namespace Hestia.DAL.EFCore.Entities
{
    public class LineEntity : ISourceLineEntity
    {
        public LineEntity(string content, bool isCovered, int numberOfAuthors, int numberOfChanges)
        {
            Content = content;
            IsCovered = isCovered;
            NumberOfAuthors = numberOfAuthors;
            NumberOfChanges = numberOfChanges;
        }

        public string Content { get; }

        public bool IsCovered { get; }

        public int NumberOfAuthors { get; }

        public int NumberOfChanges { get; }
    }
}
