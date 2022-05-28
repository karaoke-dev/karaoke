// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Overlays;

namespace osu.Game.Rulesets.Karaoke.Edit
{
    /// <summary>
    /// Copied from EditorRoundedScreen
    /// todo: will remove this screen eventually because new editor design not have round screen style.
    /// </summary>
    public class KaraokeEditorRoundedScreen : KaraokeEditorScreen
    {
        public const int HORIZONTAL_PADDING = 100;

        private Container roundedContent;

        protected override Container<Drawable> Content => roundedContent;

        public KaraokeEditorRoundedScreen(KaraokeEditorScreenMode type)
            : base(type)
        {
        }

        [BackgroundDependencyLoader]
        private void load(OverlayColourProvider colourProvider)
        {
            base.Content.Add(new Container
            {
                RelativeSizeAxes = Axes.Both,
                Padding = new MarginPadding(50),
                Child = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Masking = true,
                    CornerRadius = 10,
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            Colour = colourProvider.Background3,
                            RelativeSizeAxes = Axes.Both,
                        },
                        roundedContent = new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                        },
                    }
                }
            });
        }
    }
}
