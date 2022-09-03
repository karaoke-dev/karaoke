// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Notes
{
    public interface INotePropertyChangeHandler
    {
        void ChangeText(string text);

        void ChangeRubyText(string ruby);

        void ChangeDisplayState(bool display);
    }
}
