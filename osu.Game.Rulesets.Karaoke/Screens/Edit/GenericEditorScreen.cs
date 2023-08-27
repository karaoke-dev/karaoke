// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit;

/// <summary>
/// TODO: eventually make this inherit Screen and add a local screen stack inside the Editor.
/// </summary>
public abstract partial class GenericEditorScreen<TType> : EditorScreen
{
    public new readonly TType Type;

    protected GenericEditorScreen(TType type)
        : base(EditorScreenMode.Compose)
    {
        Type = type;
    }
}
