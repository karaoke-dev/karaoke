// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Graphics;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components
{
    public class SelectLyricButton : OsuButton
    {
        private IBindable<bool> selecting;

        protected virtual string StandardText => "Select lyric";

        protected virtual string SelectingText => "Cancel selecting";

        public Func<Dictionary<Lyric, string>> StartSelecting { get; set; }

        public SelectLyricButton()
        {
            RelativeSizeAxes = Axes.X;
            Content.CornerRadius = 15;
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colour, ILyricSelectionState lyricSelectionState)
        {
            selecting = lyricSelectionState.Selecting.GetBoundCopy();
            selecting.BindValueChanged(e =>
            {
                bool isSelecting = e.NewValue;
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
                    // update disabled lyrics list.
                    var disableLyrics = StartSelecting?.Invoke();
                    lyricSelectionState.UpdateDisableLyricList(disableLyrics);

                    // then start selecting.
                    lyricSelectionState.StartSelecting();
                }
            };
        }
    }
}
