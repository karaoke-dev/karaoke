// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics
{
    public interface ILyricsChangeHandler
    {
        void Split(int index);

        void Combine();

        void CreateAtPosition();

        void CreateAtLast();

        void AddBelowToSelection(Lyric lyric);

        void AddRangeBelowToSelection(IEnumerable<Lyric> lyrics);

        void Remove();

        void ChangeOrder(int newOrder);
    }
}
