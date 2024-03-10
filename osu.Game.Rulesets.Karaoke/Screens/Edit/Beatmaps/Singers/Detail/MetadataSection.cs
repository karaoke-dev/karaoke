// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Localisation;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Singers.Detail;

internal partial class MetadataSection : EditSingerSection
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
                TabbableContentContainer = this,
            },
            new LabelledTextBox
            {
                Label = "Romanisation",
                Current = singer.RomanisationBindable,
                TabbableContentContainer = this,
            },
            new LabelledTextBox
            {
                Label = "English name",
                Current = singer.EnglishNameBindable,
                TabbableContentContainer = this,
            },
            new LabelledTextBox
            {
                Label = "Description",
                Current = singer.DescriptionBindable,
                TabbableContentContainer = this,
            },
        };

        // todo: see NoteEditPopover to implement the undo behavior.
    }
}
