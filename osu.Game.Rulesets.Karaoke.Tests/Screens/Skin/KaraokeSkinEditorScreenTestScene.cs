// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Overlays;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Screens.Skin;
using osu.Game.Rulesets.Karaoke.Skinning;
using osu.Game.Rulesets.Karaoke.Tests.Beatmaps;
using osu.Game.Rulesets.Karaoke.Tests.Resources;
using osu.Game.Screens.Edit;
using osu.Game.Skinning;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Screens.Skin;

public abstract partial class KaraokeSkinEditorScreenTestScene<T> : EditorClockTestScene where T : KaraokeSkinEditorScreen
{
    [Cached(typeof(EditorBeatmap))]
    [Cached(typeof(IBeatSnapProvider))]
    private readonly EditorBeatmap editorBeatmap;

    [Cached]
    private readonly OverlayColourProvider colourProvider = new(OverlayColourScheme.Pink);

    private readonly KaraokeBeatmapSkin karaokeSkin = new TestKaraokeBeatmapSkin();

    protected KaraokeSkinEditorScreenTestScene()
    {
        // todo: skin editor might not need the editor beatmap.
        editorBeatmap = new EditorBeatmap(createBeatmap());
    }

    protected override void LoadComplete()
    {
        Child = new SkinProvidingContainer(karaokeSkin)
        {
            RelativeSizeAxes = Axes.Both,
            Children = new Drawable[]
            {
                editorBeatmap,
                CreateEditorScreen(karaokeSkin).With(x =>
                {
                    x.State.Value = Visibility.Visible;
                }),
            },
        };
    }

    protected abstract T CreateEditorScreen(KaraokeSkin karaokeSkin);

    protected class TestKaraokeBeatmapSkin : KaraokeBeatmapSkin
    {
        public TestKaraokeBeatmapSkin()
            : base(new SkinInfo(), TestResources.CreateSkinStorageResourceProvider())
        {
        }
    }

    private KaraokeBeatmap createBeatmap()
    {
        var beatmap = new TestKaraokeBeatmap(new KaraokeRuleset().RulesetInfo);
        if (new KaraokeBeatmapConverter(beatmap, new KaraokeRuleset()).Convert() is not KaraokeBeatmap karaokeBeatmap)
            throw new ArgumentNullException(nameof(karaokeBeatmap));

        return karaokeBeatmap;
    }
}
