// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Timing;
using osu.Framework.Utils;
using osu.Game.Rulesets.Mods;
using osu.Game.Screens.Play;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Mods
{
    public class KaraokeModSnow : Mod, IApplicableToHUD
    {
        public override string Name => "Snow";

        public override string Description => "Display some snow";
        public override string Acronym => "SW";
        public override double ScoreMultiplier => 1.0f;
        public override IconUsage? Icon => FontAwesome.Regular.Snowflake;
        public override ModType Type => ModType.Fun;

        public void ApplyToHUD(HUDOverlay overlay)
        {
            overlay.Add(CreateSnowContainer);
        }

        protected virtual SnowContainer CreateSnowContainer => new()
        {
            SnowGenerateParSecond = 1,
            EnableNewSnow = true,
            SnowExpireTime = 6000,
            Enabled = true,
            Speed = 1,
            WingAffection = 3,
            SnowSize = 0.3f,
            TexturePath = @"Mod/Snow/Snow",
            Clock = new FramedClock(new StopwatchClock(true)),
            RelativeSizeAxes = Axes.Both,
            Depth = 1,
        };

        protected class SnowContainer : Container
        {
            // Max can have 1000 snow at the scene
            public int SnowGenerateParSecond { get; set; }

            // If disable ,will stop snow
            public bool EnableNewSnow { get; set; }

            // Snow expire time
            public int SnowExpireTime { get; set; }

            // If disable ,will pause and no show will fall down
            public bool Enabled { get; set; }

            // Snow speed
            public float Speed { get; set; }

            // Wing speed
            public float WingAffection { get; set; }

            // Snow size
            public float SnowSize { get; set; }

            // Texture path
            public string TexturePath { get; set; }

            protected override void Update()
            {
                if (!Enabled)
                    return;

                base.Update();

                double currentTime = Time.Current;

                bool isCreateShow = !Children.Any() ||
                                    (Children.LastOrDefault() as SnowSprite)?.CreateTime
                                    + 1000 / SnowGenerateParSecond < currentTime;

                // If can generate new snow
                if (isCreateShow && EnableNewSnow)
                {
                    float currentAlpha = (float)RNG.Next(0, 255) / 255;
                    int width = (int)DrawWidth;
                    var newFlake = new SnowSprite
                    {
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.Centre,
                        Colour = Color4.White,
                        Position = new Vector2(RNG.Next(-width / 2, width / 2), -40),
                        Depth = 1,
                        CreateTime = currentTime,
                        Size = new Vector2(50, 50),
                        Scale = new Vector2(1, 1) * SnowSize,
                        Alpha = currentAlpha,
                        HorizontalSpeed = RNG.Next(-100, 100) + WingAffection * 10
                    };
                    Add(newFlake);
                }

                // Update each snow position
                foreach (var drawable in Children)
                {
                    if (drawable is not SnowSprite snow)
                        continue;

                    snow.X += snow.HorizontalSpeed / 1000f;
                    snow.Y += 1 * Speed;

                    // Recycle
                    if (snow.CreateTime + SnowExpireTime < currentTime)
                        Children.ToList().Remove(snow);
                }
            }

            /// <summary>
            /// Show spirit
            /// </summary>
            private class SnowSprite : Circle
            {
                public float HorizontalSpeed { get; set; }

                public double CreateTime { get; set; }
            }
        }
    }
}
