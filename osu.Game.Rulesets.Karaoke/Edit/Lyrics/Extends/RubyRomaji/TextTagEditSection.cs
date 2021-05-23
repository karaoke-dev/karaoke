// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Bindables;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Edit.Components.Containers;
using osu.Game.Rulesets.Karaoke.Objects.Types;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.RubyRomaji
{
    public abstract class TextTagEditSection<T> : Section where T : ITextTag
    {
        protected readonly BindableList<T> SelectedTextTags = new BindableList<T>();

        protected TextTagEditSection()
        {
            // create list of text-tag text-box if bindable changed.
            SelectedTextTags.BindCollectionChanged((a, b) =>
            {
                Content.RemoveAll(x => x is LabelledTextTagTextBox);
                Content.AddRange(SelectedTextTags.Select(x =>
                {
                    // todo : might apply current value.
                    return new LabelledTextTagTextBox();
                }));
            });

            // create new button.

            // should change preview text box if selected ruby/romaji changed.
        }

        public class LabelledTextTagTextBox : LabelledTextBox
        {
        }
    }
}
