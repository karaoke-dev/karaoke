// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Ja;
using Lucene.Net.Analysis.TokenAttributes;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.Romanization.Ja;

public class JaRomanizationGenerator : RomanizationGenerator<JaRomanizationGeneratorConfig>
{
    private readonly Analyzer analyzer;

    public JaRomanizationGenerator(JaRomanizationGeneratorConfig config)
        : base(config)
    {
        analyzer = Analyzer.NewAnonymous((fieldName, reader) =>
        {
            Tokenizer tokenizer = new JapaneseTokenizer(reader, null, true, JapaneseTokenizerMode.SEARCH);
            return new TokenStreamComponents(tokenizer, new JapaneseReadingFormFilter(tokenizer, false));
        });
    }

    protected override IReadOnlyDictionary<TimeTag, RomanizationGenerateResult> GenerateFromItem(Lyric item)
    {
        // Tokenize the text
        string text = item.Text;
        var tokenStream = analyzer.GetTokenStream("dummy", new StringReader(text));

        // get the processing tags.
        var processingRomanizations = getProcessingRomanizations(text, tokenStream, Config).ToArray();

        // then, trying to mapping them with the time-tags.
        return Convert(item.TimeTags, processingRomanizations);
    }

    private static IEnumerable<RomanizationGeneratorParameter> getProcessingRomanizations(string text, TokenStream tokenStream, JaRomanizationGeneratorConfig config)
    {
        // Reset the stream and convert all result
        tokenStream.Reset();

        while (true)
        {
            // Read next token
            tokenStream.ClearAttributes();
            tokenStream.IncrementToken();

            // Get result and offset
            var charTermAttribute = tokenStream.GetAttribute<ICharTermAttribute>();
            var offsetAttribute = tokenStream.GetAttribute<IOffsetAttribute>();

            // Get parsed result, result is Katakana.
            string katakana = charTermAttribute.ToString();
            if (string.IsNullOrEmpty(katakana))
                break;

            string parentText = text[offsetAttribute.StartOffset..offsetAttribute.EndOffset];
            bool fromKanji = JpStringUtils.ToKatakana(katakana) != JpStringUtils.ToKatakana(parentText);

            // Convert to romanized syllable.
            string romanizedSyllable = JpStringUtils.ToRomaji(katakana);
            if (config.Uppercase.Value)
                romanizedSyllable = romanizedSyllable.ToUpper();

            // Make tag
            yield return new RomanizationGeneratorParameter
            {
                FromKanji = fromKanji,
                StartIndex = offsetAttribute.StartOffset,
                EndIndex = offsetAttribute.EndOffset - 1,
                RomanizedSyllable = romanizedSyllable,
            };
        }

        // Dispose
        tokenStream.End();
        tokenStream.Dispose();
    }

    internal static IReadOnlyDictionary<TimeTag, RomanizationGenerateResult> Convert(IList<TimeTag> timeTags, IList<RomanizationGeneratorParameter> romanizations)
    {
        var group = createGroup(timeTags, romanizations);
        return group.ToDictionary(k => k.Key, x =>
        {
            bool isFirst = timeTags.IndexOf(x.Key) == 0; // todo: use better to mark the first syllable.
            string romanizedSyllable = string.Join(" ", x.Value.Select(r => r.RomanizedSyllable));

            return new RomanizationGenerateResult
            {
                FirstSyllable = isFirst,
                RomanizedSyllable = romanizedSyllable,
            };
        });

        static IReadOnlyDictionary<TimeTag, List<RomanizationGeneratorParameter>> createGroup(IList<TimeTag> timeTags, IList<RomanizationGeneratorParameter> romanizations)
        {
            var dictionary = timeTags.ToDictionary(x => x, v => new List<RomanizationGeneratorParameter>());

            int processedIndex = 0;

            foreach (var (timeTag, list) in dictionary)
            {
                while (processedIndex < romanizations.Count && isTimeTagInRange(timeTags, timeTag, romanizations[processedIndex]))
                {
                    list.Add(romanizations[processedIndex]);
                    processedIndex++;
                }
            }

            if (processedIndex < romanizations.Count - 1)
                throw new InvalidOperationException("Still have romanizations that haven't process");

            return dictionary;
        }

        static bool isTimeTagInRange(IEnumerable<TimeTag> timeTags, TimeTag currentTimeTag, RomanizationGeneratorParameter parameter)
        {
            if (currentTimeTag.Index.State == TextIndex.IndexState.End)
                return false;

            int romanizationIndex = parameter.StartIndex;

            var nextTimeTag = timeTags.GetNextMatch(currentTimeTag, x => x.Index > currentTimeTag.Index && x.Index.State == TextIndex.IndexState.Start);
            if (nextTimeTag == null)
                return romanizationIndex >= currentTimeTag.Index.Index;

            return romanizationIndex >= currentTimeTag.Index.Index && romanizationIndex < nextTimeTag.Index.Index;
        }
    }

    internal class RomanizationGeneratorParameter
    {
        public bool FromKanji { get; set; }

        public int StartIndex { get; set; }

        public int EndIndex { get; set; }

        public string RomanizedSyllable { get; set; } = string.Empty;
    }
}
