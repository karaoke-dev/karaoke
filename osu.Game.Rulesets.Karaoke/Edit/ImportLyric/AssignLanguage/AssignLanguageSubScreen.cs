// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics;

namespace osu.Game.Rulesets.Karaoke.Edit.ImportLyric.AssignLanguage
{
    public class AssignLanguageSubScreen : ImportLyricSubScreenWithLyricEditor
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

        protected override TopNavigation CreateNavigation()
            => new AssignLanguageNavigation(this);

        protected override Drawable CreateContent()
            => base.CreateContent().With(x =>
            {
                LyricEditor.Mode = Mode.ViewMode;
                LyricEditor.LyricFastEditMode = LyricFastEditMode.Language;
            });

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

        internal void AskForAutoAssignLanguage()
        {
            DialogOverlay.Push(new UseLanguageDetectorPopupDialog(ok =>
            {
                if (ok)
                {
                    LyricManager.AutoDetectLyricLanguage();
                    Navigation.State = NavigationState.Done;
                }
            }));
        }

        public class AssignLanguageNavigation : TopNavigation<AssignLanguageSubScreen>
        {
            private const string auto_assign_language = "AUTO_ASSIGN_LANGUAGE";

            public AssignLanguageNavigation(AssignLanguageSubScreen screen)
                : base(screen)
            {
            }

            protected override NavigationTextContainer CreateTextContainer()
                => new AssignLanguageTextFlowContainer(Screen);

            protected override void UpdateState(NavigationState value)
            {
                base.UpdateState(value);

                switch (value)
                {
                    case NavigationState.Initial:
                        NavigationText = $"Try to select left side to mark lyric's language, or click [{auto_assign_language}] to let system auto detect lyric language.";
                        break;

                    case NavigationState.Working:
                        NavigationText = $"Almost there, you can still click [{auto_assign_language}] to re-detect each lyric's language.";
                        break;

                    case NavigationState.Done:
                        NavigationText = "Cool! Seems all lyric has it's own language. Go to next step to generate ruby.";
                        break;

                    case NavigationState.Error:
                        NavigationText = "Oops, seems cause some error in here.";
                        break;
                }
            }

            private class AssignLanguageTextFlowContainer : NavigationTextContainer
            {
                public AssignLanguageTextFlowContainer(AssignLanguageSubScreen screen)
                {
                    AddLinkFactory(auto_assign_language, "language detector", () => screen.AskForAutoAssignLanguage());
                }
            }
        }
    }
}
