using System.Linq;
using Hestia.DAL.Entities;
using Hestia.Model;
using Hestia.Model.Stats;

namespace Hestia.DAL.Extensions
{
    public static class Mappers
    {
        public static Repository MapEntityToModel(this RepositoryEntity entity) =>
            new Repository(entity.Id,
                           entity.Name,
                           entity.RootDirectory.MapEntityToModel(),
                           entity.PathToCoverageResultFile);

        public static Directory MapEntityToModel(this DirectoryEntity entity) =>
            new Directory(entity.Name,
                          entity.Path,
                          entity.Directories?.Select(d => d.MapEntityToModel()).ToList() ?? Enumerable.Empty<Directory>().ToList(),
                          entity.Files?.Select(f => f.MapEntityToModel()).ToList() ?? Enumerable.Empty<File>().ToList());

        public static File MapEntityToModel(this FileEntity entity) =>
            new File(entity.Id,
                     entity.Filename,
                     entity.Extension,
                     entity.Path,
                     entity.Content?.Select(l => l.MapEntityToModel()).ToList() ?? Enumerable.Empty<SourceLine>().ToList(),
                     entity.GitStats.MapEntityToModel(),
                     entity.CoverageStats.MapEntityToModel());

        public static FileGitStats MapEntityToModel(this GitStatsEntity entity) =>
            new FileGitStats(entity.LifetimeChanges, entity.LifetimeAuthors);

        public static FileCoverageStats MapEntityToModel(this CoverageEntity entity) =>
            new FileCoverageStats(entity.PercentageOfLineCoverage);

        public static SourceLine MapEntityToModel(this LineEntity entity) =>
            new SourceLine(entity.LineNumber,
                           entity.Text,
                           entity.LineCoverageStats.MapEntityToModel(),
                           entity.LineGitStats.MapEntityToModel());

        public static LineGitStats MapEntityToModel(this LineGitStatsEntity entity) =>
            new LineGitStats(entity.ModifiedInNumberOfCommits);

        public static LineCoverageStats MapEntityToModel(this LineCoverageEntity entity) =>
            new LineCoverageStats(entity.IsCovered);
    }
}
