// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Edit.Components.Cursor;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor
{
    [TestFixture]
    public class TestSceneInvalidLyricToolTip : OsuTestScene
    {
        private InvalidLyricToolTip toolTip = null!;

        [SetUp]
        public void SetUp() => Schedule(() =>
        {
            Child = toolTip = new InvalidLyricToolTip
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre
            };
            toolTip.Show();
        });

        [Test]
        public void TestValidLyric()
        {
            setTooltip("valid lyric");
        }

        private void setTooltip(string testName, params Issue[] issues)
        {
            AddStep(testName, () =>
            {
                toolTip.SetContent(issues);
            });
        }

        internal class Check : ICheck
        {
            public IEnumerable<Issue> Run(BeatmapVerifierContext context)
            {
                throw new NotImplementedException();
            }

            public CheckMetadata Metadata { get; } = null!;

            public IEnumerable<IssueTemplate> PossibleTemplates { get; } = null!;
        }

        internal class TestIssueTemplate : IssueTemplate
        {
            public TestIssueTemplate()
                : base(new Check(), IssueType.Error, string.Empty)
            {
            }
        }
    }
}
