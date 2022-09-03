// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using Newtonsoft.Json;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Properties;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Utils
{
    public class HitObjectWritableUtilsTest
    {
        [Test]
        public void TestIsWriteLyricPropertyLocked()
        {
            // standard.
            test(new Lyric());

            // test lock state.
            foreach (var lockState in EnumUtils.GetValues<LockState>())
            {
                test(new Lyric
                {
                    Lock = lockState
                });
            }

            // reference lyric.
            test(new Lyric
            {
                ReferenceLyricConfig = new ReferenceLyricConfig(),
            });

            test(new Lyric
            {
                ReferenceLyricConfig = new SyncLyricConfig(),
            });

            void test(Lyric lyric)
                => testEveryWritablePropertyInObject<Lyric, Lyric>(lyric, (l, propertyName) => HitObjectWritableUtils.IsWriteLyricPropertyLocked(l, propertyName));
        }

        [Test]
        public void TestGetLyricPropertyLockedReason()
        {
            // standard.
            test(new Lyric());

            void test(Lyric lyric)
                => testEveryWritablePropertyInObject<Lyric, Lyric>(lyric, (l, propertyName) => HitObjectWritableUtils.GetLyricPropertyLockedReason(l, propertyName));
        }

        [Test]
        public void TestIsWriteNotePropertyLocked()
        {
            // standard.
            test(new Note());

            void test(Note note)
                => testEveryWritablePropertyInObject<Note, Note>(note, (l, propertyName) => HitObjectWritableUtils.IsWriteNotePropertyLocked(l, propertyName));
        }

        [Test]
        public void TestGetNotePropertyLockedReason()
        {
            // standard.
            test(new Note());

            // test with reference lyric.
            test(new Note
            {
                ReferenceLyric = new Lyric(),
            });

            void test(Note note)
                => testEveryWritablePropertyInObject<Note, Note>(note, (l, propertyName) => HitObjectWritableUtils.GetNotePropertyLockedReason(l, propertyName));
        }

        private void testEveryWritablePropertyInObject<THitObject, TProperty>(TProperty property, Action<TProperty, string> action)
        {
            // the purpose of this test case if focus on checking every property in the hit-object should be able to know the writable or not.
            // return value is not in the test scope.

            var allWriteableProperties = typeof(THitObject).GetProperties().Where(x => x.CanRead && x.CanWrite);

            foreach (var propertyInfo in allWriteableProperties)
            {
                if (propertyInfo.CustomAttributes.Any(x => x.AttributeType == typeof(JsonIgnoreAttribute)))
                    continue;

                action(property, propertyInfo.Name);
            }
        }
    }
}
