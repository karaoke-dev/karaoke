// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Edit.Blueprints.Notes.Components;
using osu.Game.Rulesets.Karaoke.Edit.Notes;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Extends.Notes
{
    public class NoteEditorHitObjectBlueprint : SelectionBlueprint<Note>
    {
        [Resolved]
        private NoteManager noteManager { get; set; }

        public NoteEditorHitObjectBlueprint(Note item)
            : base(item)
        {
            AddInternal(new EditBodyPiece
            {
                RelativeSizeAxes = Axes.Both
            });
        }

        public override MenuItem[] ContextMenuItems => new MenuItem[]
        {
            new OsuMenuItem(Item.Display ? "Hide" : "Show", Item.Display ? MenuItemType.Destructive : MenuItemType.Standard, () => noteManager.ChangeDisplay(Item, !Item.Display)),
            new OsuMenuItem("Split", MenuItemType.Destructive, () => noteManager.SplitNote(Item)),
        };
    }
}
