// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Karaoke.UI;
using osu.Game.Rulesets.Karaoke.UI.Components;
using osu.Game.Skinning;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Tests.Skinning
{
    [TestFixture]
    public class TestSceneHitExplosion : KaraokeSkinnableColumnTestScene
    {
        public override IReadOnlyList<Type> RequiredTypes => new[]
        {
            typeof(DrawableNote),
            typeof(DrawableKaraokeHitObject),
        };

        public TestSceneHitExplosion()
        {
            int runCount = 0;

            AddRepeatStep("explode", () =>
            {
                runCount++;

                if (runCount % 15 > 12)
                    return;

                CreatedDrawables.OfType<Container>().ForEach(c =>
                {
                    var colour = runCount / 15 % 2 == 0 ? new Color4(94, 0, 57, 255) : new Color4(6, 84, 0, 255);
                    c.Add(new SkinnableDrawable(new KaraokeSkinComponent(KaraokeSkinComponents.HitExplosion),
                        _ => new DefaultHitExplosion(colour, runCount % 6 != 0)
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                        }));
                });
            }, 100);
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            SetContents(() => new NotePlayfieldTestContainer(0)
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativePositionAxes = Axes.Y,
                Y = -0.25f,
                Size = new Vector2(DefaultHitExplosion.EXPLOSION_SIZE, DefaultColumnBackground.COLUMN_HEIGHT),
            });
        }
    }
}
