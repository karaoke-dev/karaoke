// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Skinning.Components;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Layout.Components
{
    public class ManageLanguagesDialog : TitleFocusedOverlayContainer
    {
        protected override string Title => "Manage layouts";

        public ManageLanguagesDialog()
        {
            RelativeSizeAxes = Axes.Both;
            Size = new Vector2(0.5f, 0.8f);
        }

        [BackgroundDependencyLoader]
        private void load(LayoutManager layoutManager)
        {
            Children = new Drawable[]
            {
                new DrawableLayoutList
                {
                    RelativeSizeAxes = Axes.Both,
                    Items = { BindTarget = layoutManager?.Layouts ?? new BindableList<KaraokeLayout>() }
                }
            };
        }
    }
}
