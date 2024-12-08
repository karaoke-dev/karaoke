// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Screens;
using osu.Game.Beatmaps;
using osu.Game.Screens.Play;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Import.Lyrics;

public partial class LyricImporter : ScreenWithBeatmapBackground
{
    private readonly ImportLyricOverlay importLyricOverlay;

    public override bool AllowUserExit => false;

    public override bool HideOverlaysOnEnter => true;

    public override bool DisallowExternalBeatmapRulesetChanges => true;

    public override bool? ApplyModTrackAdjustments => false;

    public Action<IBeatmap>? OnImportFinished;

    public LyricImporter()
    {
        InternalChild = importLyricOverlay = new ImportLyricOverlay
        {
            OnImportFinished = b =>
            {
                OnImportFinished?.Invoke(b);
            },
            OverlayClosed = this.Exit,
        };
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();
        importLyricOverlay.Show();
    }
}
