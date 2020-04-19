// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics.Containers;
using osu.Game.Rulesets.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.LyricEditor
{
    public class LyricEditorScreen : EditorScreenWithTimeline
    {
        private FillFlowContainer container;

        [Resolved]
        private EditorBeatmap beatmap { get; set; }

        protected override Drawable CreateMainContent()
        {
            return new OsuScrollContainer
            {
                RelativeSizeAxes = Axes.Both,
                Child = container = new FillFlowContainer
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Direction = FillDirection.Vertical,
                }
            };
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            beatmap.HitObjectAdded += addHitObject;
            beatmap.HitObjectRemoved += removeHitObject;

            container.Add(new Container
            {
                RelativeSizeAxes = Axes.X,
                Height = 1000
            });
        }

        private void addHitObject(HitObject hitObject)
        {
            // see how `DrawableEditRulesetWrapper` do
        }

        private void removeHitObject(HitObject hitObject)
        {
            
        }
    }
}
