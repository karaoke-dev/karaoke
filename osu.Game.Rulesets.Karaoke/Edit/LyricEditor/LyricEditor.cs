// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Objects;
using osu.Game.Screens.Edit;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Edit.LyricEditor
{
    public class LyricEditor : Container
    {
        [Resolved]
        private EditorBeatmap beatmap { get; set; }

        private KaraokeLyricEditorSkin skin;
        private LyricRearrangeableListContainer container;

        public LyricEditor()
        {
            Child = new SkinProvidingContainer(skin = new KaraokeLyricEditorSkin())
            {
                RelativeSizeAxes = Axes.Both,
                Child = container = new LyricRearrangeableListContainer
                {
                    RelativeSizeAxes = Axes.Both,
                }
            };
        }

        [BackgroundDependencyLoader]
        private void load()
        {
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
            if (hitObject is Lyric lyric)
            {
                container.Items.Add(lyric);
            }
        }

        private void removeHitObject(HitObject hitObject)
        {
            if (!(hitObject is Lyric lyric))
                return;

            container.Items.Remove(lyric);
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

            if (beatmap == null)
                return;

            beatmap.HitObjectAdded -= addHitObject;
            beatmap.HitObjectRemoved -= removeHitObject;
        }

        public float FontSize
        {
            get => skin.FontSize;
            set => skin.FontSize = value;
        }
    }
}
