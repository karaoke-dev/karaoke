// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Primitives;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Components.Lyrics;

public partial class LyricLayer : Layer, IPreviewLyricPositionProvider
{
    [Resolved]
    private OsuColour colours { get; set; } = null!;

    private readonly PreviewKaraokeSpriteText previewKaraokeSpriteText;

    public Action<Vector2>? SizeChanged;

    public LyricLayer(Lyric lyric)
        : base(lyric)
    {
        InternalChild = previewKaraokeSpriteText = new PreviewKaraokeSpriteText(lyric);

        previewKaraokeSpriteText.SizeChanged = size =>
        {
            SizeChanged?.Invoke(size);
        };
    }

    [BackgroundDependencyLoader]
    private void load(EditorClock clock)
    {
        previewKaraokeSpriteText.Clock = clock;
    }

    public Vector2 LyricPosition
    {
        get => previewKaraokeSpriteText.Position;
        set => previewKaraokeSpriteText.Position = value;
    }

    public override void UpdateDisableEditState(bool editable)
    {
        this.FadeTo(editable ? 1 : 0.5f, 100);
    }

    public override void TriggerDisallowEditEffect(LyricEditorMode editorMode)
    {
        this.FlashColour(colours.Red, 200);
    }

    #region Text char index

    public int? GetCharIndexByPosition(Vector2 position)
        => previewKaraokeSpriteText.GetCharIndexByPosition(position - LyricPosition);

    public RectangleF GetRectByCharIndex(int charIndex)
        => previewKaraokeSpriteText.GetRectByCharIndex(charIndex).Offset(LyricPosition);

    #endregion

    #region Text indicator

    public int? GetCharIndicatorByPosition(Vector2 position)
        => previewKaraokeSpriteText.GetCharIndicatorByPosition(position - LyricPosition);

    public RectangleF GetRectByCharIndicator(int gapIndex)
        => previewKaraokeSpriteText.GetRectByCharIndicator(gapIndex).Offset(LyricPosition);

    #endregion

    #region Ruby tag

    public RectangleF? GetRubyTagByPosition(RubyTag rubyTag)
        => previewKaraokeSpriteText.GetRubyTagByPosition(rubyTag)?.Offset(LyricPosition);

    #endregion

    #region Time tag

    public TimeTag? GetTimeTagByPosition(Vector2 position)
        => previewKaraokeSpriteText.GetTimeTagByPosition(position - LyricPosition);

    public Vector2 GetPositionByTimeTag(TimeTag timeTag)
        => previewKaraokeSpriteText.GetPositionByTimeTag(timeTag) + LyricPosition;

    #endregion
}
