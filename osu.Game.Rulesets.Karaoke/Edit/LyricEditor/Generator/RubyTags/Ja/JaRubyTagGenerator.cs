// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Ja;
using Lucene.Net.Analysis.TokenAttributes;
using osu.Game.Rulesets.Karaoke.Objects;
using System.Collections.Generic;
using System.IO;

namespace osu.Game.Rulesets.Karaoke.Edit.LyricEditor.Generator.RubyTags.Ja
{
    public class JaRubyTagGenerator : RubyTagGenerator
    {
        private readonly Analyzer analyzer;

        public JaRubyTagGenerator()
        {
            analyzer = Analyzer.NewAnonymous((fieldName, reader) =>
            {
                Tokenizer tokenizer = new JapaneseTokenizer(reader, null, true, JapaneseTokenizerMode.SEARCH);
                return new TokenStreamComponents(tokenizer, new JapaneseReadingFormFilter(tokenizer, false));
            });
        }

        public override RubyTag[] CreateRubyTags(Lyric lyric)
        {
            var text = lyric.Text;
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

                // Get parsed result
                var parsedResult = result.ToString();
                if (string.IsNullOrEmpty(parsedResult))
                    break;

                // Make tag
                tags.Add(new RubyTag
                {
                    Text = parsedResult,
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
