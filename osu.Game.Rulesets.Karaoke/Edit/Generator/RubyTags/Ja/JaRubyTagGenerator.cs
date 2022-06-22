// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Collections.Generic;
using System.IO;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Ja;
using Lucene.Net.Analysis.TokenAttributes;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.RubyTags.Ja
{
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

        public override RubyTag[] Generate(Lyric lyric)
        {
            string text = lyric.Text;
            var tags = new List<RubyTag>();

            // Tokenize the text
            var tokenStream = analyzer.GetTokenStream("dummy", new StringReader(text));

            // Get result and offset
            var result = tokenStream.GetAttribute<ICharTermAttribute>();
            var offsetAtt = tokenStream.GetAttribute<IOffsetAttribute>();

            // Reset the stream and convert all result
            tokenStream.Reset();

            while (true)
            {
                // Read next token
                tokenStream.ClearAttributes();
                tokenStream.IncrementToken();

                // Get parsed result, result is Katakana.
                string katakana = result.ToString();
                if (string.IsNullOrEmpty(katakana))
                    break;

                // Convert to Hiragana as default.
                string hiragana = JpStringUtils.ToHiragana(katakana);

                if (!Config.EnableDuplicatedRuby)
                {
                    // Not add duplicated ruby if same as parent.
                    string parentText = text[offsetAtt.StartOffset..offsetAtt.EndOffset];
                    if (parentText == katakana || parentText == hiragana)
                        continue;
                }

                // Make tag
                tags.Add(new RubyTag
                {
                    Text = Config.RubyAsKatakana ? katakana : hiragana,
                    StartIndex = offsetAtt.StartOffset,
                    EndIndex = offsetAtt.EndOffset
                });
            }

            // Dispose
            tokenStream.End();
            tokenStream.Dispose();

            return tags.ToArray();
        }
    }
}
