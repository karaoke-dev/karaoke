// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Utils;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Compose.Toolbar.View;

public partial class AdjustFontSizeButton : CompositeDrawable
{
    private readonly Bindable<float> bindableFontSize = new();

    public AdjustFontSizeButton()
    {
        OsuSpriteText fontSizeSpriteText;

        Height = SpecialActionToolbar.HEIGHT;
        AutoSizeAxes = Axes.X;
        InternalChild = new FillFlowContainer
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            AutoSizeAxes = Axes.Both,
            Direction = FillDirection.Horizontal,
            Children = new Drawable[]
            {
                new DecreasePreviewFontSizeActionButton
                {
                    Size = new Vector2(SpecialActionToolbar.ICON_SIZE),
                },
                new Container
                {
                    Width = 48,
                    Height = SpecialActionToolbar.ICON_SIZE,
                    Child = fontSizeSpriteText = new OsuSpriteText
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                    },
                },
                new IncreasePreviewFontSizeActionButton
                {
                    Size = new Vector2(SpecialActionToolbar.ICON_SIZE),
                },
            },
        };

        bindableFontSize.BindValueChanged(e =>
        {
            fontSizeSpriteText.Text = FontUtils.GetText(e.NewValue);
        });
    }

    [BackgroundDependencyLoader]
    private void load(KaraokeRulesetLyricEditorConfigManager lyricEditorConfigManager)
    {
        lyricEditorConfigManager.BindWith(KaraokeRulesetLyricEditorSetting.FontSizeInComposer, bindableFontSize);
    }

    private partial class DecreasePreviewFontSizeActionButton : PreviewFontSizeActionButton
    {
        protected override KaraokeEditAction EditAction => KaraokeEditAction.DecreasePreviewFontSize;

        protected override float GetTriggeredFontSize(float[] sizes, float currentFontSize) => sizes.GetPrevious(currentFontSize);

        public DecreasePreviewFontSizeActionButton()
        {
            SetIcon(FontAwesome.Solid.Minus);
        }
    }

    private partial class IncreasePreviewFontSizeActionButton : PreviewFontSizeActionButton
    {
        protected override KaraokeEditAction EditAction => KaraokeEditAction.IncreasePreviewFontSize;

        protected override float GetTriggeredFontSize(float[] sizes, float currentFontSize) => sizes.GetNext(currentFontSize);

        public IncreasePreviewFontSizeActionButton()
        {
            SetIcon(FontAwesome.Solid.Plus);
        }
    }

    private abstract partial class PreviewFontSizeActionButton : ToolbarEditActionButton
    {
        private static readonly float[] sizes = FontUtils.ComposerFontSize();

        private readonly Bindable<float> bindableFontSize = new();

        protected PreviewFontSizeActionButton()
        {
            Action = () =>
            {
                float triggeredFontSize = GetTriggeredFontSize(sizes, bindableFontSize.Value);
                bindableFontSize.Value = triggeredFontSize != default ? triggeredFontSize : bindableFontSize.Value;
            };

            bindableFontSize.BindValueChanged(e =>
            {
                float triggeredFontSize = GetTriggeredFontSize(sizes, e.NewValue);
                SetState(triggeredFontSize != default);
            });
        }

        protected abstract float GetTriggeredFontSize(float[] sizes, float currentFontSize);

        [BackgroundDependencyLoader]
        private void load(KaraokeRulesetLyricEditorConfigManager lyricEditorConfigManager)
        {
            lyricEditorConfigManager.BindWith(KaraokeRulesetLyricEditorSetting.FontSizeInComposer, bindableFontSize);
        }
    }
}
