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

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.Romanisation.Ja;

public class JaRomanisationGenerator : RomanisationGenerator<JaRomanisationGeneratorConfig>
{
    private readonly Analyzer analyzer;

    public JaRomanisationGenerator(JaRomanisationGeneratorConfig config)
        : base(config)
    {
        analyzer = Analyzer.NewAnonymous((fieldName, reader) =>
        {
            Tokenizer tokenizer = new JapaneseTokenizer(reader, null, true, JapaneseTokenizerMode.SEARCH);
            return new TokenStreamComponents(tokenizer, new JapaneseReadingFormFilter(tokenizer, false));
        });
    }

    protected override IReadOnlyDictionary<TimeTag, RomanisationGenerateResult> GenerateFromItem(Lyric item)
    {
        // Tokenize the text
        string text = item.Text;
        var tokenStream = analyzer.GetTokenStream("dummy", new StringReader(text));

        // get the processing tags.
        var processingRomanisations = getProcessingRomanisations(text, tokenStream, Config).ToArray();

        // then, trying to mapping them with the time-tags.
        return Convert(item.TimeTags, processingRomanisations);
    }

    private static IEnumerable<RomanisationGeneratorParameter> getProcessingRomanisations(string text, TokenStream tokenStream, JaRomanisationGeneratorConfig config)
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

            // Convert to romanised syllable.
            string romanisedSyllable = JpStringUtils.ToRomaji(katakana);
            if (config.Uppercase.Value)
                romanisedSyllable = romanisedSyllable.ToUpper();

            // Make tag
            yield return new RomanisationGeneratorParameter
            {
                FromKanji = fromKanji,
                StartIndex = offsetAttribute.StartOffset,
                EndIndex = offsetAttribute.EndOffset - 1,
                RomanisedSyllable = romanisedSyllable,
            };
        }

        // Dispose
        tokenStream.End();
        tokenStream.Dispose();
    }

    internal static IReadOnlyDictionary<TimeTag, RomanisationGenerateResult> Convert(IList<TimeTag> timeTags, IList<RomanisationGeneratorParameter> romanisations)
    {
        var group = createGroup(timeTags, romanisations);
        return group.ToDictionary(k => k.Key, x =>
        {
            bool isFirst = timeTags.IndexOf(x.Key) == 0; // todo: use better to mark the first syllable.
            string romanisedSyllable = string.Join(" ", x.Value.Select(r => r.RomanisedSyllable));

            return new RomanisationGenerateResult
            {
                FirstSyllable = isFirst,
                RomanisedSyllable = romanisedSyllable,
            };
        });

        static IReadOnlyDictionary<TimeTag, List<RomanisationGeneratorParameter>> createGroup(IList<TimeTag> timeTags, IList<RomanisationGeneratorParameter> romanisations)
        {
            var dictionary = timeTags.ToDictionary(x => x, v => new List<RomanisationGeneratorParameter>());

            int processedIndex = 0;

            foreach (var (timeTag, list) in dictionary)
            {
                while (processedIndex < romanisations.Count && isTimeTagInRange(timeTags, timeTag, romanisations[processedIndex]))
                {
                    list.Add(romanisations[processedIndex]);
                    processedIndex++;
                }
            }

            if (processedIndex < romanisations.Count - 1)
                throw new InvalidOperationException("Still have romanisations that haven't process");

            return dictionary;
        }

        static bool isTimeTagInRange(IEnumerable<TimeTag> timeTags, TimeTag currentTimeTag, RomanisationGeneratorParameter parameter)
        {
            if (currentTimeTag.Index.State == TextIndex.IndexState.End)
                return false;

            int romanisationIndex = parameter.StartIndex;

            var nextTimeTag = timeTags.GetNextMatch(currentTimeTag, x => x.Index > currentTimeTag.Index && x.Index.State == TextIndex.IndexState.Start);
            if (nextTimeTag == null)
                return romanisationIndex >= currentTimeTag.Index.Index;

            return romanisationIndex >= currentTimeTag.Index.Index && romanisationIndex < nextTimeTag.Index.Index;
        }
    }

    internal class RomanisationGeneratorParameter
    {
        public bool FromKanji { get; set; }

        public int StartIndex { get; set; }

        public int EndIndex { get; set; }

        public string RomanisedSyllable { get; set; } = string.Empty;
    }
}
