// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Import.Lyrics.GenerateRuby;

public partial class GenerateRubyStepScreen : LyricImporterStepScreenWithLyricEditor
{
    public override string Title => "Generate ruby";

    public override LyricImporterStep Step => LyricImporterStep.GenerateRuby;

    public override IconUsage Icon => FontAwesome.Solid.Gem;

    [Cached(typeof(ILyricPropertyAutoGenerateChangeHandler))]
    private readonly LyricPropertyAutoGenerateChangeHandler lyricPropertyAutoGenerateChangeHandler;

    public GenerateRubyStepScreen()
    {
        AddInternal(lyricPropertyAutoGenerateChangeHandler = new LyricPropertyAutoGenerateChangeHandler());
    }

    protected override TopNavigation CreateNavigation()
        => new GenerateRubyNavigation(this);

    protected override Drawable CreateContent()
        => base.CreateContent().With(_ =>
        {
            SwitchLyricEditorMode(LyricEditorMode.EditRomanisation);
        });

    protected override void LoadComplete()
    {
        base.LoadComplete();

        // Asking auto-generate ruby.
        if (lyricPropertyAutoGenerateChangeHandler.CanGenerate(AutoGenerateType.AutoGenerateRubyTags))
            AskForAutoGenerateRuby();
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
}
