// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.Sprites;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Config.Previews.Graphics
{
    public class ManageFontPreview : SettingsSubsectionPreview
    {
        private const float preview_width = 400;
        private const float preview_height = 320;

        private const float angle = 30;

        public ManageFontPreview()
        {
            RelativeSizeAxes = Axes.Both;
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colour)
        {
            InternalChild = new Container
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(preview_width, preview_height),
                Masking = true,
                CornerRadius = 15,
                BorderThickness = 10f,
                BorderColour = colour.Gray6,
                Children = new Drawable[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = colour.Gray3,
                    },
                    new FillFlowContainer<GenerateRowContainer>
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Width = preview_width / (float)Math.Cos(Math.PI * angle / 180.0),
                        AutoSizeAxes = Axes.Y,
                        Spacing = new Vector2(10),
                        Rotation = -angle,
                        Children = new[]
                        {
                            new GenerateRowContainer(GenerateDirection.LeftToRight),
                            new GenerateRowContainer(GenerateDirection.RightToLeft),
                            new GenerateRowContainer(GenerateDirection.LeftToRight),
                            new GenerateRowContainer(GenerateDirection.RightToLeft),
                            new GenerateRowContainer(GenerateDirection.LeftToRight),
                            new GenerateRowContainer(GenerateDirection.RightToLeft),
                            new GenerateRowContainer(GenerateDirection.LeftToRight),
                            new GenerateRowContainer(GenerateDirection.RightToLeft),
                        }
                    }
                }
            };
        }

        public class GenerateRowContainer : BeatSyncedContainer
        {
            private readonly IDictionary<string, int> words = new Dictionary<string, int>
            {
                { "Font", 10 },
                { "文字", 10 },
                { "Moji", 10 },
                { "もじ", 10 },
                { "Management", 7 },
                { "Style", 7 },
                { "Coolest", 7 },
                { "Random", 7 },
                { "karaoke!", 5 },
                { "カラオケ！", 5 },
                { "osu!", 2 },
                { "lazer!", 2 },
                { "egg", 1 },
                { "andy840119", 1 },
                { "\\andy840119/", 1 },
                { "UWU", 1 },
                { "ODO", 1 },
                { "OWO", 1 },
            };

            private readonly Random random = new Random();

            private const float moving_speed = 60;
            private const float max_text_amount = 10;
            private const float spacing_between_text = 20;
            private const float font_size = 48;

            private readonly GenerateDirection direction;

            public GenerateRowContainer(GenerateDirection direction)
            {
                this.direction = direction;

                RelativeSizeAxes = Axes.X;
                Height = font_size;
            }

            protected override void OnNewBeat(int beatIndex, TimingControlPoint timingPoint, EffectControlPoint effectPoint, ChannelAmplitudes amplitudes)
            {
                base.OnNewBeat(beatIndex, timingPoint, effectPoint, amplitudes);

                foreach (var text in Children)
                {
                    text.ScaleTo(new Vector2(1.1f), 30, Easing.OutBack)
                        .Then()
                        .ScaleTo(1, 20, Easing.OutBack);
                }
            }

            protected override void Update()
            {
                base.Update();

                var lastChild = Children?.LastOrDefault();
                ;
                bool generateNewObject = lastChild == null || isAllTextPartAppear(lastChild, direction);
                if (generateNewObject && Children?.Count < max_text_amount)
                    createNewText();

                static bool isAllTextPartAppear(Drawable text, GenerateDirection direction)
                {
                    bool startFromLeft = direction == GenerateDirection.LeftToRight;
                    var textEndPositionX = text.X + (text.DrawWidth / 2) * (startFromLeft ? -1 : 1);
                    return startFromLeft ? textEndPositionX > spacing_between_text : textEndPositionX < -spacing_between_text;
                }
            }

            private void createNewText()
            {
                bool startFromLeft = direction == GenerateDirection.LeftToRight;
                var text = new OsuSpriteText
                {
                    Anchor = startFromLeft ? Anchor.CentreLeft : Anchor.CentreRight,
                    Origin = Anchor.Centre,
                    Text = getRandomText(),
                    Colour = getRandomColour(),
                    Font = OsuFont.Default.With(size: font_size),
                };
                Add(text);

                // set start position
                var fontWidth = text.DrawWidth;
                text.X = fontWidth * (startFromLeft ? -0.5f : 0.5f);

                // set moving transform.
                var moveLength = DrawWidth + fontWidth + spacing_between_text;
                var movePosition = startFromLeft ? moveLength : -moveLength;
                var duration = moveLength / moving_speed * 1000;
                text.MoveToOffset(new Vector2(movePosition, 0), duration).Then().Expire();

                string getRandomText()
                {
                    var maxNumber = words.Values.Sum();
                    var randomNumber = random.Next(maxNumber - 1);

                    foreach (var (key, value) in words)
                    {
                        if (value >= randomNumber)
                            return key;

                        randomNumber -= value;
                    }

                    return ":Bug:";
                }

                Colour4 getRandomColour()
                {
                    var randomNumber = random.Next(1, 359);
                    return Color4Extensions.FromHSV(randomNumber, 0.2f, 0.7f);
                }
            }
        }

        public enum GenerateDirection
        {
            LeftToRight,

            RightToLeft,
        }
    }
}
