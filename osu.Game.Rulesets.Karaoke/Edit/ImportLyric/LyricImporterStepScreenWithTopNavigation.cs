// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace osu.Game.Rulesets.Karaoke.Edit.ImportLyric
{
    public abstract class LyricImporterStepScreenWithTopNavigation : LyricImporterStepScreen
    {
        protected LyricImporterStepScreenWithTopNavigation()
        {
            InternalChild = new GridContainer
            {
                RelativeSizeAxes = Axes.Both,
                RowDimensions = new[]
                {
                    new Dimension(GridSizeMode.Absolute, 40),
                    new Dimension()
                },
                Content = new[]
                {
                    new Drawable[]
                    {
                        CreateNavigation(),
                    },
                    new[]
                    {
                        CreateContent(),
                    }
                }
            };
        }

        protected abstract TopNavigation CreateNavigation();

        protected abstract Drawable CreateContent();
    }
}
