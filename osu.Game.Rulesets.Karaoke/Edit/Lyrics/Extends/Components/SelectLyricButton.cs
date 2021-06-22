// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Graphics;
using osu.Game.Graphics.UserInterface;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components
{
    public class SelectLyricButton : OsuButton
    {
        private Bindable<bool> selecting;

        protected virtual string StandardText => "Select lyric";

        protected virtual string SelectingText => "Cancel selecting";

        public SelectLyricButton()
        {
            RelativeSizeAxes = Axes.X;
            Content.CornerRadius = 15;
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colour, ILyricEditorState state)
        {
            selecting = state.Selecting.GetBoundCopy();
            selecting.BindValueChanged(e =>
            {
                var isSelecting = e.NewValue;
                BackgroundColour = isSelecting ? colour.Blue : colour.Purple;
                Text = isSelecting ? SelectingText : StandardText;
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
