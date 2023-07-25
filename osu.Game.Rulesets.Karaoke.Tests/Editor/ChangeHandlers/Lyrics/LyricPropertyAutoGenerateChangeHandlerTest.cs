// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using NUnit.Framework;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.ChangeHandlers.Lyrics;

/// <summary>
/// This test is focus on make sure that:
/// If the <see cref="Lyric.ReferenceLyric"/> in the <see cref="Lyric"/> is not empty.
/// <see cref="ILyricPropertyAutoGenerateChangeHandler"/> should be able to change the property.
/// </summary>
/// <typeparam name="TChangeHandler"></typeparam>
[TestFixture(typeof(LyricReferenceChangeHandler))]
[TestFixture(typeof(LyricLanguageChangeHandler))]
[TestFixture(typeof(LyricRubyTagsChangeHandler))]
[TestFixture(typeof(LyricRomajiTagsChangeHandler))]
[TestFixture(typeof(LyricTimeTagsChangeHandler))]
[TestFixture(typeof(LyricNotesChangeHandler))]
public partial class LyricPropertyAutoGenerateChangeHandlerTest<TChangeHandler> : LyricPropertyChangeHandlerTest<TChangeHandler>
    where TChangeHandler : LyricPropertyChangeHandler, ILyricPropertyAutoGenerateChangeHandler, new()
{
    protected override bool IncludeAutoGenerator => true;

    [Test]
    [Description("Should not be able to generate the property if the lyric is reference to other lyric.")]
    public void CheckWithReferencedLyric()
    {
        if (isLyricReferenceChangeHandler())
            return;

        PrepareLyricWithSyncConfig(new Lyric
        {
            Text = "karaoke",
            Language = new CultureInfo(17), // for auto-generate ruby and romaji.
            TimeTags = new[] // for auto-generate notes.
            {
                new TimeTag(new TextIndex(0), 0),
                new TimeTag(new TextIndex(1), 1000),
                new TimeTag(new TextIndex(2), 2000),
                new TimeTag(new TextIndex(3), 3000),
                new TimeTag(new TextIndex(3, TextIndex.IndexState.End), 4000),
            },
        });

        TriggerHandlerChanged(c =>
        {
            Assert.IsFalse(c.CanGenerate());
        });

        TriggerHandlerChanged(c =>
        {
            Assert.IsNotEmpty(c.GetGeneratorNotSupportedLyrics());
        });

        TriggerHandlerChangedWithChangeForbiddenException(c => c.AutoGenerate());
    }

    private bool isLyricReferenceChangeHandler()
        => typeof(TChangeHandler) == typeof(LyricReferenceChangeHandler);
}
