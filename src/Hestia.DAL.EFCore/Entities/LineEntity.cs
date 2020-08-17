using JetBrains.Annotations;

namespace Hestia.DAL.EFCore.Entities
{
    public class LineEntity
    {
        [UsedImplicitly]
        public LineEntity()
        {
        }

        public LineEntity(string content, bool isCovered, int numberOfAuthors, int numberOfChanges, string id)
        {
            Content = content;
            IsCovered = isCovered;
            NumberOfAuthors = numberOfAuthors;
            NumberOfChanges = numberOfChanges;
            Id = id;
        }

        public string Id { get; set; }

        public string Content { get; set; }

        public bool IsCovered { get; set; }

        public int NumberOfAuthors { get; set; }

        public int NumberOfChanges { get; set; }

        public virtual FileEntity Parent { get; set; }
    }
}
