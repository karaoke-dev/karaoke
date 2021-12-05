// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Notes
{
    public interface INotesChangeHandler : IAutoGenerateChangeHandler
    {
        void ChangeDisplay(bool display);

        void Split(float percentage = 0.5f);

        void Combine();
    }
}
