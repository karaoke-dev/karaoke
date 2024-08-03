// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps;

namespace osu.Game.Rulesets.Karaoke.Tests.Screens.Edit.Beatmap;

public abstract partial class BeatmapEditorScreenTestScene<T> : GenericEditorScreenTestScene<T, KaraokeBeatmapEditorScreenMode>
    where T : BeatmapEditorScreen;
