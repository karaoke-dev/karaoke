// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Collections.Generic;
using System.IO;
using System.Linq;
using osu.Framework.IO.Stores;
using osu.Game.IO.Archives;
using SharpCompress.Archives.Zip;

namespace osu.Game.Rulesets.Karaoke.IO.Archives
{
    /// <summary>
    /// For reading cached font reader.
    /// Cached font will be saved as xxx.cached fnt into cached folder.
    /// And notice that this class is just copied from <see cref="ZipArchiveReader"/>
    /// </summary>
    public class CachedFontArchiveReader : ArchiveReader
    {
        private readonly Stream archiveStream;
        private readonly ZipArchive archive;

        public CachedFontArchiveReader(Stream archiveStream, string name = null)
            : base(name)
        {
            this.archiveStream = archiveStream;
            archive = ZipArchive.Open(archiveStream);
        }

        public override Stream GetStream(string name)
        {
            // will search .fnt file or image in here.
            string file = Path.HasExtension(name) ? name : $"{name}.bin";
            var entry = archive.Entries.SingleOrDefault(e => e.Key == file);
            if (entry == null)
                throw new FileNotFoundException();

            // allow seeking
            var copy = new MemoryStream();

            using (var s = entry.OpenEntryStream())
                s.CopyTo(copy);

            copy.Position = 0;

            return copy;
        }

        public override void Dispose()
        {
            archive.Dispose();
            archiveStream.Dispose();
        }

        public override IEnumerable<string> Filenames => archive.Entries.Select(e => e.Key).ExcludeSystemFileNames();
    }
}
