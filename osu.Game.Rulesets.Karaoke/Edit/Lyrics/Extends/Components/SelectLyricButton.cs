// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Graphics;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;

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
        private void load(OsuColour colour, LyricSelectionState lyricSelectionState)
        {
            selecting = lyricSelectionState.Selecting.GetBoundCopy();
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
                    lyricSelectionState.EndSelecting(LyricEditorSelectingAction.Cancel);
                }
                else
                {
                    lyricSelectionState.StartSelecting();
                }
            };
        }
    }
}
