// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Skinning;
using osu.Game.Rulesets.Karaoke.UI.Components;
using osu.Game.Rulesets.Karaoke.UI.Scrolling;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.UI.Position
{
    public class NotePositionInfo : Component, INotePositionInfo
    {
        private readonly Bindable<NotePositionCalculator> position = new Bindable<NotePositionCalculator>();
        public new IBindable<NotePositionCalculator> Position => position;
        public NotePositionCalculator Calculator => Position.Value;

        private readonly IBindable<float> bindableColumnHeight = new Bindable<float>(DefaultColumnBackground.COLUMN_HEIGHT);
        private readonly IBindable<float> bindableColumnSpacing = new Bindable<float>(ScrollingNotePlayfield.COLUMN_SPACING);

        [BackgroundDependencyLoader]
        private void load(IBeatmap beatmap, ISkinSource skin)
        {
            // todo : get from beatmap.
            const int columns = 9;

            bindableColumnHeight.BindTo(skin.GetConfig<KaraokeSkinConfigurationLookup, float>(new KaraokeSkinConfigurationLookup(columns, LegacyKaraokeSkinConfigurationLookups.ColumnHeight)));
            bindableColumnSpacing.BindTo(skin.GetConfig<KaraokeSkinConfigurationLookup, float>(new KaraokeSkinConfigurationLookup(columns, LegacyKaraokeSkinConfigurationLookups.ColumnSpacing)));

            bindableColumnHeight.BindValueChanged(e => updatePositionCalculator());
            bindableColumnSpacing.BindValueChanged(e => updatePositionCalculator());

            updatePositionCalculator();

            void updatePositionCalculator()
                => position.Value = new NotePositionCalculator(columns, bindableColumnHeight.Value, bindableColumnSpacing.Value);
        }
    }
}
