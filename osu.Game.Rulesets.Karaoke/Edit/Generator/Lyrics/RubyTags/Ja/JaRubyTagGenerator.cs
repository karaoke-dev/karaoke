// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Ja;
using Lucene.Net.Analysis.TokenAttributes;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.RubyTags.Ja;

public class JaRubyTagGenerator : RubyTagGenerator<JaRubyTagGeneratorConfig>
{
    private readonly Analyzer analyzer;

    public JaRubyTagGenerator(JaRubyTagGeneratorConfig config)
        : base(config)
    {
        analyzer = Analyzer.NewAnonymous((fieldName, reader) =>
        {
            Tokenizer tokenizer = new JapaneseTokenizer(reader, null, true, JapaneseTokenizerMode.SEARCH);
            return new TokenStreamComponents(tokenizer, new JapaneseReadingFormFilter(tokenizer, false));
        });
    }

    protected override RubyTag[] GenerateFromItem(Lyric item)
    {
        // Tokenize the text
        string text = item.Text;
        var tokenStream = analyzer.GetTokenStream("dummy", new StringReader(text));

        return getProcessingRubyTags(text, tokenStream, Config).ToArray();
    }

    private static IEnumerable<RubyTag> getProcessingRubyTags(string text, TokenStream tokenStream, JaRubyTagGeneratorConfig config)
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

            // Convert to Hiragana as default.
            string hiragana = JpStringUtils.ToHiragana(katakana);

            if (!config.EnableDuplicatedRuby.Value)
            {
                // Not add duplicated ruby if same as parent.
                string parentText = text[offsetAttribute.StartOffset..offsetAttribute.EndOffset];
                if (parentText == katakana || parentText == hiragana)
                    continue;
            }

            // Make tag
            yield return new RubyTag
            {
                Text = config.RubyAsKatakana.Value ? katakana : hiragana,
                StartIndex = offsetAttribute.StartOffset,
                EndIndex = offsetAttribute.EndOffset - 1,
            };
        }

        // Dispose
        tokenStream.End();
        tokenStream.Dispose();
    }
}
