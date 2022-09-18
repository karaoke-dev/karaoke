// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using osu.Framework.Allocation;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Localisation;
using osu.Game.Graphics.UserInterface;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osu.Game.Rulesets.Karaoke.Utils;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Settings.RubyRomaji.Components
{
    public class CreateNewTextTagButton<TTextTag> : OsuButton, IHasPopover where TTextTag : class, ITextTag, new()
    {
        public new Action<TTextTag> Action;

        [Resolved]
        private ILyricCaretState lyricCaretState { get; set; }

        public LocalisableString LabelledTextBoxLabel { get; set; }

        public LocalisableString LabelledTextBoxDescription { get; set; }

        public CreateNewTextTagButton()
        {
            RelativeSizeAxes = Axes.X;
            Content.CornerRadius = 15;
            base.Action = this.ShowPopover;
        }

        public Popover GetPopover()
        {
            var lyric = lyricCaretState.BindableCaretPosition.Value?.Lyric;
            return new CreateNewPopover(lyric)
            {
                LabelledTextBoxLabel = LabelledTextBoxLabel,
                LabelledTextBoxDescription = LabelledTextBoxDescription,
                Action = textTag =>
                {
                    this.HidePopover();
                    Action?.Invoke(textTag);
                }
            };
        }

        private class CreateNewPopover : OsuPopover
        {
            public Action<TTextTag> Action;

            private readonly LabelledNumberBox labelledStartIndexNumberBox;
            private readonly LabelledNumberBox labelledEndIndexNumberBox;
            private readonly LabelledTextBox labelledTagTextBox;

            private readonly Lyric lyric;

            public CreateNewPopover(Lyric lyric)
            {
                this.lyric = lyric;

                Child = new FillFlowContainer
                {
                    Width = 300,
                    AutoSizeAxes = Axes.Y,
                    Spacing = new Vector2(10),
                    Children = new Drawable[]
                    {
                        labelledStartIndexNumberBox = new LabelledNumberBox
                        {
                            Label = "Start index",
                            Description = "Please enter the start text index in the lyric",
                            PlaceholderText = "0",
                            TabbableContentContainer = this,
                        },
                        labelledEndIndexNumberBox = new LabelledNumberBox
                        {
                            Label = "End index",
                            Description = "Please enter the end text index in the lyric",
                            PlaceholderText = "1",
                            TabbableContentContainer = this,
                        },
                        labelledTagTextBox = new LabelledTextBox
                        {
                            PlaceholderText = "Text",
                            TabbableContentContainer = this,
                        },
                        new AddButton
                        {
                            Text = "Add",
                            Action = submit
                        }
                    }
                };
            }

            public LocalisableString LabelledTextBoxLabel
            {
                set => labelledTagTextBox.Label = value;
            }

            public LocalisableString LabelledTextBoxDescription
            {
                set => labelledTagTextBox.Description = value;
            }

            private void submit()
            {
                const int invalid_tag_index = -1;

                string startIndexText = labelledStartIndexNumberBox.Current.Value;
                int startIndex = startIndexText == null ? invalid_tag_index : int.Parse(startIndexText);

                if (TextTagUtils.OutOfRange(lyric.Text, startIndex))
                {
                    GetContainingInputManager().ChangeFocus(labelledStartIndexNumberBox);
                    return;
                }

                string endIndexText = labelledEndIndexNumberBox.Current.Value;
                int endIndex = endIndexText == null ? invalid_tag_index : int.Parse(endIndexText);

                if (TextTagUtils.OutOfRange(lyric.Text, endIndex))
                {
                    GetContainingInputManager().ChangeFocus(labelledEndIndexNumberBox);
                    return;
                }

                string textTagText = labelledTagTextBox.Current.Value;

                if (string.IsNullOrEmpty(textTagText))
                {
                    GetContainingInputManager().ChangeFocus(labelledTagTextBox);
                    return;
                }

                Action?.Invoke(new TTextTag
                {
                    StartIndex = Math.Min(startIndex, endIndex),
                    EndIndex = Math.Max(startIndex, endIndex),
                    Text = textTagText
                });
            }

            private class AddButton : OsuButton
            {
                public AddButton()
                {
                    RelativeSizeAxes = Axes.X;
                    Content.CornerRadius = 15;
                }
            }
        }
    }
}
