// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Game.Beatmaps;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Play.PlayerSettings;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.UI.PlayerSettings
{
    public class PracticeSettings : PlayerSettingsGroup, IKeyBindingHandler<KaraokeAction>
    {
        private readonly PlayerSliderBar<double> preemptTimeSliderBar;

        public PracticeSettings(IBeatmap beatmap)
            : base("Practice")
        {
            var lyrics = beatmap.HitObjects.OfType<Lyric>().ToList();

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
                new LyricsPreview(lyrics)
                {
                    Height = 580,
                    RelativeSizeAxes = Axes.X,
                    Spacing = new Vector2(15),
                }
            };
        }

        public bool OnPressed(KeyBindingPressEvent<KaraokeAction> e)
        {
            switch (e.Action)
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

        public void OnReleased(KeyBindingReleaseEvent<KaraokeAction> e)
        {
        }

        [BackgroundDependencyLoader]
        private void load(KaraokeRulesetConfigManager config)
        {
            preemptTimeSliderBar.Current = config.GetBindable<double>(KaraokeRulesetSetting.PracticePreemptTime);
        }
    }
}
