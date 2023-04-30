// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.IO;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas.Types;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Beatmaps;

public interface IBeatmapSingersChangeHandler
{
    // todo: should use IBindableList eventually, but cannot do that because it's bind to selection item.
    BindableList<ISinger> Singers { get; }

    void ChangeOrder(ISinger singer, int newIndex);

    bool ChangeSingerAvatar(Singer singer, FileInfo fileInfo);

    Singer Add();

    void Remove(Singer singer);
}
