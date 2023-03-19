// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Edit.Setup.Components;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Screens.Edit.Setup;

namespace osu.Game.Rulesets.Karaoke.Edit.Setup
{
    public partial class KaraokeSetupSection : RulesetSetupSection
    {
        private KaraokeBeatmap karaokeBeatmap => EditorBeatmapUtils.GetPlayableBeatmap(Beatmap);

        private LabelledSwitchButton scorable;
        private LabelledSingerList singerList;

        [Cached(typeof(IKaraokeBeatmapResourcesProvider))]
        private KaraokeBeatmapResourcesProvider karaokeBeatmapResourcesProvider;

        public KaraokeSetupSection()
            : base(new KaraokeRuleset().RulesetInfo)
        {
            AddInternal(karaokeBeatmapResourcesProvider = new KaraokeBeatmapResourcesProvider());
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Children = new Drawable[]
            {
                scorable = new LabelledSwitchButton
                {
                    Label = "Scorable",
                    Description = "Will not show score playfield if the option is unchecked.",
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

            scorable.Current.BindValueChanged(_ => updateBeatmap());
        }

        private void updateBeatmap()
        {
            // todo: update the value.
            // karaokeBeatmap.Scorable = scorable.Current.Value;
        }
    }
}
