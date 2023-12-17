// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Issues;
using osu.Game.Rulesets.Karaoke.Tests.Helper;
using osu.Game.Tests.Visual;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Tests.Screens.Edit.Components.Issues;

[TestFixture]
public partial class TestSceneIssueIcon : OsuTestScene
{
    private IssueIcon icon = null!;

    [SetUp]
    public void SetUp() => Schedule(() =>
    {
        Child = icon = new IssueIcon
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            Size = new Vector2(64),
        };
    });

    [Test]
    public void DisplayIconByIssues()
    {
        var availableIssues = TestCaseCheckHelper.CreateAllAvailableIssues();

        foreach (var (check, issues) in availableIssues)
        {
            AddLabel($"Check: {check.Metadata.Description}");

            foreach (var issue in issues)
            {
                AddStep($"Test lyric with template {issue.Template.UnformattedMessage}", () =>
                {
                    icon.Issue = issue;
                });
            }
        }
    }
}
