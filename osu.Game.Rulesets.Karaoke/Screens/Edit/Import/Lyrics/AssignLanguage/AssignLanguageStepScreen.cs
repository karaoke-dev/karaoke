// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Import.Lyrics.AssignLanguage;

public partial class AssignLanguageStepScreen : LyricImporterStepScreenWithLyricEditor
{
    public override string Title => "Language";

    public override string ShortTitle => "Language";

    public override LyricImporterStep Step => LyricImporterStep.AssignLanguage;

    public override IconUsage Icon => FontAwesome.Solid.Globe;

    [Cached(typeof(ILyricPropertyAutoGenerateChangeHandler))]
    private readonly LyricPropertyAutoGenerateChangeHandler lyricPropertyAutoGenerateChangeHandler;

    [Cached(typeof(ILyricLanguageChangeHandler))]
    private readonly LyricLanguageChangeHandler lyricLanguageChangeHandler;

    public AssignLanguageStepScreen()
    {
        AddInternal(lyricPropertyAutoGenerateChangeHandler = new LyricPropertyAutoGenerateChangeHandler());
        AddInternal(lyricLanguageChangeHandler = new LyricLanguageChangeHandler());
    }

    protected override TopNavigation CreateNavigation()
        => new AssignLanguageNavigation(this);

    protected override Drawable CreateContent()
        => base.CreateContent().With(_ =>
        {
            SwitchLyricEditorMode(LyricEditorMode.Language);
        });

    protected override void LoadComplete()
    {
        base.LoadComplete();
        AskForAutoAssignLanguage();
    }

    public override void Complete()
    {
        // Check is need to go to generate ruby/romaji step or just skip.
        if (lyricPropertyAutoGenerateChangeHandler.CanGenerate(AutoGenerateType.AutoGenerateRubyTags)
            || lyricPropertyAutoGenerateChangeHandler.CanGenerate(AutoGenerateType.AutoGenerateRomajiTags))
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

            PrepareAutoGenerate();
        }));
    }
}
