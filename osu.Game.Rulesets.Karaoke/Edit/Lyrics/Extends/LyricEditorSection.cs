// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Components.Containers;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends
{
    /// <summary>
    /// Base section class for lyric editor.
    /// todo: should inherit the EditorRoundedScreenSettingsSection eventually, but seems that class haven't ready.
    /// </summary>
    public abstract class LyricEditorSection : Section
    {
        protected LyricEditorSection()
        {
            Padding = new MarginPadding(0);
        }
    }
}
