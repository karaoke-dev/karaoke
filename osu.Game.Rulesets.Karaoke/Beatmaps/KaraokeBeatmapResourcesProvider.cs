// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Textures;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas.Types;

namespace osu.Game.Rulesets.Karaoke.Beatmaps;

public partial class KaraokeBeatmapResourcesProvider : Component, IKaraokeBeatmapResourcesProvider
{
    [Resolved]
    private BeatmapManager beatmapManager { get; set; } = null!;

    [Resolved]
    private IBindable<WorkingBeatmap> working { get; set; } = null!;

    public Texture? GetSingerAvatar(ISinger singer)
    {
        return null;
    }

    /*
    public Texture? GetSingerAvatar(ISinger singer)
    {
        var provider = getBeatmapResourceProvider();
        if (provider == null)
            return null;

        if (singer is not Singer s)
            return null;

        var beatmapSetInfo = working.Value.BeatmapSetInfo;
        if (beatmapSetInfo == null)
            return null;

        string? path = beatmapSetInfo.GetPathForFile($"assets/singers/{s.AvatarFile}");
        return provider.LargeTextureStore.Get(path);
    }

    private IBeatmapResourceProvider? getBeatmapResourceProvider()
    {
        // todo : use better way to get the resource provider.
        var prop = typeof(BeatmapManager).GetField("workingBeatmapCache", BindingFlags.Instance | BindingFlags.NonPublic);
        if (prop == null)
            throw new ArgumentNullException();

        return prop.GetValue(beatmapManager) as WorkingBeatmapCache;
    }
    */
}
