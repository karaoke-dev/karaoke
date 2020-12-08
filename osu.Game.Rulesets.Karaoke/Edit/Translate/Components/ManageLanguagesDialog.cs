// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Graphics.Containers;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Translate.Components
{
    // todo : move to other place
    /*
    public class ManageLanguagesDialog : TitleFocusedOverlayContainer
    {
        protected override string Title => "Manage translates";

        public ManageLanguagesDialog()
        {
            RelativeSizeAxes = Axes.Both;
            Size = new Vector2(0.5f, 0.8f);
        }

        [BackgroundDependencyLoader]
        private void load(TranslateManager translateManager)
        {
            Children = new Drawable[]
            {
                new DrawableLanguageList
                {
                    RelativeSizeAxes = Axes.Both,
                    Items = { BindTarget = translateManager?.Languages ?? new BindableList<BeatmapSetOnlineLanguage>() }
                }
            };
        }
    }
    */
}
