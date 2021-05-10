// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics;
using osu.Game.Rulesets.Karaoke.Graphics.Cursor;
using osu.Game.Rulesets.Karaoke.Objects;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Singers.Components.SingerLyricEditor
{
    public class LyricTimelineHitObjectBlueprint : SelectionBlueprint<Lyric>, IHasCustomTooltip
    {
        public LyricTimelineHitObjectBlueprint(Lyric item)
            : base(item)
        {
            Anchor = Anchor.CentreLeft;
            Origin = Anchor.CentreLeft;

            RelativePositionAxes = Axes.X;
            RelativeSizeAxes = Axes.Y;
            AutoSizeAxes = Axes.X;

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
                        Text = item?.Text
                    }
                }
            });
        }

        [BackgroundDependencyLoader]
        private void load(LyricManager lyricManager, SingerLyricEditor editor)
        {
            X = (float)Item.LyricStartTime;
            Item.SingersBindable.BindValueChanged(e =>
            {
                // Check is lyric contains this singer, or default singer
                var isSingerMatch = lyricManager.SingerInLyric(editor.Singer, Item);

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

        public object TooltipContent => Item;

        public ITooltip GetCustomTooltip() => new LyricTooltip();

        protected override void OnSelected()
        {
            // base logic hides selected blueprints when not selected, but timeline doesn't do that.
        }

        protected override void OnDeselected()
        {
            // base logic hides selected blueprints when not selected, but timeline doesn't do that.
        }

        public override Vector2 ScreenSpaceSelectionPoint => ScreenSpaceDrawQuad.TopLeft;
    }
}
