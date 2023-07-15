// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics.Cursor;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Singers;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Tests.Screens.Edit.Beatmap;

[TestFixture]
public partial class TestSceneSingerScreen : BeatmapEditorScreenTestScene<SingerScreen>
{
    protected override Container<Drawable> Content { get; } = new Container { RelativeSizeAxes = Axes.Both };

    protected override SingerScreen CreateEditorScreen() => new();

    protected override KaraokeBeatmap CreateBeatmap()
    {
        var karaokeBeatmap = base.CreateBeatmap();

        var singerInfo = karaokeBeatmap.SingerInfo;

        singerInfo.AddSinger(s =>
        {
            s.Order = 1;
            s.Name = "初音ミク";
            s.RomajiName = "Hatsune Miku";
            s.EnglishName = "Miku";
            s.Description = "International superstar vocaloid Hatsune Miku.";
            s.Hue = 189 / 360f;
        });

        singerInfo.AddSinger(s =>
        {
            s.Order = 2;
            s.Name = "ハク";
            s.RomajiName = "haku";
            s.EnglishName = "andy840119";
            s.Description = "Creator of this ruleset.";
            s.Hue = 46 / 360f;
        });

        singerInfo.AddSinger(s =>
        {
            s.Order = 3;
            s.Name = "ゴミパソコン";
            s.RomajiName = "gomi-pasokonn";
            s.EnglishName = "Miku";
            s.Description = "My fucking slow desktop.";
            s.Hue = 290 / 360f;
        });

        return karaokeBeatmap;
    }

    private DialogOverlay dialogOverlay = null!;
    private LyricsProvider lyricsProvider = null!;
    private KaraokeBeatmapResourcesProvider karaokeBeatmapResourcesProvider = null!;

    [BackgroundDependencyLoader]
    private void load()
    {
        base.Content.AddRange(new Drawable[]
        {
            new OsuContextMenuContainer
            {
                RelativeSizeAxes = Axes.Both,
                Child = Content,
            },
            dialogOverlay = new DialogOverlay(),
            lyricsProvider = new LyricsProvider(),
            karaokeBeatmapResourcesProvider = new KaraokeBeatmapResourcesProvider(),
        });

        var beatDivisor = new BindableBeatDivisor
        {
            Value = Beatmap.Value.BeatmapInfo.BeatDivisor,
        };
        var editorClock = new EditorClock(Beatmap.Value.Beatmap, beatDivisor);
        Dependencies.CacheAs(editorClock);
        Dependencies.Cache(beatDivisor);
        Dependencies.CacheAs<IDialogOverlay>(dialogOverlay);
        Dependencies.CacheAs<ILyricsProvider>(lyricsProvider);
        Dependencies.CacheAs<IKaraokeBeatmapResourcesProvider>(karaokeBeatmapResourcesProvider);
    }
}
