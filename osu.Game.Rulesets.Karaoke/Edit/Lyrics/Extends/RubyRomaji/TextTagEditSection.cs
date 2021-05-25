// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osu.Game.Graphics;
using osu.Game.Graphics.UserInterface;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Edit.Components.Containers;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osu.Game.Rulesets.Karaoke.Utils;

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
                Content.AddRange(e.NewValue.Select(x =>
                {
                    var relativeToLyricText = TextTagUtils.GetTextFromLyric(x, Lyric?.Text);
                    var range = TextTagUtils.PositionFormattedString(x);
                    return new LabelledTextTagTextBox(x)
                    {
                        Label = relativeToLyricText,
                        Description = range,
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
            [Resolved]
            private OsuColour colours { get; set; }

            private readonly BindableList<ITextTag> selectedTextTag = new BindableList<ITextTag>();

            private readonly ITextTag textTag;

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
