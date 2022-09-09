// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Texting
{
    public class TextingSwitchSpecialActionSection : SpecialActionSection<TextingEditModeSpecialAction>
    {
        protected override string SwitchActionTitle => "Special actions";

        protected override string SwitchActionDescription => "Copy, delete or move the lyrics.";

        [BackgroundDependencyLoader]
        private void load(ITextingModeState textingModeState)
        {
            BindTo(textingModeState);
        }

        protected override void UpdateActionArea(TextingEditModeSpecialAction action)
        {
            RemoveAll(x => x is TextingDeleteSubsection, true);

            switch (action)
            {
                case TextingEditModeSpecialAction.Copy:
                    // todo: implement
                    break;

                case TextingEditModeSpecialAction.Delete:
                    Add(new TextingDeleteSubsection());
                    break;

                case TextingEditModeSpecialAction.Move:
                    // todo: implement
                    break;

                default:
                    return;
            }
        }
    }
}
