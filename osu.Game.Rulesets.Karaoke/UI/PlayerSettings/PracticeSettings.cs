// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Game.Beatmaps;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Mods;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Mods;
using osu.Game.Screens.Play.PlayerSettings;

namespace osu.Game.Rulesets.Karaoke.UI.PlayerSettings;

[Cached(typeof(ILyricNavigator))]
public partial class PracticeSettings : PlayerSettingsGroup, IKeyBindingHandler<KaraokeAction>, ILyricNavigator
{
    [Resolved]
    private IBindable<WorkingBeatmap> beatmap { get; set; } = null!;

    private readonly PlayerSliderBar<double> preemptTimeSliderBar;

    public PracticeSettings()
        : base("Practice")
    {
        Children = new Drawable[]
        {
            new OsuSpriteText
            {
                Text = "Practice preempt time:",
            },
            preemptTimeSliderBar = new PlayerSliderBar<double>(),
            new OsuSpriteText
            {
                Text = "Lyric:",
            },
            new LyricsPreview
            {
                Height = 580,
                RelativeSizeAxes = Axes.X,
            },
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
    private void load(IBindable<IReadOnlyList<Mod>> mods)
    {
        var practiceMod = mods.Value.OfType<KaraokeModPractice>().First();
        preemptTimeSliderBar.Current.Value = practiceMod.LyricPreemptTime.Value;
    }

    public void SeekTimeByLyric(Lyric target)
    {
        double? time = target.StartTime - preemptTimeSliderBar.Current.Value;
        if (time != null)
            beatmap.Value.Track.Seek(time.Value);
    }
}
