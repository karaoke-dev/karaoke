// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Bindables;

namespace osu.Game.Rulesets.Karaoke.Edit.Singers.Edit
{
    internal class MetadataSection : EditSingerSection
    {
        protected override string Title => "Metadata";

        private LabelledTextBox nameTextBox;
        private LabelledTextBox romajiNameTextBox;
        private LabelledTextBox englishNameTextBox;
        private LabelledTextBox descriptionTextBox;

        [BackgroundDependencyLoader]
        private void load(BindableClassWithCurrent<Singer> currentSinger)
        {
            Children = new Drawable[]
            {
                new OsuSpriteText
                {
                    Text = "Singer metadata"
                },
                nameTextBox = new LabelledTextBox
                {
                    Label = "Singer",
                    TabbableContentContainer = this
                },
                romajiNameTextBox = new LabelledTextBox
                {
                    Label = "Romaji name",
                    TabbableContentContainer = this
                },
                englishNameTextBox = new LabelledTextBox
                {
                    Label = "English name",
                    TabbableContentContainer = this
                },
                descriptionTextBox = new LabelledTextBox
                {
                    Label = "Description",
                    TabbableContentContainer = this
                },
            };

            currentSinger.BindValueChanged(e =>
            {
                var singer = e.NewValue;
                nameTextBox.Current.Value = singer.Name;
                romajiNameTextBox.Current.Value = singer.RomajiName;
                englishNameTextBox.Current.Value = singer.EnglishName;
                descriptionTextBox.Current.Value = singer.Description;
            });

            foreach (var item in Children.OfType<LabelledTextBox>())
                item.OnCommit += onCommit;

            void onCommit(TextBox sender, bool newText)
            {
                if (!newText) return;

                var singer = currentSinger.Value;

                // for now, update these on commit rather than making BeatmapMetadata bindables.
                // after switching database engines we can reconsider if switching to bindables is a good direction.
                singer.Name = nameTextBox.Current.Value;
                singer.RomajiName = romajiNameTextBox.Current.Value;
                singer.EnglishName = englishNameTextBox.Current.Value;
                singer.Description = descriptionTextBox.Current.Value;

                // trigger update change to let parent update info.
                currentSinger.TriggerOtherChange();
            }
        }
    }
}
