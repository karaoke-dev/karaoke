// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Components.Lyrics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States;
using osu.Game.Skinning;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Compose;

public partial class LyricEditor : CompositeDrawable
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
            Padding = new MarginPadding
            {
                Vertical = 64,
                Horizontal = 120,
            },
            RelativeSizeAxes = Axes.Both,
        };

        bindableFocusedLyric.BindValueChanged(e =>
        {
            skinProvidingContainer.Clear();

            var lyric = e.NewValue;
            if (lyric == null)
                return;

            const int border = 36;

            skinProvidingContainer.Add(new InteractableLyric(lyric)
            {
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                LyricPosition = new Vector2(border),
                TextSizeChanged = (self, size) =>
                {
                    self.Width = size.X + border * 2;
                    self.Height = size.Y + border * 2;
                },
                Layers = new Layer[]
                {
                    new GridLayer(lyric)
                    {
                        Spacing = 10,
                    },
                    new LyricLayer(lyric),
                    new EditLyricLayer(lyric),
                    new TimeTagLayer(lyric),
                    new CaretLayer(lyric),
                    new BlueprintLayer(lyric),
                },
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
