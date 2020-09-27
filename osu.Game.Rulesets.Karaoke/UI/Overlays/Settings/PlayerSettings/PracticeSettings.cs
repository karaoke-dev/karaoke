// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Input.Bindings;
using osu.Game.Beatmaps;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Graphics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Play.PlayerSettings;

namespace osu.Game.Rulesets.Karaoke.UI.Overlays.Settings.PlayerSettings
{
    public class PracticeSettings : PlayerSettingsGroup, IKeyBindingHandler<KaraokeAction>
    {
        private readonly PlayerSliderBar<double> preemptTimeSliderBar;
        private readonly LyricPreview lyricPreview;

        public PracticeSettings(IBeatmap beatmap)
            : base("Practice")
        {
            var lyrics = beatmap.HitObjects.OfType<LyricLine>().ToList();

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
                lyricPreview = new LyricPreview(lyrics)
                {
                    Height = 580,
                    RelativeSizeAxes = Axes.X
                }
            };
        }

        public bool OnPressed(KaraokeAction action)
        {
            switch (action)
            {
                case KaraokeAction.FirstLyric:
                    // TODO : switch to first lyric
                    break;

                case KaraokeAction.PreviousLyric:
                    // TODO : switch to previous lyric
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

        public void OnReleased(KaraokeAction action)
        {
        }

        [BackgroundDependencyLoader]
        private void load(KaraokeRulesetConfigManager config, KaraokeSessionStatics session)
        {
            preemptTimeSliderBar.Bindable = config.GetBindable<double>(KaraokeRulesetSetting.PracticePreemptTime);
            session.BindWith(KaraokeRulesetSession.NowLyric, lyricPreview.SelectedLyricLine);
        }
    }
}
