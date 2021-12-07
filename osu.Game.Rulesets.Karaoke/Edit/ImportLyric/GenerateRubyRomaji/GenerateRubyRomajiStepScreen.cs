// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics;

namespace osu.Game.Rulesets.Karaoke.Edit.ImportLyric.GenerateRubyRomaji
{
    public class GenerateRubyRomajiStepScreen : LyricImporterStepScreenWithLyricEditor
    {
        public override string Title => "Generate ruby";

        public override string ShortTitle => "Generate ruby";

        public override LyricImporterStep Step => LyricImporterStep.GenerateRuby;

        public override IconUsage Icon => FontAwesome.Solid.Gem;

        [Cached(typeof(ILyricRubyChangeHandler))]
        private readonly LyricRubyChangeHandler lyricRubyChangeHandler;

        [Cached(typeof(ILyricRomajiChangeHandler))]
        private readonly LyricRomajiChangeHandler lyricRomajiChangeHandler;

        public GenerateRubyRomajiStepScreen()
        {
            AddInternal(lyricRubyChangeHandler = new LyricRubyChangeHandler());
            AddInternal(lyricRomajiChangeHandler = new LyricRomajiChangeHandler());
        }

        protected override TopNavigation CreateNavigation()
            => new GenerateRubyNavigation(this);

        protected override Drawable CreateContent()
            => base.CreateContent().With(_ =>
            {
                LyricEditor.Mode = LyricEditorMode.EditRomaji;
            });

        protected override void LoadComplete()
        {
            base.LoadComplete();
            Navigation.State = NavigationState.Initial;

            // Asking auto-generate ruby or romaji.
            if (lyricRubyChangeHandler.CanGenerate())
                AskForAutoGenerateRuby();
            else if (lyricRomajiChangeHandler.CanGenerate())
                AskForAutoGenerateRomaji();
        }

        public override void Complete()
        {
            ScreenStack.Push(LyricImporterStep.GenerateTimeTag);
        }

        internal void AskForAutoGenerateRuby()
        {
            LyricEditor.Mode = LyricEditorMode.EditRuby;

            DialogOverlay.Push(new UseAutoGenerateRubyPopupDialog(ok =>
            {
                if (!ok)
                    return;

                // todo: select all lyrics or switch to select mode.

                lyricRubyChangeHandler.AutoGenerate();
                Navigation.State = NavigationState.Done;
            }));
        }

        internal void AskForAutoGenerateRomaji()
        {
            LyricEditor.Mode = LyricEditorMode.EditRomaji;

            DialogOverlay.Push(new UseAutoGenerateRomajiPopupDialog(ok =>
            {
                if (!ok)
                    return;

                // todo: select all lyrics or switch to select mode.

                lyricRomajiChangeHandler.AutoGenerate();
                Navigation.State = NavigationState.Done;
            }));
        }

        public class GenerateRubyNavigation : TopNavigation<GenerateRubyRomajiStepScreen>
        {
            private const string auto_generate_ruby = "AUTO_GENERATE_RUBY";
            private const string auto_generate_romaji = "AUTO_GENERATE_ROMAJI";

            public GenerateRubyNavigation(GenerateRubyRomajiStepScreen screen)
                : base(screen)
            {
            }

            protected override NavigationTextContainer CreateTextContainer()
                => new GenerateRubyTextFlowContainer(Screen);

            protected override void UpdateState(NavigationState value)
            {
                base.UpdateState(value);

                switch (value)
                {
                    case NavigationState.Initial:
                        NavigationText = $"Lazy to typing ruby? Press [{auto_generate_ruby}] or [{auto_generate_romaji}] to auto-generate ruby and romaji. It's very easy.";
                        break;

                    case NavigationState.Working:
                    case NavigationState.Done:
                        NavigationText = $"Go to next step to generate time-tag. Messing around? Press [{auto_generate_ruby}] or [{auto_generate_romaji}] again.";
                        break;

                    case NavigationState.Error:
                        NavigationText = "Oops, seems cause some error in here.";
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(value));
                }
            }

            protected override bool AbleToNextStep(NavigationState value)
                => value == NavigationState.Initial || value == NavigationState.Working || value == NavigationState.Done;

            private class GenerateRubyTextFlowContainer : NavigationTextContainer
            {
                public GenerateRubyTextFlowContainer(GenerateRubyRomajiStepScreen screen)
                {
                    AddLinkFactory(auto_generate_ruby, "auto generate ruby", screen.AskForAutoGenerateRuby);
                    AddLinkFactory(auto_generate_romaji, "auto generate romaji", screen.AskForAutoGenerateRomaji);
                }
            }
        }
    }
}
