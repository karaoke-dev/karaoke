// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics;

public interface ILyricEditorVerifier
{
    IBindableList<Issue> GetBindable(KaraokeHitObject hitObject);

    IBindableList<Issue> GetIssueByEditMode(LyricEditorMode editorMode);

    void Refresh();

    void RefreshByHitObject(KaraokeHitObject hitObject);
}
