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
    public class LabelledReferenceLyricSelector : LabelledComponent<LabelledReferenceLyricSelector.SelectLyricButton, Lyric?>
    {
        public LabelledReferenceLyricSelector()
            : base(true)
        {
        }

        protected override SelectLyricButton CreateComponent()
            => new()
            {
                RelativeSizeAxes = Axes.X
            };

        public Lyric? IgnoredLyric
        {
            get => Component.IgnoredLyric;
            set => Component.IgnoredLyric = value;
        }

        public class SelectLyricButton : OsuButton, IHasCurrentValue<Lyric?>, IHasPopover
        {
            [Resolved, AllowNull]
            private EditorBeatmap editorBeatmap { get; set; }

            private readonly BindableWithCurrent<Lyric?> current = new();

            public Bindable<Lyric?> Current
            {
                get => current.Current;
                set => current.Current = value;
            }

            private Lyric? ignoredLyric;

            public Lyric? IgnoredLyric
            {
                get => ignoredLyric;
                set
                {
                    ignoredLyric = value;

                    // should not enable the selection if current lyric is being referenced.
                    Enabled.Value = ignoredLyric != null && !EditorBeatmapUtils.GetAllReferenceLyrics(editorBeatmap, ignoredLyric).Any();
                }
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
                => new LyricSelectorPopover(Current, IgnoredLyric);
        }

        private class LyricSelectorPopover : OsuPopover
        {
            private readonly ReferenceLyricSelector lyricSelector;

            [Cached]
            private readonly Lyric? ignoreLyric;

            public LyricSelectorPopover(Bindable<Lyric?> bindable, Lyric? ignoreLyric)
            {
                this.ignoreLyric = ignoreLyric;

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

                    [Resolved]
                    private Lyric? ignoredLyric { get; set; }

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
                        // should have disable style if lyric is not selectable.
                        textFlowContainer.Alpha = selectable(model) ? 1 : 0.5f;

                        base.CreateDisplayContent(textFlowContainer, model);

                        if (model == null)
                            return;

                        // add reference text at the end of the text.
                        int referenceLyricsAmount = EditorBeatmapUtils.GetAllReferenceLyrics(editorBeatmap, model).Count();

                        if (referenceLyricsAmount > 0)
                        {
                            textFlowContainer.AddText($"({referenceLyricsAmount} reference)", x => x.Colour = colours.Red);
                        }
                    }

                    private bool selectable(Lyric? lyric)
                        => lyric != ignoredLyric && lyric?.ReferenceLyric == null;
                }
            }
        }
    }
}
