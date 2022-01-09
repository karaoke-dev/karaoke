// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

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

        [Cached(typeof(ILyricRubyTagsChangeHandler))]
        private readonly LyricRubyTagsChangeHandler lyricRubyTagsChangeHandler;

        [Cached(typeof(ILyricRomajiTagsChangeHandler))]
        private readonly LyricRomajiTagsChangeHandler lyricRomajiTagsChangeHandler;

        public GenerateRubyRomajiStepScreen()
        {
            AddInternal(lyricRubyTagsChangeHandler = new LyricRubyTagsChangeHandler());
            AddInternal(lyricRomajiTagsChangeHandler = new LyricRomajiTagsChangeHandler());
        }

        protected override TopNavigation CreateNavigation()
            => new GenerateRubyRomajiNavigation(this);

        protected override Drawable CreateContent()
            => base.CreateContent().With(_ =>
            {
                LyricEditor.Mode = LyricEditorMode.EditRomaji;
            });

        protected override void LoadComplete()
        {
            base.LoadComplete();

            // Asking auto-generate ruby or romaji.
            if (lyricRubyTagsChangeHandler.CanGenerate())
                AskForAutoGenerateRuby();
            else if (lyricRomajiTagsChangeHandler.CanGenerate())
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

                lyricRubyTagsChangeHandler.AutoGenerate();
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

                lyricRomajiTagsChangeHandler.AutoGenerate();
                Navigation.State = NavigationState.Done;
            }));
        }
    }
}
