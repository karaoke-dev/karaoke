// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics.UserInterface;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Edit.Components.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Reference
{
    public class LabelledLyricSelector : LabelledComponent<LabelledLyricSelector.SelectLyricButton, Lyric?>
    {
        public LabelledLyricSelector()
            : base(true)
        {
        }

        protected override SelectLyricButton CreateComponent()
            => new()
            {
                RelativeSizeAxes = Axes.X
            };

        public class SelectLyricButton : OsuButton, IHasCurrentValue<Lyric?>, IHasPopover
        {
            private readonly BindableWithCurrent<Lyric?> current = new();

            public Bindable<Lyric?> Current
            {
                get => current.Current;
                set => current.Current = value;
            }

            public SelectLyricButton()
            {
                Action = this.ShowPopover;
                current.BindValueChanged(x =>
                {
                    var lyric = x.NewValue;
                    Text = lyric == null
                        ? "Select lyric..."
                        : $"#{lyric.Order} {lyric.Text}";
                }, true);
            }

            public Popover GetPopover()
                => new LyricSelectorPopover(Current);
        }

        private class LyricSelectorPopover : OsuPopover
        {
            private readonly LyricSelector lyricSelector;

            public LyricSelectorPopover(Bindable<Lyric?> bindable)
            {
                Child = lyricSelector = new LyricSelector
                {
                    Width = 400,
                    Height = 600,
                    Current = bindable
                };
            }

            protected override void LoadComplete()
            {
                base.LoadComplete();

                GetContainingInputManager().ChangeFocus(lyricSelector);
            }
        }
    }
}
