// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Compose.Toolbar;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Compose
{
    public class SpecialActionToolbar : CompositeDrawable
    {
        public const int HEIGHT = 26;
        public const int ICON_SPACING = 2;
        public const int ICON_SIZE = HEIGHT - ICON_SPACING * 2;

        private readonly IBindable<LyricEditorMode> bindableMode = new Bindable<LyricEditorMode>();

        private readonly Box background;

        public SpecialActionToolbar()
        {
            AutoSizeAxes = Axes.Both;

            InternalChildren = new Drawable[]
            {
                background = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                },
                new FillFlowContainer
                {
                    AutoSizeAxes = Axes.Both,
                    Padding = new MarginPadding(5),
                    Spacing = new Vector2(5),
                    Children = new Drawable[]
                    {
                        new TogglePropertyPanelButton(),
                        new ToggleInvalidInfoPanelButton(),
                    }
                }
            };
        }

        [BackgroundDependencyLoader(true)]
        private void load(ILyricEditorState state, LyricEditorColourProvider colourProvider)
        {
            bindableMode.BindTo(state.BindableMode);
            bindableMode.BindValueChanged(x =>
            {
                background.Colour = colourProvider.Background2(state.Mode);

                // todo: add different toolbar by mode.
            }, true);
        }
    }
}
