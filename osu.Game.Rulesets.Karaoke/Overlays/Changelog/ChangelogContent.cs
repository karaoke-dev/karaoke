// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Online.API.Requests.Responses;

namespace osu.Game.Rulesets.Karaoke.Overlays.Changelog
{
    /// <summary>
    /// Base change log content
    /// </summary>
    public abstract class ChangelogContent : FillFlowContainer
    {
        public Action<APIChangelogBuild> BuildSelected;

        protected void SelectBuild(APIChangelogBuild build) => BuildSelected?.Invoke(build);

        protected ChangelogContent()
        {
            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;
            Direction = FillDirection.Vertical;
        }
    }
}
