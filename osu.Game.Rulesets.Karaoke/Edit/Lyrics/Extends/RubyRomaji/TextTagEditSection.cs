// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Edit.Components.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.RubyRomaji.Components;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.RubyRomaji
{
    public abstract class TextTagEditSection<T> : Section where T : class, ITextTag
    {
        protected readonly Bindable<T[]> TextTags = new();

        protected Lyric Lyric { get; set; }

        protected TextTagEditSection()
        {
            // create list of text-tag text-box if bindable changed.
            TextTags.BindValueChanged(e =>
            {
                RemoveAll(x => x is LabelledTextTagTextBox<T>);
                AddRange(e.NewValue?.Select(x =>
                {
                    string relativeToLyricText = TextTagUtils.GetTextFromLyric(x, Lyric?.Text);
                    string range = TextTagUtils.PositionFormattedString(x);

                    return CreateLabelledTextTagTextBox(x).With(t =>
                    {
                        t.Label = relativeToLyricText;
                        t.Description = range;
                        t.OnDeleteButtonClick = () =>
                        {
                            LyricUtils.RemoveTextTag(Lyric, x);
                        };
                        t.TabbableContentContainer = this;
                    });
                }));
            });

            // add create button.
            AddCreateButton();
        }

        protected abstract LabelledTextTagTextBox<T> CreateLabelledTextTagTextBox(T textTag);

        protected void AddCreateButton()
        {
            var fillFlowContainer = Content as FillFlowContainer;

            // create new button.
            fillFlowContainer?.Insert(int.MaxValue, new CreateNewButton
            {
                Text = "Create new",
                Action = () =>
                {
                    // todo : add new ruby/romaji next to selected one.
                }
            });
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
