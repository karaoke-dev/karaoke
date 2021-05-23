// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics.UserInterface;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Edit.Components.Containers;
using osu.Game.Rulesets.Karaoke.Objects.Types;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.RubyRomaji
{
    public abstract class TextTagEditSection<T> : Section where T : ITextTag
    {
        protected readonly BindableList<T> TextTags = new BindableList<T>();

        protected TextTagEditSection()
        {
            // create list of text-tag text-box if bindable changed.
            TextTags.BindCollectionChanged((a, b) =>
            {
                Content.RemoveAll(x => x is LabelledTextTagTextBox);
                Content.AddRange(TextTags.Select(x => new LabelledTextTagTextBox(x)));
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
            private ILyricEditorState state { get; set; }

            public LabelledTextTagTextBox(ITextTag textTag)
            {
                // should set ruby / romaji as hover if text-box is selected.

                // should change preview text box if selected ruby/romaji changed.
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
