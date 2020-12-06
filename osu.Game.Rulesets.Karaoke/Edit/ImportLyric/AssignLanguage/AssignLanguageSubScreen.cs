// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Timing;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics;

namespace osu.Game.Rulesets.Karaoke.Edit.ImportLyric.AssignLanguage
{
    public class AssignLanguageSubScreen : ImportLyricSubScreenWithTopNavigation
    {
        public override string Title => "Language";

        public override string ShortTitle => "Language";

        public override ImportLyricStep Step => ImportLyricStep.AssignLanguage;

        public override IconUsage Icon => FontAwesome.Solid.Globe;

        [Cached]
        protected readonly LyricManager LyricManager;

        public AssignLanguageSubScreen()
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
            => new AssignLanguageNavigation(this);

        protected override Drawable CreateContent()
            => new LyricEditor
            {
                RelativeSizeAxes = Axes.Both,
                Mode = Mode.EditMode,
                LyricFastEditMode = LyricFastEditMode.Language,
            };

        protected override void LoadComplete()
        {
            base.LoadComplete();
            Navigation.State = NavigationState.Initial;
            AskForAutoAssignLanguage();
        }

        public override void Complete()
        {
            ScreenStack.Push(ImportLyricStep.GenerateRuby);
        }

        protected void AskForAutoAssignLanguage()
        {
            DialogOverlay.Push(new UseLanguageDetectorPopupDialog(ok =>
            {
                if (ok)
                    LyricManager.AutoDetectLyricLanguage();
            }));
        }

        public class AssignLanguageNavigation : TopNavigation
        {
            public AssignLanguageNavigation(ImportLyricSubScreen screen)
                : base(screen)
            {
            }

            protected override void UpdateState(NavigationState value)
            {
                base.UpdateState(value);

                switch (value)
                {
                    case NavigationState.Initial:
                        NavigationText = "Try to select left side to mark lyric's language.";
                        break;

                    case NavigationState.Working:
                        NavigationText = "Almost there/";
                        break;

                    case NavigationState.Done:
                        NavigationText = "Cool! Seems all lyric has it's own language. Go to next step to generate ruby.";
                        break;

                    case NavigationState.Error:
                        NavigationText = "Oops, seems cause some error in here.";
                        break;
                }
            }
        }
    }
}
