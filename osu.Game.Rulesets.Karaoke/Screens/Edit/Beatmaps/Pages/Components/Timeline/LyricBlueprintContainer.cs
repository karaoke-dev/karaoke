// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Timeline;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Pages.Components.Timeline;

public partial class LyricBlueprintContainer : EditableTimelineBlueprintContainer<Lyric>
{
    [BackgroundDependencyLoader]
    private void load(ILyricsProvider lyricsProvider)
    {
        Items.BindTo(lyricsProvider.BindableLyrics);
    }

    protected override SelectionBlueprint<Lyric> CreateBlueprintFor(Lyric item)
        => new PreviewLyricSelectionBlueprint(item);
}
