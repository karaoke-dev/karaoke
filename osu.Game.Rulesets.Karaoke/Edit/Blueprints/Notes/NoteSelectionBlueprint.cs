// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Edit.Blueprints.Notes.Components;
using osu.Game.Rulesets.Karaoke.Edit.Components.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Edit.Notes;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.UI;
using osu.Game.Rulesets.Karaoke.UI.Position;
using osu.Game.Rulesets.UI;
using osu.Game.Rulesets.UI.Scrolling;

namespace osu.Game.Rulesets.Karaoke.Edit.Blueprints.Notes
{
    public class NoteSelectionBlueprint : KaraokeSelectionBlueprint<Note>, IHasPopover
    {
        [Resolved]
        private NoteManager noteManager { get; set; }

        [Resolved]
        private Playfield playfield { get; set; }

        [Resolved]
        private IScrollingInfo scrollingInfo { get; set; }

        [Resolved]
        private INotePositionInfo notePositionInfo { get; set; }

        protected ScrollingHitObjectContainer HitObjectContainer => ((KaraokePlayfield)playfield).NotePlayfield.HitObjectContainer;

        public NoteSelectionBlueprint(Note note)
            : base(note)
        {
            AddInternal(new EditBodyPiece
            {
                RelativeSizeAxes = Axes.Both
            });
        }

        protected override void Update()
        {
            base.Update();

            var anchor = scrollingInfo.Direction.Value == ScrollingDirection.Left ? Anchor.CentreLeft : Anchor.CentreRight;
            Anchor = Origin = anchor;

            Position = Parent.ToLocalSpace(HitObjectContainer.ScreenSpacePositionAtTime(HitObject.StartTime)) - AnchorPosition;
            Y += notePositionInfo.Calculator.YPositionAt(HitObject.Tone);

            Width = HitObjectContainer.LengthAtTime(HitObject.StartTime, HitObject.EndTime);
            Height = notePositionInfo.Calculator.ColumnHeight;
        }

        public override MenuItem[] ContextMenuItems => new MenuItem[]
        {
            new OsuMenuItem(HitObject.Display ? "Hide" : "Show", HitObject.Display ? MenuItemType.Destructive : MenuItemType.Standard, () => noteManager.ChangeDisplay(HitObject, !HitObject.Display)),
            new OsuMenuItem("Split", MenuItemType.Destructive, () => noteManager.SplitNote(HitObject)),
        };

        public Popover GetPopover() => new NoteEditPopover(HitObject);

        protected override bool OnClick(ClickEvent e)
        {
            this.ShowPopover();
            return base.OnClick(e);
        }
    }
}
