// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Bindings;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Play.PlayerSettings;

namespace osu.Game.Rulesets.Karaoke.UI.PlayerSettings
{
    public class PracticeSettings : PlayerSettingsGroup, IKeyBindingHandler<KaraokeAction>
    {
        protected override string Title => "Practice";

        private readonly PlayerSliderBar<double> preemptTimeSliderBar;
        private readonly LyricPreview lyricPreview;

        public PracticeSettings()
        {
            Children = new Drawable[]
            {
                new OsuSpriteText
                {
                    Text = "Practice preempt time:"
                },
                preemptTimeSliderBar = new PlayerSliderBar<double>(),
                new OsuSpriteText
                {
                    Text = "Lyric:"
                },
                lyricPreview = new LyricPreview
                {
                    Height = 500
                }
            };

            lyricPreview.SelectedLyricLine.BindValueChanged(value =>
            {
                var lyricStartTime = value.NewValue?.StartTime;

                // TODO : switch track to target start time.
            });
        }

        public bool OnPressed(KaraokeAction action)
        {
            switch (action)
            {
                case KaraokeAction.FirstLyric:
                    // TODO : switch to first lyric
                    break;

                case KaraokeAction.PreviousLyric:
                    // TODO : switch to pervious lyric
                    break;

                case KaraokeAction.NextLyric:
                    // TODO : switch to next lyric
                    break;

                case KaraokeAction.PlayAndPause:
                    // TODO : pause
                    break;

                default:
                    return false;
            }

            return true;
        }

        public bool OnReleased(KaraokeAction action) => true;

        [BackgroundDependencyLoader]
        private void load(KaraokeRulesetConfigManager config)
        {
            preemptTimeSliderBar.Bindable = config.GetBindable<double>(KaraokeRulesetSetting.PracticePreemptTime);
        }

        internal class LyricPreview : Container
        {
            public Bindable<LyricLine> SelectedLyricLine { get; } = new Bindable<LyricLine>();

            public LyricPreview()
            {
                RelativeSizeAxes = Axes.X;
                Child = new OsuScrollContainer
                {
                    RelativeSizeAxes = Axes.Both
                };

                // TODO : initial lyrics

                // If can, get event on which lyric line is playing.
            }
        }
    }
}
