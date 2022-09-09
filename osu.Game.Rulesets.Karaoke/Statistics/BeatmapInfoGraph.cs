// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Linq;
using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osu.Game.Beatmaps;
using static osu.Game.Screens.Select.BeatmapInfoWedge;

namespace osu.Game.Rulesets.Karaoke.Statistics
{
    public class BeatmapInfoGraph : ClickableContainer
    {
        [Resolved(CanBeNull = true)]
        private OsuGame game { get; set; }

        private readonly IBeatmap beatmap;

        public BeatmapInfoGraph(IBeatmap beatmap)
        {
            this.beatmap = beatmap;
            Masking = true;
            CornerRadius = 5;
        }

        protected override bool OnClick(ClickEvent e)
        {
            game?.ShowBeatmap(beatmap.BeatmapInfo.OnlineID);
            return base.OnClick(e);
        }

        [BackgroundDependencyLoader(true)]
        private void load([CanBeNull] IBindable<WorkingBeatmap> workingBeatmap, [CanBeNull] BeatmapDifficultyCache difficultyCache)
        {
            if (workingBeatmap == null || difficultyCache == null)
                return;

            var beatmapDifficulty = difficultyCache.GetBindableDifficulty(beatmap.BeatmapInfo).Value;
            LoadComponentAsync(new BeatmapInfoWedge(workingBeatmap.Value), Add);
        }

        public class BeatmapInfoWedge : WedgeInfoText
        {
            public BeatmapInfoWedge(WorkingBeatmap beatmap)
                : base(beatmap, new KaraokeRuleset().RulesetInfo)
            {
            }

            [BackgroundDependencyLoader]
            private void load()
            {
                // Adjust metadata size
                var centerMetadata = Children.FirstOrDefault(x => x.Name == "Centre-aligned metadata");
                if (centerMetadata != null)
                    centerMetadata.Y = -20;

                // Use tricky to add extra info.
                if (InfoLabelContainer == null)
                    return;

                var shouldBeRemovedLabel = InfoLabelContainer.Children.OfType<InfoLabel>()
                                                             .Where(x => x.TooltipText == "Note" || x.TooltipText == "This beatmap is not scorable.").ToList();
                InfoLabelContainer.RemoveRange(shouldBeRemovedLabel, true);
            }

            protected FillFlowContainer InfoLabelContainer
                => (Children.LastOrDefault() as FillFlowContainer)?.LastOrDefault() as FillFlowContainer;
        }
    }
}
