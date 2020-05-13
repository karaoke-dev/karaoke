// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.UI.Components;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Rulesets.UI.Scrolling;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Tests.Skinning
{
    /// <summary>
    /// A test scene for a karaoke hitobject.
    /// </summary>
    public abstract class KaraokeHitObjectTestScene : KaraokeSkinnableColumnTestScene
    {
        protected const float PADDING = 100;

        [BackgroundDependencyLoader]
        private void load()
        {
            SetContents(() => new FillFlowContainer
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
                Height = 0.7f,
                Direction = FillDirection.Horizontal,
                Padding = new MarginPadding { Left = PADDING, Right = PADDING },
                Children = new[]
                {
                    new NotePlayfieldTestContainer(COLUMN_NUMBER)
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        RelativeSizeAxes = Axes.X,
                        Height = DefaultColumnBackground.COLUMN_HEIGHT,
                        Child = new ScrollingHitObjectContainer
                        {
                            RelativeSizeAxes = Axes.Both,
                        }.With(c =>
                        {
                            c.Add(CreateHitObject().With(h =>
                            {
                                h.HitObject.StartTime = START_TIME;

                                if (h.HitObject is IHasEndTime hasEndTimeHitObject)
                                    hasEndTimeHitObject.EndTime = START_TIME * 2;

                                h.AccentColour.Value = Color4.Orange;
                            }));
                        })
                    },
                }
            });
        }

        protected abstract DrawableHitObject CreateHitObject();
    }
}
