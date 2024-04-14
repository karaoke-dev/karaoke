// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osu.Game.Beatmaps;
using osu.Game.Input.Bindings;
using osu.Game.Screens.Play;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Import.Lyrics;

public partial class LyricImporter : ScreenWithBeatmapBackground, IKeyBindingHandler<GlobalAction>
{
    private readonly ImportLyricOverlay importLyricOverlay;

    // Hide the back button because we cannot show it only in the first step.
    public override bool AllowBackButton => false;

    public event Action<IBeatmap>? OnImportFinished;

    public LyricImporter()
    {
        InternalChild = importLyricOverlay = new ImportLyricOverlay
        {
            OnImportCancelled = this.Exit,
            OnImportFinished = b =>
            {
                this.Exit();
                OnImportFinished?.Invoke(b);
            },
        };
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();
        importLyricOverlay.Show();
    }

    public bool OnPressed(KeyBindingPressEvent<GlobalAction> e)
    {
        if (e.Repeat)
            return false;

        switch (e.Action)
        {
            case GlobalAction.Back:
                if (importLyricOverlay.IsFirstStep())
                {
                    // the better UX behavior should be move to the previous step.
                    // But it will not asking.
                    return false;

                    // todo: implement.
                    // ScreenStack.Exit();
                }

                this.Exit();
                return true;

            default:
                return false;
        }
    }

    public void OnReleased(KeyBindingReleaseEvent<GlobalAction> e)
    {
    }
}
