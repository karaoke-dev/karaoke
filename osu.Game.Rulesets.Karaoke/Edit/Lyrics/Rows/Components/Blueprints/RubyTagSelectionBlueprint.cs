// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Types;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components.Blueprints
{
    public class RubyTagSelectionBlueprint : TextTagSelectionBlueprint<ITextTag>
    {
        [UsedImplicitly]
        private readonly Bindable<string> text;

        [UsedImplicitly]
        private readonly BindableNumber<int> startIndex;

        [UsedImplicitly]
        private readonly BindableNumber<int> endIndex;

        public RubyTagSelectionBlueprint(ITextTag item)
            : base(item)
        {
            if (!(item is RubyTag romajiTag))
                throw new InvalidCastException(nameof(item));

            text = romajiTag.TextBindable.GetBoundCopy();
            startIndex = romajiTag.StartIndexBindable.GetBoundCopy();
            endIndex = romajiTag.EndIndexBindable.GetBoundCopy();
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            UpdatePositionAndSize();
            text.BindValueChanged(e => UpdatePositionAndSize());
            startIndex.BindValueChanged(e => UpdatePositionAndSize());
            endIndex.BindValueChanged(e => UpdatePositionAndSize());
        }
    }
}
