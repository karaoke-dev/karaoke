// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Graphics.UserInterface;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components
{
    public class AutoGenerateButton : OsuButton
    {
        private Bindable<bool> selecting;

        public AutoGenerateButton()
        {
            RelativeSizeAxes = Axes.X;
            Content.CornerRadius = 15;
        }

        [BackgroundDependencyLoader]
        private void load(ILyricEditorState state)
        {
            selecting = state.Selecting.GetBoundCopy();
            selecting.BindValueChanged(e =>
            {
                Text = e.NewValue ? "Cancel generate" : "Generate";
            }, true);

            Action = () =>
            {
                if (selecting.Value)
                {
                    state.EndSelecting(LyricEditorSelectingAction.Cancel);
                }
                else
                {
                    state?.StartSelecting();
                }
            };
        }
    }
}
