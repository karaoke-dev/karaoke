// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Linq;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Settings.RubyRomaji.Components;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Settings.RubyRomaji
{
    public abstract class TextTagEditSection<TTextTag> : LyricPropertySection where TTextTag : class, ITextTag, new()
    {
        protected readonly IBindableList<TTextTag> TextTags = new BindableList<TTextTag>();

        private Lyric lyric;

        protected TextTagEditSection()
        {
            // add create button.
            addCreateButton();

            // create list of text-tag text-box if bindable changed.
            TextTags.BindCollectionChanged((_, _) =>
            {
                RemoveAll(x => x is LabelledTextTagTextBox<TTextTag>, true);
                AddRange(TextTags.Select(x =>
                {
                    string relativeToLyricText = TextTagUtils.GetTextFromLyric(x, lyric.Text);
                    string range = TextTagUtils.PositionFormattedString(x);

                    return CreateLabelledTextTagTextBox(lyric, x).With(t =>
                    {
                        t.Label = relativeToLyricText;
                        t.Description = range;
                        t.TabbableContentContainer = this;
                    });
                }));
            });
        }

        protected override void OnLyricChanged(Lyric lyric)
        {
            TextTags.UnbindBindings();

            if (lyric == null)
                return;

            this.lyric = lyric;

            TextTags.BindTo(GetBindableTextTags(lyric));
        }

        private void addCreateButton()
        {
            var fillFlowContainer = Content as FillFlowContainer;

            // create new button.
            fillFlowContainer?.Insert(int.MaxValue, new CreateNewTextTagButton<TTextTag>
            {
                Text = CreateNewTextTagButtonText(),
                LabelledTextBoxLabel = CreateNewTextTagTitle(),
                LabelledTextBoxDescription = CreateNewTextTagDescription(),
                Action = AddTextTag
            });
        }

        protected abstract IBindableList<TTextTag> GetBindableTextTags(Lyric lyric);

        protected abstract LabelledTextTagTextBox<TTextTag> CreateLabelledTextTagTextBox(Lyric lyric, TTextTag textTag);

        protected abstract void AddTextTag(TTextTag textTag);

        protected abstract LocalisableString CreateNewTextTagButtonText();

        protected abstract LocalisableString CreateNewTextTagTitle();

        protected abstract LocalisableString CreateNewTextTagDescription();
    }
}
