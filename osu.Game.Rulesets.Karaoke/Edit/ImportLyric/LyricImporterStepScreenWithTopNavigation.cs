// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace osu.Game.Rulesets.Karaoke.Edit.ImportLyric
{
    public abstract class LyricImporterStepScreenWithTopNavigation : LyricImporterStepScreen
    {
        protected LyricImporterStepScreenWithTopNavigation()
        {
            Padding = new MarginPadding(10);
            InternalChild = new GridContainer
            {
                RelativeSizeAxes = Axes.Both,
                RowDimensions = new[]
                {
                    new Dimension(GridSizeMode.Absolute, 40),
                    new Dimension(GridSizeMode.Absolute, 10),
                    new Dimension()
                },
                Content = new[]
                {
                    new Drawable[]
                    {
                        CreateNavigation(),
                    },
                    Array.Empty<Drawable>(),
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
