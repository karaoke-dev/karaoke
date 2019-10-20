// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects.Drawables.Pieces;
using osu.Game.Rulesets.Objects.Drawables;
using System.Linq;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Beatmaps.Objects;
using osu.Game.Rulesets.Karaoke.Skinning;
using osu.Game.Rulesets.Scoring;
using osu.Game.Skinning;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Objects.Drawables
{
    public class DrawableLyricLine : DrawableKaraokeHitObject
    {
        private readonly Container frontKaraokeTextContainer;
        private readonly KaraokeText frontKaraokeText;

        private readonly Container backKaraokeTextContainer;
        private readonly KaraokeText backKaraokeText;

        private readonly OsuSpriteText translateText;

        public new LyricLine HitObject => (LyricLine)base.HitObject;

        public DrawableLyricLine(LyricLine hitObject)
            : base(hitObject)
        {
            Scale = new Vector2(2f);
            AutoSizeAxes = Axes.Both;
            AddInternal(new Container
            {
                AutoSizeAxes = Axes.Both,
                Children = new Drawable[]
                {
                    backKaraokeTextContainer = new Container
                    {
                        AutoSizeAxes = Axes.Both,
                        Masking = true,
                        Child = backKaraokeText = new KaraokeText
                        {
                            Margin = new MarginPadding(30),
                        }
                    },
                    frontKaraokeTextContainer = new Container
                    {
                        AutoSizeAxes = Axes.Y,
                        Masking = true,
                        Child = frontKaraokeText = new KaraokeText
                        {
                            Margin = new MarginPadding(30),
                        }
                    }
                }
            });

            AddInternal(translateText = new OsuSpriteText
            {
                Anchor = Anchor.BottomLeft,
                Origin = Anchor.TopLeft,
                Text = hitObject.TranslateText
            });

            ApplyHitObject(frontKaraokeText, hitObject);
            ApplyHitObject(backKaraokeText, hitObject);
        }

        protected void ApplyHitObject(KaraokeText text, LyricLine lyricLine)
        {
            text.Text = lyricLine.Text;
            text.Rubies = lyricLine.RubyTags?.Select(x => new PositionText(x.Ruby, x.StartIndex, x.EndIndex)).ToArray();
            text.Romajies = lyricLine.RomajiTags?.Select(x => new PositionText(x.Romaji, x.StartIndex, x.EndIndex)).ToArray();
        }

        protected override void Update()
        {
            var percentage = HitObject.GetPercentageByTime(Time.Current);
            var startIndex = percentage.StartIndex;
            var endIndex = percentage.EndIndex;
            var textPercentage = (float)percentage.TextPercentage;

            if (percentage.Available && startIndex != endIndex)
            {
                // Update front karaoke text's width
                var width = backKaraokeText.GetPrecentageWidth(startIndex, endIndex, textPercentage);
                frontKaraokeTextContainer.Width = width;
            }

            base.Update();
        }

        protected override void ApplySkin(ISkinSource skin, bool allowFallback)
        {
            base.ApplySkin(skin, allowFallback);

            var karaokeFont = skin.GetConfig<KaraokeFontLookup, KaraokeFont>(new KaraokeFontLookup { FontIndex = HitObject.FontIndex })?.Value;
            if (karaokeFont != null)
                ApplyFont(karaokeFont);

            var karaokeLayout = skin.GetConfig<KaraokeLayoutLookup, KaraokeLayout>(new KaraokeLayoutLookup { LayoutIndex = HitObject.LayoutIndex })?.Value;
            if (karaokeLayout != null)
                ApplyLayout(karaokeLayout);
        }

        protected void ApplyFont(KaraokeFont font)
        {
            applyTextBrushInfo(frontKaraokeText, font.FrontTextBrushInfo);
            applyTextBrushInfo(backKaraokeText, font.BackTextBrushInfo);

            applyTextFontInfo(frontKaraokeText);
            applyTextFontInfo(backKaraokeText);

            applyShadow(frontKaraokeText);
            applyShadow(backKaraokeText);

            void applyTextBrushInfo(KaraokeText text, KaraokeFont.TextBrushInfo brushInfo)
            {
                // TODO : implement TextBrush and BorderBrush
                var textBrush = brushInfo.TextBrush;
                text.Colour = textBrush.SolidColor;

                // TODO : maybe mixed color in the future
                var shadowBrush = brushInfo.ShadowBrush;
                text.ShadowColour = shadowBrush.SolidColor;
            }

            void applyTextFontInfo(KaraokeText text)
            {
                // TODO : FontName, Bold and EdgeSize
                var lyricFont = font.LyricTextFontInfo.LyricTextFontInfo;
                text.Font = new FontUsage(size: lyricFont.CharSize);

                var rubyFont = font.RubyTextFontInfo.LyricTextFontInfo;
                text.RubyFont = new FontUsage(size: rubyFont.CharSize);
            }

            void applyShadow(KaraokeText text)
            {
                text.Shadow = font.UseShadow;
                text.ShadowOffset = font.ShadowOffset / 60;
            }
        }

        protected void ApplyLayout(KaraokeLayout layout)
        {
            Anchor = layout.Alignment;
            Origin = layout.Alignment;
            Margin = new MarginPadding
            {
                Left = layout.HorizontalMargin,
                Right = layout.HorizontalMargin,
                Top = layout.VerticalMargin,
                Bottom = layout.VerticalMargin
            };

            // Apply text property
            applyKarokeTextProperty(frontKaraokeText);
            applyKarokeTextProperty(backKaraokeText);

            void applyKarokeTextProperty(KaraokeText text)
            {
                // TODO : Continuous
                // TODO : SmartHorizon

                text.Spacing = new Vector2(layout.LyricsInterval, text.Spacing.Y);
                text.RubySpacing = new Vector2(layout.RubyInterval, text.RubySpacing.Y);
                text.RomajiSpacing = new Vector2(layout.RubyInterval, text.RomajiSpacing.Y);

                // TODO : RubyAlignment

                text.RubyMargin = layout.RubyMargin;
                text.RomajiMargin = layout.RomajiMargin;
            }
        }

        protected override void CheckForResult(bool userTriggered, double timeOffset)
        {
            if (userTriggered || Time.Current < HitObject.EndTime)
                return;

            ApplyResult(r => { r.Type = HitResult.Miss; });
        }

        protected override void UpdateStateTransforms(ArmedState state)
        {
            base.UpdateStateTransforms(state);

            using (BeginDelayedSequence(HitObject.Duration, true))
            {
                this.FadeOut(500);
            }
        }
    }
}
