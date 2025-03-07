﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Rendering;
using osu.Framework.Graphics.Sprites;
using osu.Framework.IO.Stores;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.IO.Stores;
using osu.Game.Rulesets.Karaoke.Skinning.Fonts;
using osu.Game.Rulesets.Karaoke.Stages.Drawables;
using osu.Game.Rulesets.UI;

namespace osu.Game.Rulesets.Karaoke.UI;

/// <summary>
/// Having a place to get all user customize font.
/// todo : need to check will have better place or not.
/// </summary>
public partial class KaraokePlayfieldAdjustmentContainer : PlayfieldAdjustmentContainer
{
    [Resolved]
    private FontStore fontStore { get; set; } = null!;

    private KaraokeLocalFontStore localFontStore = null!;

    protected override Container<Drawable> Content => content;
    private readonly DrawableStage content;

    public KaraokePlayfieldAdjustmentContainer()
    {
        InternalChild = content = new DrawableStage
        {
            RelativeSizeAxes = Axes.Both,
        };
    }

    [BackgroundDependencyLoader]
    private void load(FontManager fontManager, IRenderer renderer, KaraokeRulesetConfigManager manager)
    {
        // get all font usage which wants to import.
        var targetImportFonts = new[]
        {
            manager.Get<FontUsage>(KaraokeRulesetSetting.MainFont),
            manager.Get<FontUsage>(KaraokeRulesetSetting.RubyFont),
            manager.Get<FontUsage>(KaraokeRulesetSetting.RomanisationFont),
            manager.Get<FontUsage>(KaraokeRulesetSetting.TranslationFont),
            manager.Get<FontUsage>(KaraokeRulesetSetting.NoteFont),
        };

        var fontInfos = targetImportFonts
                        .Distinct()
                        .ToArray();

        if (!fontInfos.Any())
            return;

        // create local font store and import those files
        localFontStore = new KaraokeLocalFontStore(fontManager, renderer);
        fontStore.AddStore(localFontStore);

        foreach (var fontInfo in fontInfos)
        {
            localFontStore.AddFont(fontInfo);
        }
    }

    protected override void Dispose(bool isDisposing)
    {
        base.Dispose(isDisposing);

        fontStore.RemoveStore(localFontStore);
    }
}
