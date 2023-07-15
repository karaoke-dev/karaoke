// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osu.Game.Rulesets.Karaoke.Objects.Utils;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.RubyRomaji.Components;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.RubyRomaji;

public abstract partial class TextTagEditSection<TTextTag> : LyricPropertiesSection<TTextTag> where TTextTag : class, ITextTag, new()
{
    protected abstract partial class TextTagsEditor : LyricPropertiesEditor
    {
        protected sealed override Drawable CreateDrawable(TTextTag item)
        {
            string relativeToLyricText = TextTagUtils.GetTextFromLyric(item, CurrentLyric.Text);
            string range = TextTagUtils.PositionFormattedString(item);

            return CreateLabelledTextTagTextBox(CurrentLyric, item).With(t =>
            {
                t.Label = relativeToLyricText;
                t.Description = range;
                t.TabbableContentContainer = this;
            });
        }

        protected abstract LabelledTextTagTextBox<TTextTag> CreateLabelledTextTagTextBox(Lyric lyric, TTextTag textTag);

        protected override EditorSectionButton CreateCreateNewItemButton()
        {
            return new CreateNewTextTagButton<TTextTag>
            {
                Text = CreateNewTextTagButtonText(),
                LabelledTextBoxLabel = CreateNewTextTagTitle(),
                LabelledTextBoxDescription = CreateNewTextTagDescription(),
                Action = AddTextTag,
            };
        }

        protected override IBindableList<TTextTag> GetItems(Lyric lyric)
        {
            throw new System.NotImplementedException();
        }

        protected abstract LocalisableString CreateNewTextTagButtonText();

        protected abstract LocalisableString CreateNewTextTagTitle();

        protected abstract LocalisableString CreateNewTextTagDescription();

        protected abstract void AddTextTag(TTextTag textTag);
    }
}
