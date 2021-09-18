// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Edit.Setup.Components;
using osu.Game.Screens.Edit.Setup;

namespace osu.Game.Rulesets.Karaoke.Edit.Setup
{
    public class KaraokeSetupSection : RulesetSetupSection
    {
        private KaraokeBeatmap karaokeBeatmap => Beatmap.PlayableBeatmap as KaraokeBeatmap;

        private LabelledSwitchButton saitenable;
        private LabelledSingerList singerList;

        public KaraokeSetupSection()
            : base(new KaraokeRuleset().RulesetInfo)
        {
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Children = new Drawable[]
            {
                saitenable = new LabelledSwitchButton
                {
                    Label = "Saitenable",
                    Description = "Will not show saiten playfield if the option is unchecked.",
                    Current = { Value = true }
                },
                singerList = new LabelledSingerList
                {
                    Label = "Singer list",
                    Description = "All the singers in beatmap.",
                    FixedLabelWidth = LABEL_WIDTH,
                    SingerNamePrefix = "#"
                }
            };
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            saitenable.Current.BindValueChanged(_ => updateBeatmap());
        }

        private void updateBeatmap()
        {
            // todo: update the value.
            // karaokeBeatmap.Saitenable = saitenable.Current.Value;
        }
    }
}
