// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.UI.Components;
using osu.Game.Skinning;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Tests.Skinning
{
    public class TestSceneColumnBackground : KaraokeSkinnableColumnTestScene
    {
        [BackgroundDependencyLoader]
        private void load()
        {
            SetContents(_ => new FillFlowContainer
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(0.8f),
                Direction = FillDirection.Vertical,
                Spacing = new Vector2(20),
                Children = new Drawable[]
                {
                    new NotePlayfieldTestContainer(0)
                    {
                        RelativeSizeAxes = Axes.Both,
                        Height = 0.5f,
                        Child = new SkinnableDrawable(new KaraokeSkinComponent(KaraokeSkinComponents.ColumnBackground), _ => new DefaultColumnBackground(0))
                        {
                            RelativeSizeAxes = Axes.Both
                        }
                    },
                    new NotePlayfieldTestContainer(1)
                    {
                        RelativeSizeAxes = Axes.Both,
                        Height = 0.5f,
                        Child = new SkinnableDrawable(new KaraokeSkinComponent(KaraokeSkinComponents.ColumnBackground), _ => new DefaultColumnBackground(1))
                        {
                            RelativeSizeAxes = Axes.Both
                        }
                    }
                }
            });
        }
    }
}
