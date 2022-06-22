// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using osu.Framework.Bindables;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes
{
    public interface IHasEditModeState<T> where T : Enum
    {
        IBindable<T> BindableEditMode { get; }

        T EditMode => BindableEditMode.Value;

        void ChangeEditMode(T mode);
    }
}
