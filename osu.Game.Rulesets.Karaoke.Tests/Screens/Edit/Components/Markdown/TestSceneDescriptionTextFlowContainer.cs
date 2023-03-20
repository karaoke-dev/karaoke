// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Cursor;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Markdown;
using osu.Game.Tests.Visual;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Tests.Screens.Edit.Components.Markdown;

[TestFixture]
public partial class TestSceneDescriptionTextFlowContainer : OsuTestScene
{
    [Cached]
    private readonly OverlayColourProvider overlayColourProvider = new(OverlayColourScheme.Blue);

    private DescriptionTextFlowContainer lyricEditorDescriptionTextFlowContainer = null!;

    [SetUp]
    public void SetUp() => Schedule(() =>
    {
        Child = new PopoverContainer
        {
            RelativeSizeAxes = Axes.Both,
            Child = lyricEditorDescriptionTextFlowContainer = new DescriptionTextFlowContainer
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(100)
            }
        };
    });

    [Test]
    public void TestDisplayDescription()
    {
        AddStep("Markdown description", () =>
        {
            lyricEditorDescriptionTextFlowContainer.Description = new DescriptionFormat
            {
                Text = "Test description with `Markdown` format."
            };
        });
    }

    [Test]
    public void TestDisplayDescriptionWithKey()
    {
        AddStep("Markdown description with key", () =>
        {
            lyricEditorDescriptionTextFlowContainer.Description = new DescriptionFormat
            {
                Text = $"Test description with [{DescriptionFormat.LINK_KEY_ACTION}](set_time)",
                Actions = new Dictionary<string, IDescriptionAction>
                {
                    {
                        "set_time", new InputKeyDescriptionAction
                        {
                            AdjustableActions = new[]
                            {
                                KaraokeEditAction.SetTime
                            }
                        }
                    }
                }
            };
        });

        AddStep("Markdown description with key text and tooltip", () =>
        {
            lyricEditorDescriptionTextFlowContainer.Description = new DescriptionFormat
            {
                Text = $"Test description with [{DescriptionFormat.LINK_KEY_ACTION}](set_time)",
                Actions = new Dictionary<string, IDescriptionAction>
                {
                    {
                        "set_time", new InputKeyDescriptionAction
                        {
                            Text = "set time key.",
                            AdjustableActions = new[]
                            {
                                KaraokeEditAction.SetTime
                            }
                        }
                    }
                }
            };
        });
    }
}
