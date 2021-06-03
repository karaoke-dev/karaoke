// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Edit.Blueprints.Notes.Components;
using osu.Game.Rulesets.Karaoke.Edit.Notes;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.UI.Components;
using osu.Game.Rulesets.Karaoke.UI.Position;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Extends.Notes
{
    public class NoteEditorHitObjectBlueprint : SelectionBlueprint<Note>
    {
        [UsedImplicitly]
        private readonly Bindable<double> startTime;

        [UsedImplicitly]
        private readonly Bindable<Tone> tone;

        private readonly EditBodyPiece editBodyPiece;

        [Resolved]
        private NoteManager noteManager { get; set; }

        public NoteEditorHitObjectBlueprint(Note item)
            : base(item)
        {
            startTime = item.StartTimeBindable.GetBoundCopy();
            tone = item.ToneBindable.GetBoundCopy();

            AddInternal(editBodyPiece = new EditBodyPiece
            {
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                Width = 100
            });
        }

        [BackgroundDependencyLoader]
        private void load(IPositionCalculator positionCalculator)
        {
            editBodyPiece.Height = DefaultColumnBackground.COLUMN_HEIGHT;

            startTime.BindValueChanged(e =>
            {
                // todo : adjust x position
            });

            tone.BindValueChanged(e =>
            {
                // todo : adjust y position
                editBodyPiece.Y = positionCalculator.YPositionAt(e.NewValue);
            }, true);
        }

        public override bool ReceivePositionalInputAt(Vector2 screenSpacePos)
            => editBodyPiece.ReceivePositionalInputAt(screenSpacePos);

        public override Vector2 ScreenSpaceSelectionPoint => editBodyPiece.ScreenSpaceDrawQuad.Centre;

        public override Quad SelectionQuad => editBodyPiece.ScreenSpaceDrawQuad;

        public override MenuItem[] ContextMenuItems => new MenuItem[]
        {
            new OsuMenuItem(Item.Display ? "Hide" : "Show", Item.Display ? MenuItemType.Destructive : MenuItemType.Standard, () => noteManager.ChangeDisplay(Item, !Item.Display)),
            new OsuMenuItem("Split", MenuItemType.Destructive, () => noteManager.SplitNote(Item)),
        };
    }
}
