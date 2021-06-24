// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.RubyRomaji;

namespace osu.Game.Rulesets.Karaoke.Edit.ImportLyric.GenerateRubyRomaji
{
    public class GenerateRubyRomajiSubScreen : ImportLyricSubScreenWithLyricEditor
    {
        public override string Title => "Generate ruby";

        public override string ShortTitle => "Generate ruby";

        public override ImportLyricStep Step => ImportLyricStep.GenerateRuby;

        public override IconUsage Icon => FontAwesome.Solid.Gem;

        [Cached]
        protected readonly RubyRomajiManager RubyRomajiManager;

        public GenerateRubyRomajiSubScreen()
        {
            AddInternal(RubyRomajiManager = new RubyRomajiManager());
        }

        protected override TopNavigation CreateNavigation()
            => new GenerateRubyNavigation(this);

        protected override Drawable CreateContent()
            => base.CreateContent().With(x =>
            {
                LyricEditor.Mode = LyricEditorMode.EditRomaji;
            });

        protected override void LoadComplete()
        {
            base.LoadComplete();
            Navigation.State = NavigationState.Initial;

            // Asking auto-generate ruby or romaji.
            if (RubyRomajiManager.CanAutoGenerateRuby())
                AskForAutoGenerateRuby();
            else
                AskForAutoGenerateRomaji();
        }

        public override void Complete()
        {
            ScreenStack.Push(ImportLyricStep.GenerateTimeTag);
        }

        internal void AskForAutoGenerateRuby()
        {
            LyricEditor.Mode = LyricEditorMode.EditRuby;

            DialogOverlay.Push(new UseAutoGenerateRubyPopupDialog(ok =>
            {
                if (!ok)
                    return;

                RubyRomajiManager.AutoGenerateLyricRuby();
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

                RubyRomajiManager.AutoGenerateLyricRomaji();
                Navigation.State = NavigationState.Done;
            }));
        }

        public class GenerateRubyNavigation : TopNavigation<GenerateRubyRomajiSubScreen>
        {
            private const string auto_generate_ruby = "AUTO_GENERATE_RUBY";
            private const string auto_generate_romaji = "AUTO_GENERATE_ROMAJI";

            public GenerateRubyNavigation(GenerateRubyRomajiSubScreen screen)
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
                public GenerateRubyTextFlowContainer(GenerateRubyRomajiSubScreen screen)
                {
                    AddLinkFactory(auto_generate_ruby, "auto generate ruby", screen.AskForAutoGenerateRuby);
                    AddLinkFactory(auto_generate_romaji, "auto generate romaji", screen.AskForAutoGenerateRomaji);
                }
            }
        }
    }
}
