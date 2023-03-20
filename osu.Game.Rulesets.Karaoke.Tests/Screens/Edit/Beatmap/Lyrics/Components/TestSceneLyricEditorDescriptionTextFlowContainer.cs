// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Cursor;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Components.Markdown;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Markdown;
using osu.Game.Tests.Visual;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Tests.Screens.Edit.Beatmap.Lyrics.Components;

[TestFixture]
public partial class TestSceneDescriptionTextFlowContainer : OsuTestScene
{
    [Cached]
    private readonly OverlayColourProvider overlayColourProvider = new(OverlayColourScheme.Blue);

    private LyricEditorDescriptionTextFlowContainer lyricEditorDescriptionTextFlowContainer = null!;

    [SetUp]
    public void SetUp() => Schedule(() =>
    {
        Child = new PopoverContainer
        {
            RelativeSizeAxes = Axes.Both,
            Child = lyricEditorDescriptionTextFlowContainer = new LyricEditorDescriptionTextFlowContainer
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(100)
            }
        };
    });

    [Test]
    public void TestDisplayDescriptionWithEditMode()
    {
        AddStep("Markdown description", () =>
        {
            lyricEditorDescriptionTextFlowContainer.Description = new DescriptionFormat
            {
                Text = $"Test description with [{DescriptionFormat.LINK_KEY_ACTION}](singer_mode)",
                Actions = new Dictionary<string, IDescriptionAction>
                {
                    {
                        "singer_mode", new SwitchModeDescriptionAction
                        {
                            Text = "edit text mode",
                            Mode = LyricEditorMode.Singer
                        }
                    }
                }
            };
        });
    }
}
