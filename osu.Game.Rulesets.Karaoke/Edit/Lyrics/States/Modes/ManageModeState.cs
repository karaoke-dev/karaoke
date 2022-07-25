﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Manage;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes
{
    public class ManageModeState : Component, IManageModeState
    {
        private readonly Bindable<TextingEditMode> bindableEditMode = new();

        public IBindable<TextingEditMode> BindableEditMode => bindableEditMode;

        public void ChangeEditMode(TextingEditMode mode)
            => bindableEditMode.Value = mode;

        public Bindable<ManageEditModeSpecialAction> BindableSpecialAction { get; } = new();
    }
}
