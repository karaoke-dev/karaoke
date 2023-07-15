// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Issues;
using osu.Game.Rulesets.Karaoke.Tests.Helper;
using osu.Game.Tests.Visual;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Tests.Screens.Edit.Components.Issues;

[TestFixture]
public partial class TestSceneIssueIcon : OsuTestScene
{
    private SpriteIcon icon = null!;

    [SetUp]
    public void SetUp() => Schedule(() =>
    {
        Child = icon = new SpriteIcon
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            Size = new Vector2(64),
        };
    });

    [Test]
    public void DisplayIconByIssueTemplate()
    {
        var availableChecks = TestCaseCheckHelper.GetAllAvailableChecks();

        foreach (var check in availableChecks)
        {
            AddLabel($"Check: {check.Metadata.Description}");

            foreach (var template in check.PossibleTemplates)
            {
                AddStep($"Test lyric with template {template.UnformattedMessage}", () =>
                {
                    icon.Icon = IssueIcon.GetIconByIssueTemplate(template);
                });
            }
        }
    }
}
