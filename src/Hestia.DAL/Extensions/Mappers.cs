using System.Linq;
using Hestia.DAL.Entities;
using Hestia.Model;
using Hestia.Model.Stats;

namespace Hestia.DAL.Extensions
{
    public static class Mappers
    {
        public static Repository MapEntityToModel(this RepositoryEntity entity)
        {
            return new Repository(entity.Id,
                                  entity.Name,
                                  entity.RootDirectory.MapEntityToModel(),
                                  entity.PathToCoverageResultFile);
        }

        public static Directory MapEntityToModel(this DirectoryEntity entity)
        {
            return new Directory(entity.Name,
                                 entity.Path,
                                 entity.Directories.Select(d => d.MapEntityToModel()),
                                 entity.Files.Select(f => f.MapEntityToModel()));
        }

        public static File MapEntityToModel(this FileEntity entity) =>
            new File(entity.Id,
                     entity.Filename,
                     entity.Extension,
                     entity.Path,
                     entity.Content.Select(l => l.MapEntityToModel()),
                     entity.GitStats.MapEntityToModel(),
                     entity.CoverageStats.MapEntityToModel());

        public static FileGitStats MapEntityToModel(this GitStatsEntity entity) =>
            new FileGitStats(entity.LifetimeChanges);

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
