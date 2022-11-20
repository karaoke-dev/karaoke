// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Edit.Components.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Compose.Panels;

public abstract class PanelSection : Section
{
    private readonly IBindable<Lyric?> bindableFocusedLyric = new Bindable<Lyric?>();

    protected Lyric? Lyric;

    protected override void LoadComplete()
    {
        base.LoadComplete();

        bindableFocusedLyric.BindValueChanged(x =>
        {
            Lyric = x.NewValue;

            OnLyricChanged(Lyric);
        }, true);
    }

    [BackgroundDependencyLoader]
    private void load(ILyricCaretState lyricCaretState)
    {
        bindableFocusedLyric.BindTo(lyricCaretState.BindableFocusedLyric);
    }

    protected abstract void OnLyricChanged(Lyric? lyric);
}
