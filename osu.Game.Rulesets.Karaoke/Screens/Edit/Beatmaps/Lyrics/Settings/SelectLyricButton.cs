// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Localisation;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings;

public abstract partial class SelectLyricButton : EditorSectionButton
{
    private IBindable<bool> selecting = null!;

    protected abstract LocalisableString StandardText { get; }

    protected abstract LocalisableString SelectingText { get; }

    protected virtual IDictionary<Lyric, LocalisableString> GetDisableSelectingLyrics()
    {
        return new Dictionary<Lyric, LocalisableString>();
    }

    protected abstract void Apply();

    protected virtual void Cancel() { }

    [Resolved]
    private ILyricSelectionState lyricSelectionState { get; set; } = null!;

    [BackgroundDependencyLoader]
    private void load(OsuColour colours)
    {
        selecting = lyricSelectionState.Selecting.GetBoundCopy();
        selecting.BindValueChanged(e =>
        {
            bool isSelecting = e.NewValue;
            BackgroundColour = isSelecting ? colours.Blue : colours.Purple;
            Text = isSelecting ? SelectingText : StandardText;
        }, true);

        Action = () =>
        {
            if (!selecting.Value)
            {
                StartSelectingLyrics();
            }
            else
            {
                EndSelectingLyrics();
            }
        };

        lyricSelectionState.Action = e =>
        {
            switch (e)
            {
                case LyricEditorSelectingAction.Apply:
                    Apply();
                    return;

                case LyricEditorSelectingAction.Cancel:
                    Cancel();
                    return;

                default:
                    throw new InvalidOperationException();
            }
        };
    }

    protected virtual void StartSelectingLyrics()
    {
        // update disabled lyrics list.
        var disableLyrics = GetDisableSelectingLyrics();
        lyricSelectionState.UpdateDisableLyricList(disableLyrics);

        // then start selecting.
        lyricSelectionState.StartSelecting();
    }

    protected virtual void EndSelectingLyrics()
    {
        lyricSelectionState.EndSelecting(LyricEditorSelectingAction.Cancel);
    }
}
