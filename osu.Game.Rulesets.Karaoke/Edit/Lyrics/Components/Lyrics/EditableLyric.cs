// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Components.Lyrics
{
    [Cached(typeof(IEditableLyricState))]
    public class EditableLyric : InteractableLyric, IEditableLyricState
    {
        [Resolved, AllowNull]
        private ILyricsChangeHandler lyricsChangeHandler { get; set; }

        [Resolved, AllowNull]
        private ILyricCaretState lyricCaretState { get; set; }

        public EditableLyric(Lyric lyric)
            : base(lyric)
        {
            CornerRadius = 5;
            Padding = new MarginPadding { Bottom = 10 };
        }

        protected override IEnumerable<BaseLayer> CreateLayers(Lyric lyric)
        {
            return new BaseLayer[]
            {
                new TimeTagLayer(lyric),
                new CaretLayer(lyric),
                new BlueprintLayer(lyric),
            };
        }

        public void TriggerDisallowEditEffect()
        {
            InternalChildren.OfType<BaseLayer>().ForEach(x => x.TriggerDisallowEditEffect(BindableMode.Value));
        }

        protected override bool OnDoubleClick(DoubleClickEvent e)
        {
            var position = lyricCaretState.BindableCaretPosition.Value;

            switch (position)
            {
                case CuttingCaretPosition cuttingCaretPosition:
                    lyricsChangeHandler.Split(cuttingCaretPosition.Index);
                    return true;

                default:
                    return false;
            }
        }
    }
}
