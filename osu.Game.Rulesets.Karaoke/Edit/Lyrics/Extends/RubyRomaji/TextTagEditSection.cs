// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Game.Graphics;
using osu.Game.Graphics.UserInterface;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Edit.Components.Containers;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osu.Game.Rulesets.Karaoke.Utils;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.RubyRomaji
{
    public abstract class TextTagEditSection<T> : Section where T : ITextTag
    {
        protected readonly Bindable<T[]> TextTags = new Bindable<T[]>();

        protected Lyric Lyric { get; set; }

        protected TextTagEditSection()
        {
            // create list of text-tag text-box if bindable changed.
            TextTags.BindValueChanged(e =>
            {
                Content.RemoveAll(x => x is LabelledTextTagTextBox);
                Content.AddRange(e.NewValue?.Select(x =>
                {
                    var relativeToLyricText = TextTagUtils.GetTextFromLyric(x, Lyric?.Text);
                    var range = TextTagUtils.PositionFormattedString(x);
                    return new LabelledTextTagTextBox(x)
                    {
                        Label = relativeToLyricText,
                        Description = range,
                        OnDeleteButtonClick = () =>
                        {
                            LyricUtils.RemoveTextTag(Lyric, x);
                        },
                        TabbableContentContainer = this
                    };
                }));
            });

            // add create button.
            AddCreateButton();
        }

        protected void AddCreateButton()
        {
            var fillFlowContainer = Content as FillFlowContainer;

            // create new button.
            fillFlowContainer?.Insert(int.MaxValue, new CreateNewButton
            {
                Depth = float.MinValue,
                Text = "Create new",
                Action = () =>
                {
                    // todo : add new ruby/romaji next to selected one.
                }
            });
        }

        public class LabelledTextTagTextBox : LabelledTextBox
        {
            protected const float DELETE_BUTTON_SIZE = 20f;

            [Resolved]
            private OsuColour colours { get; set; }

            private readonly BindableList<ITextTag> selectedTextTag = new BindableList<ITextTag>();

            private readonly ITextTag textTag;

            public Action OnDeleteButtonClick;

            public LabelledTextTagTextBox(ITextTag textTag)
            {
                this.textTag = textTag;

                // apply current text from text-tag.
                Component.Text = textTag.Text;

                // should change preview text box if selected ruby/romaji changed.
                OnCommit += (sender, newText) =>
                {
                    textTag.Text = sender.Text;
                };

                // change style if focus.
                selectedTextTag.BindCollectionChanged((e, a) =>
                {
                    var highLight = selectedTextTag.Contains(textTag);

                    Component.BorderColour = highLight ? colours.Yellow : colours.Blue;
                    Component.BorderThickness = highLight ? 3 : 0;
                });

                if (!(InternalChildren[1] is FillFlowContainer fillFlowContainer))
                    return;

                // change padding to place delete button.
                fillFlowContainer.Padding = new MarginPadding
                {
                    Horizontal = CONTENT_PADDING_HORIZONTAL,
                    Vertical = CONTENT_PADDING_VERTICAL,
                    Right = CONTENT_PADDING_HORIZONTAL + DELETE_BUTTON_SIZE + CONTENT_PADDING_HORIZONTAL,
                };

                // add delete button.
                AddInternal(new Container
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Padding = new MarginPadding
                    {
                        Top = CONTENT_PADDING_VERTICAL + 10,
                        Right = CONTENT_PADDING_HORIZONTAL,
                    },
                    Child = new DeleteIconButton
                    {
                        Anchor = Anchor.TopRight,
                        Origin = Anchor.TopRight,
                        Size = new Vector2(DELETE_BUTTON_SIZE),
                        Action = () => OnDeleteButtonClick?.Invoke(),
                        Hover = hover =>
                        {
                            if (hover)
                            {
                                // trigger selected if hover on delete button.
                                selectedTextTag.Add(textTag);
                            }
                            else
                            {
                                // do not clear current selected if typing.
                                if (Component.HasFocus)
                                    return;

                                selectedTextTag.Remove(textTag);
                            }
                        }
                    }
                });
            }

            protected override void OnFocus(FocusEvent e)
            {
                // do not trigger origin focus event if this drawable has been removed.
                // usually cause by user clicking the delete button.
                if (Parent == null)
                    return;

                base.OnFocus(e);
            }

            protected override OsuTextBox CreateTextBox() => new TextTagTextBox
            {
                CommitOnFocusLost = true,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.X,
                CornerRadius = CORNER_RADIUS,
                Selected = () =>
                {
                    // not trigger again if already focus.
                    if (selectedTextTag.Contains(textTag) && selectedTextTag.Count == 1)
                        return;

                    // trigger selected.
                    selectedTextTag.Clear();
                    selectedTextTag.Add(textTag);
                }
            };

            [BackgroundDependencyLoader]
            private void load(ILyricEditorState state)
            {
                state.SelectedTextTags.BindTo(selectedTextTag);
            }

            internal class TextTagTextBox : OsuTextBox
            {
                public Action Selected;

                protected override void OnFocus(FocusEvent e)
                {
                    Selected?.Invoke();
                    base.OnFocus(e);
                }
            }

            internal class DeleteIconButton : IconButton
            {
                [Resolved]
                protected OsuColour Colours { get; private set; }

                public Action<bool> Hover;

                public DeleteIconButton()
                {
                    Icon = FontAwesome.Solid.Trash;
                }

                protected override bool OnHover(HoverEvent e)
                {
                    Colour = Colours.Yellow;
                    Hover?.Invoke(true);
                    return base.OnHover(e);
                }

                protected override void OnHoverLost(HoverLostEvent e)
                {
                    Colour = Colours.GrayF;
                    Hover?.Invoke(false);
                    base.OnHoverLost(e);
                }
            }
        }

        public class CreateNewButton : OsuButton
        {
            public CreateNewButton()
            {
                RelativeSizeAxes = Axes.X;
                Content.CornerRadius = 15;
            }
        }
    }
}
