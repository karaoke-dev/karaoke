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
            refreshPreviewLyric(e.NewValue);
        });

        bindableFontSize.BindValueChanged(e =>
        {
            skin.FontSize = e.NewValue;
            refreshPreviewLyric(bindableFocusedLyric.Value);
        });
    }

    private void refreshPreviewLyric(Lyric? lyric)
    {
        skinProvidingContainer.Clear();

        if (lyric == null)
            return;

        const int border = 36;

        skinProvidingContainer.Add(new InteractableLyric(lyric)
        {
            Anchor = Anchor.CentreLeft,
            Origin = Anchor.CentreLeft,
            TextSizeChanged = (self, size) =>
            {
                self.Width = size.X + border * 2;
                self.Height = size.Y + border * 2;
            },
            Loaders = new LayerLoader[]
            {
                new LayerLoader<GridLayer>
                {
                    OnLoad = layer =>
                    {
                        layer.Spacing = 10;
                    },
                },
                new LayerLoader<LyricLayer>
                {
                    OnLoad = layer =>
                    {
                        layer.LyricPosition = new Vector2(border);
                    },
                },
                new LayerLoader<EditLyricLayer>(),
                new LayerLoader<TimeTagLayer>(),
                new LayerLoader<CaretLayer>(),
                new LayerLoader<BlueprintLayer>(),
            },
        });
    }

    [BackgroundDependencyLoader]
    private void load(ILyricCaretState lyricCaretState, KaraokeRulesetLyricEditorConfigManager lyricEditorConfigManager)
    {
        bindableFocusedLyric.BindTo(lyricCaretState.BindableFocusedLyric);

        lyricEditorConfigManager.BindWith(KaraokeRulesetLyricEditorSetting.FontSizeInComposer, bindableFontSize);
    }
}
