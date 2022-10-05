// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Edit.Blueprints.Notes;
using osu.Game.Rulesets.Karaoke.Edit.Blueprints.Notes.Components;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Notes;
using osu.Game.Rulesets.Karaoke.Edit.Components.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.UI.Position;
using osu.Game.Rulesets.UI;
using osu.Game.Rulesets.UI.Scrolling;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Compose.BottomEditor.Notes
{
    /// <summary>
    /// Copy from <see cref="NoteSelectionBlueprint"/>
    /// </summary>
    public class NoteEditorHitObjectBlueprint : SelectionBlueprint<Note>, IHasPopover
    {
        private readonly IBindable<double> timeRange = new BindableDouble();
        private readonly IBindable<ScrollingDirection> direction = new Bindable<ScrollingDirection>();

        private float scrollLength => Parent.DrawWidth;

        private bool axisInverted => direction.Value == ScrollingDirection.Right;

        [Resolved]
        private INotesChangeHandler notesChangeHandler { get; set; }

        [Resolved]
        private INotePropertyChangeHandler notePropertyChangeHandler { get; set; }

        [Resolved]
        private IScrollingInfo scrollingInfo { get; set; }

        [Resolved]
        private INotePositionInfo notePositionInfo { get; set; }

        [Resolved]
        private IEditNoteModeState editNoteModeState { get; set; }

        [Resolved]
        private Playfield playfield { get; set; }

        protected ScrollingHitObjectContainer HitObjectContainer => ((EditorNotePlayfield)playfield).HitObjectContainer;

        public NoteEditorHitObjectBlueprint(Note note)
            : base(note)
        {
            RelativeSizeAxes = Axes.None;
            AddInternal(new EditBodyPiece
            {
                RelativeSizeAxes = Axes.Both
            });
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            direction.BindTo(scrollingInfo.Direction);
            timeRange.BindTo(scrollingInfo.TimeRange);
        }

        protected override void Update()
        {
            base.Update();

            var anchor = scrollingInfo.Direction.Value == ScrollingDirection.Left ? Anchor.CentreLeft : Anchor.CentreRight;
            Anchor = Origin = anchor;

            Position = Parent.ToLocalSpace(HitObjectContainer.ScreenSpacePositionAtTime(Item.StartTime)) - AnchorPosition;
            Y += notePositionInfo.Calculator.YPositionAt(Item.Tone);

            Width = HitObjectContainer.LengthAtTime(Item.StartTime, Item.EndTime);
            Height = notePositionInfo.Calculator.ColumnHeight;
        }

        public override MenuItem[] ContextMenuItems => new MenuItem[]
        {
            new OsuMenuItem(Item.Display ? "Hide" : "Show", Item.Display ? MenuItemType.Destructive : MenuItemType.Standard, () => notePropertyChangeHandler.ChangeDisplayState(!Item.Display)),
            new OsuMenuItem("Split", MenuItemType.Destructive, () => notesChangeHandler.Split()),
        };

        public Popover GetPopover() => new NoteEditPopover(Item);

        protected override bool OnClick(ClickEvent e)
        {
            // should only select current note before open the popover because note change handler will change property in all selected notes.
            editNoteModeState.SelectedItems.Clear();
            editNoteModeState.SelectedItems.Add(Item);

            this.ShowPopover();
            return base.OnClick(e);
        }
    }
}
