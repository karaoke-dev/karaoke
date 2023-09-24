// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Bindables;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;

public interface IHasEditStep<T> where T : Enum
{
    IBindable<T> BindableEditStep { get; }

    T EditStep => BindableEditStep.Value;

    void ChangeEditStep(T mode);
}