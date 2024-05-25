// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Language;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;

public partial class EditLanguageModeState : Component, IEditLanguageModeState
{
    public Bindable<LanguageEditModeSpecialAction> BindableSpecialAction { get; } = new();

    public Bindable<LanguageEditStep> BindableEditStep { get; } = new();
}
