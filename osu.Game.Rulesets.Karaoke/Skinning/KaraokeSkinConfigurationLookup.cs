// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

namespace osu.Game.Rulesets.Karaoke.Skinning
{
    public class KaraokeSkinConfigurationLookup
    {
        public readonly int Columns;
        public readonly LegacyKaraokeSkinConfigurationLookups Lookup;
        public readonly int? TargetColumn;

        public KaraokeSkinConfigurationLookup(int columns, LegacyKaraokeSkinConfigurationLookups lookup, int? targetColumn = null)
        {
            Columns = columns;
            Lookup = lookup;
            TargetColumn = targetColumn;
        }
    }

    public enum LegacyKaraokeSkinConfigurationLookups
    {
        ColumnHeight,
        ColumnSpacing,
        LightImage,
        UpLineWidth,
        DownLineWidth,
        LightPosition,
        HitPosition,
        JudgementLineHeadImage,
        JudgementLineTailImage,
        JudgementLineBodyImage,
        JudgementAresPrecentage,
        ShowJudgementLine,
        NoteHeadImage,
        NoteTailImage,
        NoteBodyImage,
        ExplosionImage,
        ExplosionScale,
    }

    public enum LegacyKaraokeSkinNoteLayer
    {
        Border,
        Foreground,
        Background
    }
}
