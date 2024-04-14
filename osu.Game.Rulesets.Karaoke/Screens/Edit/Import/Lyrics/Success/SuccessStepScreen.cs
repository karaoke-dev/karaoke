// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Import.Lyrics.Success;

public partial class SuccessStepScreen : LyricImporterStepScreen
{
    public override string Title => "Success";

    public override IconUsage Icon => FontAwesome.Regular.CheckCircle;

    [Resolved]
    private IImportStateResolver importStateResolver { get; set; } = null!;

    public override void Complete()
    {
        importStateResolver.Finish();
    }
}
