// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings;

public abstract partial class LyricEditorSettings : EditorSettings
{
    public abstract SettingsDirection Direction { get; }

    public abstract float SettingsWidth { get; }

    [BackgroundDependencyLoader]
    private void load(ILyricEditorState state, LyricEditorColourProvider colourProvider)
    {
        // change the background colour to the lighter one.
        ChangeBackgroundColour(colourProvider.Background3(state.Mode));
    }
}
