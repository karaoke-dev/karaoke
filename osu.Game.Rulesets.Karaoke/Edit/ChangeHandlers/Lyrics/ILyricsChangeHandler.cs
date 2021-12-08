// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics
{
    public interface ILyricsChangeHandler
    {
        void Split(int index);

        void Combine();

        void CreateAtPosition(int? nextToOrder = null);

        void CreateAtLast();

        void Remove();
    }
}
