// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Graphics;
using osu.Framework.Localisation;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;

namespace osu.Game.Rulesets.Karaoke.Edit.Singers.Detail
{
    internal class MetadataSection : EditSingerSection
    {
        protected override LocalisableString Title => "Metadata";

        public MetadataSection(Singer singer)
        {
            Children = new Drawable[]
            {
                new LabelledTextBox
                {
                    Label = "Singer",
                    Current = singer.NameBindable,
                    TabbableContentContainer = this
                },
                new LabelledTextBox
                {
                    Label = "Romaji name",
                    Current = singer.RomajiNameBindable,
                    TabbableContentContainer = this
                },
                new LabelledTextBox
                {
                    Label = "English name",
                    Current = singer.EnglishNameBindable,
                    TabbableContentContainer = this
                },
                new LabelledTextBox
                {
                    Label = "Description",
                    Current = singer.DescriptionBindable,
                    TabbableContentContainer = this
                },
            };

            // todo: see NoteEditPopover to implement the undo behavior.
        }
    }
}
