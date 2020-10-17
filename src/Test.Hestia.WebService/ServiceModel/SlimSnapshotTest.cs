using System;
using System.Collections.Generic;
using FluentAssertions;
using Hestia.Model;
using Hestia.Model.Interfaces;
using Hestia.WebService.ServiceModel;
using LanguageExt;
using Xunit;

namespace Test.Hestia.WebService.ServiceModel
{
    public class SlimSnapshotTest
    {
        [Fact]
        public void FromCreatesCorrectSlimSnapshotRepresentation()
        {
            var snapshot = new RepositorySnapshot("bla",
                                                  "path",
                                                  new List<IFile>(),
                                                  "bla",
                                                  Option<string>.None,
                                                  "123",
                                                  DateTime.MinValue,
                                                  0,
                                                  0).AsHeader();

            var slim = SlimSnapshot.From(snapshot);

            slim.Id
                .Should()
                .Be(snapshot.Id);
            slim.Name
                .Should()
                .Be(snapshot.Name);
            slim.AtHash
                .Should()
                .Be(snapshot.AtHash);
            slim.CommitDate
                .Should()
                .Be(snapshot.CommitDate);
        }
    }
}
