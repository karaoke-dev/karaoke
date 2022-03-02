// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Cursor;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components.Description;
using osu.Game.Tests.Visual;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Components
{
    [TestFixture]
    public class TestSceneDescriptionTextFlowContainer : OsuTestScene
    {
        [Cached]
        private readonly OverlayColourProvider overlayColourProvider = new(OverlayColourScheme.Blue);

        private DescriptionTextFlowContainer descriptionTextFlowContainer;

        [SetUp]
        public void SetUp() => Schedule(() =>
        {
            Child = new PopoverContainer
            {
                RelativeSizeAxes = Axes.Both,
                Child = descriptionTextFlowContainer = new DescriptionTextFlowContainer
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
                descriptionTextFlowContainer.Description = new DescriptionFormat
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
                descriptionTextFlowContainer.Description = new DescriptionFormat
                {
                    Text = "Test description with [key](set_time)",
                    Keys = new Dictionary<string, InputKey>
                    {
                        {
                            "set_time", new InputKey
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
                descriptionTextFlowContainer.Description = new DescriptionFormat
                {
                    Text = "Test description with [key](set_time)",
                    Keys = new Dictionary<string, InputKey>
                    {
                        {
                            "set_time", new InputKey
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
}
