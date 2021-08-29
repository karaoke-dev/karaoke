// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics;

namespace osu.Game.Rulesets.Karaoke.Edit
{
    public class KaraokeLyricEditor : CompositeDrawable
    {
        private readonly Bindable<LyricEditorMode> bindableLyricEditorMode = new();

        private readonly LyricEditor lyricEditor;

        public KaraokeLyricEditor(Ruleset ruleset)
        {
            AddInternal(new KaraokeEditInputManager(ruleset.RulesetInfo)
            {
                RelativeSizeAxes = Axes.Both,
                Child = lyricEditor = new LyricEditor
                {
                    RelativeSizeAxes = Axes.Both,
                }
            });
            bindableLyricEditorMode.BindValueChanged(e =>
            {
                lyricEditor.Mode = e.NewValue;
            });
        }

        [BackgroundDependencyLoader]
        private void load(KaraokeRulesetLyricEditorConfigManager lyricEditorConfigManager)
        {
            lyricEditorConfigManager.BindWith(KaraokeRulesetLyricEditorSetting.LyricEditorMode, bindableLyricEditorMode);
        }
    }
}
