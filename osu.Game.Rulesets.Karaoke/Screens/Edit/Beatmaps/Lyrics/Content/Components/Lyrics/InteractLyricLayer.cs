// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Components.Lyrics;

/// <summary>
/// Compare to <see cref="EditLyricLayer"/>, this layer only allows to:
/// 1. Hover the whole lyric.
/// 2. Selec the whole lyric.
/// </summary>
public partial class InteractLyricLayer : UIEventLayer
{
    [Resolved]
    private ILyricCaretState lyricCaretState { get; set; } = null!;

    public InteractLyricLayer(Lyric lyric)
        : base(lyric)
    {
    }

    protected override bool OnMouseMove(MouseMoveEvent e)
    {
        if (!lyricCaretState.CaretEnabled)
            return false;

        if (IsDragged)
            return false;

        lyricCaretState.MoveHoverCaretToTargetPosition(Lyric);

        return base.OnMouseMove(e);
    }

    protected override void OnHoverLost(HoverLostEvent e)
    {
        base.OnHoverLost(e);

        // lost hover caret and time-tag caret
        lyricCaretState.ClearHoverCaretPosition();
    }

    protected override bool OnClick(ClickEvent e)
    {
        return lyricCaretState.MoveCaretToTargetPosition(Lyric);
    }
}
