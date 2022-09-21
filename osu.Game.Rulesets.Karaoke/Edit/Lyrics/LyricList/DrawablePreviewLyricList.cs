// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows;
using osu.Game.Rulesets.Karaoke.Objects;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricList
{
    public class DrawablePreviewLyricList : DrawableLyricList
    {
        private readonly IBindable<LyricEditorMode> bindableMode = new Bindable<LyricEditorMode>();
        private readonly IBindable<bool> bindableAutoFocusToEditLyric = new Bindable<bool>();
        private readonly IBindable<int> bindableAutoFocusToEditLyricSkipRows = new Bindable<int>();

        protected override bool ScrollToPosition(ICaretPosition caret)
        {
            // should not move the position if caret is only support clicking.
            if (caret is ClickingCaretPosition)
                return false;

            // should not move the position in manage lyric mode.
            if (bindableMode.Value == LyricEditorMode.Texting)
                return false;

            // move to target position if auto focus.
            bool autoFocus = bindableAutoFocusToEditLyric.Value;
            if (!autoFocus)
                return false;

            return true;
        }

        protected override int SkipRows()
        {
            return bindableAutoFocusToEditLyricSkipRows.Value;
        }

        protected override DrawableLyricListItem CreateLyricListItem(Lyric item)
            => new DrawablePreviewLyricListItem(item);

        protected override Vector2 Spacing => new(0, 2);

        protected override Drawable CreateBottomDrawable()
        {
            return new Container
            {
                RelativeSizeAxes = Axes.X,
                Height = 75,
                Padding = new MarginPadding { Left = DrawableLyricListItem.HANDLER_WIDTH },
                Child = new Container
                {
                    Masking = true,
                    CornerRadius = 5,
                    RelativeSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Alpha = 0.5f,
                            Colour = Color4.Black
                        },
                        new CreateNewLyricRow
                        {
                            RelativeSizeAxes = Axes.Both,
                        }
                    }
                }
            };
        }

        [BackgroundDependencyLoader]
        private void load(ILyricEditorState state, KaraokeRulesetLyricEditorConfigManager lyricEditorConfigManager)
        {
            bindableMode.BindTo(state.BindableMode);

            lyricEditorConfigManager.BindWith(KaraokeRulesetLyricEditorSetting.AutoFocusToEditLyric, bindableAutoFocusToEditLyric);
            lyricEditorConfigManager.BindWith(KaraokeRulesetLyricEditorSetting.AutoFocusToEditLyricSkipRows, bindableAutoFocusToEditLyricSkipRows);
        }
    }
}
