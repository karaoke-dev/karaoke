// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Localisation;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Overlays.Toolbar;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Components.Markdown;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Markdown;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings
{
    public abstract partial class LyricEditorEditModeSection<TEditModeState, TEditMode> : LyricEditorEditModeSection<TEditMode>
        where TEditModeState : IHasEditModeState<TEditMode> where TEditMode : Enum
    {
        [Resolved]
        private TEditModeState tEditModeState { get; set; }

        protected sealed override TEditMode DefaultMode() => tEditModeState.EditMode;

        internal sealed override void UpdateEditMode(TEditMode mode)
        {
            tEditModeState.ChangeEditMode(mode);

            base.UpdateEditMode(mode);
        }
    }

    public abstract partial class LyricEditorEditModeSection<TEditMode> : EditModeSection<TEditMode>
        where TEditMode : Enum
    {
        protected sealed override LocalisableString Title => "Edit mode";

        [Resolved]
        private ILyricSelectionState lyricSelectionState { get; set; }

        protected override DescriptionTextFlowContainer CreateDescriptionTextFlowContainer()
            => new LyricEditorDescriptionTextFlowContainer();

        internal override void UpdateEditMode(TEditMode mode)
        {
            // should cancel the selection after change to the new edit mode.
            lyricSelectionState?.EndSelecting(LyricEditorSelectingAction.Cancel);

            base.UpdateEditMode(mode);
        }

        protected abstract partial class VerifySelection : Selection
        {
            private readonly IBindableList<Issue> bindableIssues = new BindableList<Issue>();

            protected VerifySelection()
            {
                CountCircle countCircle;

                AddInternal(countCircle = new CountCircle
                {
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.Centre,
                    X = -5,
                });

                bindableIssues.BindCollectionChanged((_, _) =>
                {
                    int count = bindableIssues.Count;
                    countCircle.Alpha = count == 0 ? 0 : 1;
                    countCircle.Count = count;
                });
            }

            [BackgroundDependencyLoader]
            private void load(ILyricEditorVerifier verifier)
            {
                bindableIssues.BindTo(verifier.GetIssueByEditMode(EditMode));
            }

            protected abstract LyricEditorMode EditMode { get; }
        }

        /// <summary>
        /// Copied from <see cref="ToolbarNotificationButton"/>
        /// </summary>
        private partial class CountCircle : CompositeDrawable
        {
            private readonly OsuSpriteText countText;
            private readonly Circle circle;

            private int count;

            public int Count
            {
                get => count;
                set
                {
                    if (count == value)
                        return;

                    if (value != count)
                    {
                        circle.FlashColour(Color4.White, 600, Easing.OutQuint);
                        this.ScaleTo(1.1f).Then().ScaleTo(1, 600, Easing.OutElastic);
                    }

                    count = value;
                    countText.Text = value.ToString("#,0");
                }
            }

            public CountCircle()
            {
                AutoSizeAxes = Axes.X;
                Height = 20;

                InternalChildren = new Drawable[]
                {
                    circle = new Circle
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Color4.Red
                    },
                    countText = new OsuSpriteText
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Y = -1,
                        Font = OsuFont.GetFont(size: 18, weight: FontWeight.Bold),
                        Padding = new MarginPadding(5),
                        Colour = Color4.White,
                        UseFullGlyphHeight = true,
                    }
                };
            }
        }
    }
}
