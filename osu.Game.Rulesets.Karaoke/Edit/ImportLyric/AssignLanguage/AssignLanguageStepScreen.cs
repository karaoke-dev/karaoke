// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.ImportLyric.AssignLanguage
{
    public class AssignLanguageStepScreen : LyricImporterStepScreenWithLyricEditor
    {
        public override string Title => "Language";

        public override string ShortTitle => "Language";

        public override LyricImporterStep Step => LyricImporterStep.AssignLanguage;

        public override IconUsage Icon => FontAwesome.Solid.Globe;

        [Cached(typeof(ILyricRubyChangeHandler))]
        private readonly LyricRubyChangeHandler lyricRubyChangeHandler;

        [Cached(typeof(ILyricRomajiChangeHandler))]
        private readonly LyricRomajiChangeHandler lyricRomajiChangeHandler;

        [Resolved]
        private EditorBeatmap beatmap { get; set; }

        public AssignLanguageStepScreen()
        {
            AddInternal(lyricRubyChangeHandler = new LyricRubyChangeHandler());
            AddInternal(lyricRomajiChangeHandler = new LyricRomajiChangeHandler());
        }

        protected override TopNavigation CreateNavigation()
            => new AssignLanguageNavigation(this);

        protected override Drawable CreateContent()
            => base.CreateContent().With(_ =>
            {
                LyricEditor.Mode = LyricEditorMode.Language;
            });

        protected override void LoadComplete()
        {
            base.LoadComplete();
            Navigation.State = NavigationState.Initial;
            AskForAutoAssignLanguage();
        }

        public override void Complete()
        {
            // Check is need to go to generate ruby/romaji step or just skip.
            if (lyricRubyChangeHandler.CanGenerate() || lyricRomajiChangeHandler.CanGenerate())
            {
                ScreenStack.Push(LyricImporterStep.GenerateRuby);
            }
            else
            {
                ScreenStack.Push(LyricImporterStep.GenerateTimeTag);
            }
        }

        internal void AskForAutoAssignLanguage()
        {
            DialogOverlay.Push(new UseLanguageDetectorPopupDialog(ok =>
            {
                if (!ok)
                    return;

                LyricManager.AutoDetectLyricLanguage();
                Navigation.State = NavigationState.Done;
            }));
        }

        public class AssignLanguageNavigation : TopNavigation<AssignLanguageStepScreen>
        {
            private const string auto_assign_language = "AUTO_ASSIGN_LANGUAGE";

            public AssignLanguageNavigation(AssignLanguageStepScreen screen)
                : base(screen)
            {
            }

            protected override NavigationTextContainer CreateTextContainer()
                => new AssignLanguageTextFlowContainer(Screen);

            protected override void UpdateState(NavigationState value)
            {
                base.UpdateState(value);

                NavigationText = value switch
                {
                    NavigationState.Initial => $"Try to select left side to mark lyric's language, or click [{auto_assign_language}] to let system auto detect lyric language.",
                    NavigationState.Working => $"Almost there, you can still click [{auto_assign_language}] to re-detect each lyric's language.",
                    NavigationState.Done => "Cool! Seems all lyric has it's own language. Go to next step to generate ruby.",
                    NavigationState.Error => "Oops, seems cause some error in here.",
                    _ => throw new ArgumentOutOfRangeException(nameof(value))
                };
            }

            private class AssignLanguageTextFlowContainer : NavigationTextContainer
            {
                public AssignLanguageTextFlowContainer(AssignLanguageStepScreen screen)
                {
                    AddLinkFactory(auto_assign_language, "language detector", screen.AskForAutoAssignLanguage);
                }
            }
        }
    }
}
