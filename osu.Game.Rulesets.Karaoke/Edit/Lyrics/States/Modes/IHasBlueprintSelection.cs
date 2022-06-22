// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Bindables;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes
{
    public interface IHasBlueprintSelection<T> where T : class
    {
        BindableList<T> SelectedItems { get; }
    }
}
