// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Graphics.Cursor;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Objects;
using osu.Game.Screens.Edit.Compose.Components.Timeline;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Singers.Components.Timeline
{
    public class LyricTimelineHitObjectBlueprint : TimelineHitObjectBlueprint, IHasCustomTooltip
    {
        private readonly Singer singer;

        public LyricTimelineHitObjectBlueprint(HitObject hitObject, Singer singer)
            : base(hitObject)
        {
            this.singer = singer;

            // Use tricky way to hide the timeline component.
            InternalChildren.ForEach(x => x.Alpha = 0);

            // todo : wait for better solution until some of child component is overridable.
            AddInternal(new Container
            {
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                RelativeSizeAxes = Axes.X,
                Height = 20,
                Masking = true,
                CornerRadius = 5,
                Children = new Drawable[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Color4.Gray,
                    },
                    new OsuSpriteText
                    {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        Margin = new MarginPadding { Left = 10 },
                        Text = (hitObject as Lyric)?.Text
                    }
                }
            });
        }

        [BackgroundDependencyLoader]
        private void load(SingerManager singerManager)
        {
            if (HitObject is Lyric lyric)
            {
                lyric.SingersBindable.BindValueChanged(e =>
                {
                    // Check if lyric contains this singer, or default singer
                    var isSingerMatch = singerManager.SingerInLyric(singer, lyric);

                    if (isSingerMatch)
                    {
                        Show();
                    }
                    else
                    {
                        Hide();
                    }
                }, true);
            }
        }

        public object TooltipContent => HitObject;

        public ITooltip GetCustomTooltip() => new LyricTooltip();
    }
}
