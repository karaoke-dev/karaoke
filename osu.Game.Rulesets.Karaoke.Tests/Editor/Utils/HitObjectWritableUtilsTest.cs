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

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Utils
{
    public class HitObjectWritableUtilsTest
    {
        [Test]
        public void TestIsWriteLyricPropertyLocked()
        {
            test(new Lyric());

            void test(Lyric lyric)
                => testEveryWritablePropertyInObject<Lyric, Lyric>(lyric, (l, propertyName) => HitObjectWritableUtils.IsWriteLyricPropertyLocked(l, propertyName));
        }

        [Test]
        public void TestGetLyricPropertyLockedReason()
        {
            test(new Lyric());

            void test(Lyric lyric)
                => testEveryWritablePropertyInObject<Lyric, Lyric>(lyric, (l, propertyName) => HitObjectWritableUtils.GetLyricPropertyLockedReason(l, propertyName));
        }

        [Test]
        public void TestIsWriteLyricPropertyLockedByState()
        {
            test(LockState.None);
            test(LockState.Partial);
            test(LockState.Full);

            void test(LockState lockState)
                => testEveryWritablePropertyInObject<Lyric, LockState>(lockState, (l, propertyName) => HitObjectWritableUtils.IsWriteLyricPropertyLockedByState(l, propertyName));
        }

        [Test]
        public void TestIsWriteLyricPropertyLockedByConfig()
        {
            test(new SyncLyricConfig());
            test(new ReferenceLyricConfig());
            test(null);

            void test(IReferenceLyricPropertyConfig? config)
                => testEveryWritablePropertyInObject<Lyric, IReferenceLyricPropertyConfig?>(config, (c, propertyName) => HitObjectWritableUtils.IsWriteLyricPropertyLockedByConfig(c, propertyName));
        }

        [Test]
        public void TestIsWriteNotePropertyLocked()
        {
            test(new Note());

            void test(Note note)
                => testEveryWritablePropertyInObject<Note, Note>(note, (l, propertyName) => HitObjectWritableUtils.IsWriteNotePropertyLocked(l, propertyName));
        }

        [Test]
        public void TestGetNotePropertyLockedReason()
        {
            test(new Note());

            void test(Note note)
                => testEveryWritablePropertyInObject<Note, Note>(note, (l, propertyName) => HitObjectWritableUtils.GetNotePropertyLockedReason(l, propertyName));
        }

        [Test]
        public void TestIsWriteNotePropertyLockedByReferenceLyric()
        {
            test(new Lyric());

            void test(Lyric lyric)
                => testEveryWritablePropertyInObject<Note, Lyric>(lyric, (l, propertyName) => HitObjectWritableUtils.IsWriteNotePropertyLockedByReferenceLyric(l, propertyName));
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
