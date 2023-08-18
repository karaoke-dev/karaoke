// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Bindables;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings;

public abstract partial class TextTagSettings<TEditStep> : LyricEditorSettings
    where TEditStep : Enum
{
    public override SettingsDirection Direction => SettingsDirection.Right;

    public override float SettingsWidth => 350;

    protected readonly IBindable<TEditStep> BindableEditStep = new Bindable<TEditStep>();
}
