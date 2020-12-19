// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Timing;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics;

namespace osu.Game.Rulesets.Karaoke.Edit.ImportLyric.EditLyric
{
    public class EditLyricSubScreen : ImportLyricSubScreenWithTopNavigation
    {
        public override string Title => "Edit lyric";

        public override string ShortTitle => "Edit";

        public override ImportLyricStep Step => ImportLyricStep.EditLyric;

        public override IconUsage Icon => FontAwesome.Solid.Globe;

        [Cached]
        protected readonly LyricManager LyricManager;

        public EditLyricSubScreen()
        {
            AddInternal(LyricManager = new LyricManager());
        }

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
        {
            var dependencies = new DependencyContainer(base.CreateChildDependencies(parent));
            var clock = new DecoupleableInterpolatingFramedClock { IsCoupled = false };
            dependencies.CacheAs<IAdjustableClock>(clock);
            dependencies.CacheAs<IFrameBasedClock>(clock);

            return dependencies;
        }

        protected override TopNavigation CreateNavigation()
            => new EditLyricNavigation(this);

        protected override Drawable CreateContent()
            => new LyricEditor
            {
                RelativeSizeAxes = Axes.Both,
                Mode = Mode.EditMode,
                LyricFastEditMode = LyricFastEditMode.None,
            };

        protected override void LoadComplete()
        {
            base.LoadComplete();
            Navigation.State = NavigationState.Initial;
        }

        public override void Complete()
        {
            ScreenStack.Push(ImportLyricStep.AssignLanguage);
        }

        public class EditLyricNavigation : TopNavigation
        {
            public EditLyricNavigation(EditLyricSubScreen screen)
                : base(screen)
            {
            }

            protected override void UpdateState(NavigationState value)
            {
                base.UpdateState(value);

                switch (value)
                {
                    case NavigationState.Initial:
                        NavigationText = "Check and edit lyric if needed.";
                        break;

                    case NavigationState.Working:
                    case NavigationState.Done:
                        NavigationText = "Cool!";
                        break;

                    case NavigationState.Error:
                        NavigationText = "Oops, seems cause some error in here.";
                        break;
                }
            }
        }
    }
}
