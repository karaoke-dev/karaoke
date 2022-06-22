// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components.Blueprints
{
    public class RubyTagSelectionBlueprint : TextTagSelectionBlueprint<RubyTag>
    {
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
    }
}
