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

        [Cached(typeof(ILyricAutoGenerateChangeHandler))]
        private readonly LyricAutoGenerateChangeHandler lyricAutoGenerateChangeHandler;

        [Cached(typeof(ILyricRubyTagsChangeHandler))]
        private readonly LyricRubyTagsChangeHandler lyricRubyTagsChangeHandler;

        [Cached(typeof(ILyricRomajiTagsChangeHandler))]
        private readonly LyricRomajiTagsChangeHandler lyricRomajiTagsChangeHandler;

        public GenerateRubyRomajiStepScreen()
        {
            AddInternal(lyricAutoGenerateChangeHandler = new LyricAutoGenerateChangeHandler());
            AddInternal(lyricRubyTagsChangeHandler = new LyricRubyTagsChangeHandler());
            AddInternal(lyricRomajiTagsChangeHandler = new LyricRomajiTagsChangeHandler());
        }

        protected override TopNavigation CreateNavigation()
            => new GenerateRubyRomajiNavigation(this);

        protected override Drawable CreateContent()
            => base.CreateContent().With(_ =>
            {
                SwitchLyricEditorMode(LyricEditorMode.EditRomaji);
            });

        protected override void LoadComplete()
        {
            base.LoadComplete();

            // Asking auto-generate ruby or romaji.
            if (lyricAutoGenerateChangeHandler.CanGenerate(LyricAutoGenerateProperty.AutoGenerateRubyTags))
                AskForAutoGenerateRuby();
            else if (lyricAutoGenerateChangeHandler.CanGenerate(LyricAutoGenerateProperty.AutoGenerateRomajiTags))
                AskForAutoGenerateRomaji();
        }

        public override void Complete()
        {
            ScreenStack.Push(LyricImporterStep.GenerateTimeTag);
        }

        internal void AskForAutoGenerateRuby()
        {
            SwitchLyricEditorMode(LyricEditorMode.EditRuby);

            DialogOverlay.Push(new UseAutoGenerateRubyPopupDialog(ok =>
            {
                if (!ok)
                    return;

                PrepareAutoGenerate();
            }));
        }

        internal void AskForAutoGenerateRomaji()
        {
            SwitchLyricEditorMode(LyricEditorMode.EditRomaji);

            DialogOverlay.Push(new UseAutoGenerateRomajiPopupDialog(ok =>
            {
                if (!ok)
                    return;

                PrepareAutoGenerate();
            }));
        }
    }
}
