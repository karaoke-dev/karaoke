// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Timing;
using osu.Game.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Edit.LyricEditor.Components;
using osu.Game.Rulesets.Karaoke.Objects;
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

        [BackgroundDependencyLoader]
        private void load(IFrameBasedClock framedClock)
        {
            container.Clock = framedClock;

            foreach (var obj in beatmap.HitObjects)
                Schedule(() => addHitObject(obj));
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            beatmap.HitObjectAdded += addHitObject;
            beatmap.HitObjectRemoved += removeHitObject;
        }

        private void addHitObject(HitObject hitObject)
        {
            // see how `DrawableEditRulesetWrapper` do
            if (hitObject is LyricLine lyric)
            {
                container.Add(new LyricControl(lyric)
                {
                    RelativeSizeAxes = Axes.X,
                });
            }
        }

        private void removeHitObject(HitObject hitObject)
        {
            
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

            if (beatmap != null)
            {
                beatmap.HitObjectAdded -= addHitObject;
                beatmap.HitObjectRemoved -= removeHitObject;
            }
        }
    }
}
