// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Components.Lyrics.Blueprints;

public abstract partial class LyricPropertyBlueprintContainer<T> : BindableBlueprintContainer<T> where T : class
{
    [Resolved]
    private ILyricCaretState lyricCaretState { get; set; } = null!;

    private readonly BindableList<T> lyricListProperties;

    protected readonly Lyric Lyric;

    protected LyricPropertyBlueprintContainer(Lyric lyric)
    {
        lyricListProperties = GetProperties(lyric);
        Lyric = lyric;
    }

    protected abstract BindableList<T> GetProperties(Lyric lyric);

    [BackgroundDependencyLoader]
    private void load()
    {
        // Make it auto create or remove the blueprint by the list.
        RegisterBindable(lyricListProperties);
    }

    protected override bool OnClick(ClickEvent e)
    {
        lyricCaretState.MoveCaretToTargetPosition(Lyric);
        return base.OnClick(e);
    }

    protected override bool OnDragStart(DragStartEvent e)
    {
        lyricCaretState.MoveCaretToTargetPosition(Lyric);
        return base.OnDragStart(e);
    }
}
