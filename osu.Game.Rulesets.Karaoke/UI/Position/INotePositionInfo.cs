// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Bindables;

namespace osu.Game.Rulesets.Karaoke.UI.Position
{
    public interface INotePositionInfo
    {
        IBindable<NotePositionCalculator> Position { get; }

        NotePositionCalculator Calculator { get; }
    }
}
