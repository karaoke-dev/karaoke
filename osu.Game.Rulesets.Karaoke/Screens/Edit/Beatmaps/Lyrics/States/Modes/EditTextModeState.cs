// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Text;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;

public partial class EditTextModeState : Component, IEditTextModeState
{
    public Bindable<TextEditStep> BindableEditStep { get; } = new();

    public Bindable<TextEditModeSpecialAction> BindableSpecialAction { get; } = new();
}
