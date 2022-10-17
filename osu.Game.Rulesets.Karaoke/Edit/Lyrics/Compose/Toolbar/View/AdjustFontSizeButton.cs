// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Utils;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Compose.Toolbar.View
{
    public class AdjustFontSizeButton : CompositeDrawable
    {
        private readonly Bindable<float> bindableFontSize = new();

        public AdjustFontSizeButton()
        {
            IconButton previousSizeButton;
            OsuSpriteText fontSizeSpriteText;
            IconButton nextSizeButton;
            float[] sizes = FontUtils.ComposerFontSize();

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
                    previousSizeButton = new IconButton
                    {
                        Size = new Vector2(SpecialActionToolbar.ICON_SIZE),
                        Icon = FontAwesome.Solid.Minus,
                        Action = () =>
                        {
                            float previousSize = sizes.GetPrevious(bindableFontSize.Value);
                            if (previousSize == default)
                                return;

                            bindableFontSize.Value = previousSize;
                        }
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
                    nextSizeButton = new IconButton
                    {
                        Size = new Vector2(SpecialActionToolbar.ICON_SIZE),
                        Icon = FontAwesome.Solid.Plus,
                        Action = () =>
                        {
                            float nextSize = sizes.GetNext(bindableFontSize.Value);
                            if (nextSize == default)
                                return;

                            bindableFontSize.Value = nextSize;
                        }
                    }
                }
            };

            bindableFontSize.BindValueChanged(e =>
            {
                fontSizeSpriteText.Text = FontUtils.GetText(e.NewValue);
                previousSizeButton.Enabled.Value = sizes.GetPrevious(e.NewValue) != default;
                nextSizeButton.Enabled.Value = sizes.GetNext(e.NewValue) != default;
            });
        }

        [BackgroundDependencyLoader]
        private void load(KaraokeRulesetLyricEditorConfigManager lyricEditorConfigManager)
        {
            lyricEditorConfigManager.BindWith(KaraokeRulesetLyricEditorSetting.FontSizeInComposer, bindableFontSize);
        }
    }
}
