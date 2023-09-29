// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Game.Graphics;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Components.Lyrics.Blueprints;

public partial class RubyTagSelectionBlueprint : SelectionBlueprint<RubyTag>, IHasPopover
{
    [Resolved]
    private IEditRubyModeState editRubyModeState { get; set; } = null!;

    [Resolved]
    private IPreviewLyricPositionProvider previewLyricPositionProvider { get; set; } = null!;

    [UsedImplicitly]
    private readonly Bindable<string> text;

    [UsedImplicitly]
    private readonly BindableNumber<int> startIndex;

    [UsedImplicitly]
    private readonly BindableNumber<int> endIndex;

    private readonly Container previewTextArea;
    private readonly Container indexRangeBackground;

    public RubyTagSelectionBlueprint(RubyTag item)
        : base(item)
    {
        // Instead of adding the margin to the popover, use this way to make the popover not block the lyric text.
        RelativeSizeAxes = Axes.Y;
        AutoSizeAxes = Axes.X;

        text = item.TextBindable.GetBoundCopy();
        startIndex = item.StartIndexBindable.GetBoundCopy();
        endIndex = item.EndIndexBindable.GetBoundCopy();

        InternalChildren = new[]
        {
            previewTextArea = new Container
            {
                Alpha = 0,
            },
            indexRangeBackground = new Container
            {
                Masking = true,
                BorderThickness = 3,
                Alpha = 0,
                BorderColour = Color4.White,
                Children = new Drawable[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Alpha = 0f,
                        AlwaysPresent = true,
                    },
                },
            },
        };
    }

    [BackgroundDependencyLoader]
    private void load(OsuColour colours)
    {
        indexRangeBackground.Colour = colours.Pink;

        UpdatePositionAndSize();
        text.BindValueChanged(_ => UpdatePositionAndSize());
        startIndex.BindValueChanged(_ => UpdatePositionAndSize());
        endIndex.BindValueChanged(_ => UpdatePositionAndSize());
    }

    protected void UpdatePositionAndSize()
    {
        // wait until lyric update ruby position.
        ScheduleAfterChildren(() =>
        {
            var rubyTagRect = previewLyricPositionProvider.GetRubyTagByPosition(Item);

            if (rubyTagRect == null)
            {
                return;
            }

            var startRect = previewLyricPositionProvider.GetRectByCharIndex(Item.StartIndex);
            var endRect = previewLyricPositionProvider.GetRectByCharIndex(Item.EndIndex);

            // update select position
            updateDrawableRect(previewTextArea, rubyTagRect.Value);

            // update index range position.
            var indexRangePosition = new Vector2(startRect.Left, rubyTagRect.Value.Y);
            var indexRangeSize = new Vector2(endRect.Right - startRect.Left, rubyTagRect.Value.Height);
            updateDrawableRect(indexRangeBackground, new RectangleF(indexRangePosition, indexRangeSize));
        });

        static void updateDrawableRect(Drawable target, RectangleF rect)
        {
            target.X = rect.X;
            target.Y = rect.Y;
            target.Width = rect.Width;
            target.Height = rect.Height;
        }
    }

    public override bool ReceivePositionalInputAt(Vector2 screenSpacePos)
        => previewTextArea.ReceivePositionalInputAt(screenSpacePos);

    public override Vector2 ScreenSpaceSelectionPoint => ScreenSpaceDrawQuad.TopLeft;

    public override Quad SelectionQuad => previewTextArea.ScreenSpaceDrawQuad;

    protected override void OnSelected()
    {
        indexRangeBackground.FadeIn(500);
    }

    protected override void OnDeselected()
    {
        indexRangeBackground.FadeOut(500);
    }

    public Popover GetPopover() => new RubyEditPopover(Item);

    protected override bool OnClick(ClickEvent e)
    {
        Schedule(() =>
        {
            // should select the current item after popover opened, or other popover closed.
            editRubyModeState.Select(Item);
        });

        this.ShowPopover();
        return base.OnClick(e);
    }

    private partial class RubyEditPopover : OsuPopover
    {
        [Resolved]
        private ILyricRubyTagsChangeHandler lyricRubyTagsChangeHandler { get; set; } = null!;

        private readonly LabelledTextBox labelledRubyTextBox;

        public RubyEditPopover(RubyTag rubyTag)
        {
            AllowableAnchors = new[] { Anchor.TopCentre, Anchor.BottomCentre };
            Child = new FillFlowContainer
            {
                Width = 200,
                Direction = FillDirection.Vertical,
                AutoSizeAxes = Axes.Y,
                Spacing = new Vector2(0, 10),
                Children = new Drawable[]
                {
                    labelledRubyTextBox = new LabelledTextBox
                    {
                        Label = "Ruby",
                        Current =
                        {
                            Value = rubyTag.Text,
                        },
                    },
                    new DeleteRubyButton
                    {
                        Text = "Delete",
                        Action = deleteRubyText,
                    },
                },
            };

            labelledRubyTextBox.OnCommit += (_, newText) =>
            {
                if (newText)
                {
                    editRubyText();
                }
            };

            return;

            void editRubyText()
            {
                lyricRubyTagsChangeHandler.SetText(rubyTag, labelledRubyTextBox.Text);
                this.HidePopover();
            }

            void deleteRubyText()
            {
                lyricRubyTagsChangeHandler.Remove(rubyTag);
                this.HidePopover();
            }
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            ScheduleAfterChildren(() => GetContainingInputManager().ChangeFocus(labelledRubyTextBox));
        }

        private partial class DeleteRubyButton : EditorSectionButton
        {
            [BackgroundDependencyLoader]
            private void load(OsuColour colours)
            {
                BackgroundColour = colours.Pink3;
            }
        }
    }
}
