// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Game.Graphics;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Components.Lyrics.Blueprints;

public partial class RubyTagSelectionBlueprint : TextTagSelectionBlueprint<RubyTag>, IHasPopover
{
    [Resolved]
    private IEditRubyModeState editRubyModeState { get; set; } = null!;

    [UsedImplicitly]
    private readonly Bindable<string> text;

    [UsedImplicitly]
    private readonly BindableNumber<int> startIndex;

    [UsedImplicitly]
    private readonly BindableNumber<int> endIndex;

    public RubyTagSelectionBlueprint(RubyTag item)
        : base(item)
    {
        text = item.TextBindable.GetBoundCopy();
        startIndex = item.StartIndexBindable.GetBoundCopy();
        endIndex = item.EndIndexBindable.GetBoundCopy();
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        UpdatePositionAndSize();
        text.BindValueChanged(_ => UpdatePositionAndSize());
        startIndex.BindValueChanged(_ => UpdatePositionAndSize());
        endIndex.BindValueChanged(_ => UpdatePositionAndSize());
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
