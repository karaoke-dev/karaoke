// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.UserInterface;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Edit.Components.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;

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
            private readonly ReferenceLyricSelector lyricSelector;

            public LyricSelectorPopover(Bindable<Lyric?> bindable)
            {
                Child = lyricSelector = new ReferenceLyricSelector
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

        protected class ReferenceLyricSelector : LyricSelector
        {
            protected override RearrangeableLyricListContainer CreateRearrangeableLyricListContainer()
                => new RearrangeableReferenceLyricListContainer();

            protected class RearrangeableReferenceLyricListContainer : RearrangeableLyricListContainer
            {
                protected override DrawableTextListItem CreateDrawable(Lyric? item)
                    => new DrawableReferenceLyricListItem(item);

                protected class DrawableReferenceLyricListItem : DrawableLyricListItem
                {
                    [Resolved, AllowNull]
                    private OsuColour colours { get; set; }

                    [Resolved, AllowNull]
                    private EditorBeatmap editorBeatmap { get; set; }

                    public DrawableReferenceLyricListItem(Lyric? item)
                        : base(item)
                    {
                    }

                    protected override bool OnClick(ClickEvent e)
                    {
                        // cannot select those lyric that already contains reference lyric.
                        if (!selectable(Model))
                            return false;

                        return base.OnClick(e);
                    }

                    protected override void CreateDisplayContent(OsuTextFlowContainer textFlowContainer, Lyric? model)
                    {
                        base.CreateDisplayContent(textFlowContainer, model);

                        // should have disable style if lyric is not selectable.
                        textFlowContainer.Alpha = selectable(model) ? 1 : 0.5f;

                        if (model == null)
                            return;

                        Schedule(() =>
                        {
                            // add reference text at the end of the text.
                            int referenceLyricsAmount = EditorBeatmapUtils.GetAllReferenceLyrics(editorBeatmap, model).Count();

                            if (referenceLyricsAmount > 0)
                            {
                                textFlowContainer.AddText($"({referenceLyricsAmount} reference)", x => x.Colour = colours.Red);
                            }
                        });
                    }

                    private static bool selectable(Lyric? lyric) => lyric?.ReferenceLyric == null;
                }
            }
        }
    }
}
