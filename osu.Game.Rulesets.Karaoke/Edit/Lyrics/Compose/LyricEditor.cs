// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Components.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Compose
{
    public class LyricEditor : CompositeDrawable
    {
        private readonly IBindable<Lyric?> bindableFocusedLyric = new Bindable<Lyric?>();
        private readonly IBindable<float> bindableFontSize = new Bindable<float>();

        private readonly LyricEditorSkin skin;
        private readonly SkinProvidingContainer skinProvidingContainer;

        public LyricEditor()
        {
            RelativeSizeAxes = Axes.Both;

            InternalChild = skinProvidingContainer = new SkinProvidingContainer(skin = new LyricEditorSkin(null))
            {
                Margin = new MarginPadding { Left = 30 },
                RelativeSizeAxes = Axes.Both,
            };

            bindableFocusedLyric.BindValueChanged(e =>
            {
                skinProvidingContainer.Clear();

                var lyric = e.NewValue;
                if (lyric == null)
                    return;

                skinProvidingContainer.Add(new EditableLyric(lyric)
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                });
            });

            bindableFontSize.BindValueChanged(e =>
            {
                skin.FontSize = e.NewValue;
            });
        }

        [BackgroundDependencyLoader]
        private void load(ILyricCaretState lyricCaretState, KaraokeRulesetLyricEditorConfigManager lyricEditorConfigManager)
        {
            bindableFocusedLyric.BindTo(lyricCaretState.BindableFocusedLyric);

            lyricEditorConfigManager.BindWith(KaraokeRulesetLyricEditorSetting.FontSizeInComposer, bindableFontSize);
        }
    }
}
