// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using NUnit.Framework;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Game.Rulesets.Karaoke.Bindables;

namespace osu.Game.Rulesets.Karaoke.Tests.Bindables
{
    [TestFixture]
    public class BindableDictionaryTest
    {
        private BindableDictionary<int, string> bindableStringDictionary;

        [SetUp]
        public void SetUp()
        {
            bindableStringDictionary = new BindableDictionary<int, string>();
        }

        #region Constructor

        [Test]
        public void TestConstructorDoesNotAddItemsByDefault()
        {
            Assert.IsEmpty(bindableStringDictionary);
        }

        [Test]
        public void TestConstructorWithItemsAddsItemsInternally()
        {
            var dictionary = new Dictionary<int, string>
            {
                { 0 , "ok"},
                { 0 , "nope"},
                { 0 , "random"},
                { 0 , null },
                { 0 , ""}
            };

            var bindableDictionary = new BindableDictionary<int, string>(dictionary);

            Assert.Multiple(() =>
            {
                foreach (var item in dictionary)
                    Assert.Contains(item, bindableDictionary);

                Assert.AreEqual(dictionary.Count, bindableDictionary.Count);
            });
        }

        #endregion

        #region BindTarget

        /// <summary>
        /// Tests binding via the various <see cref="BindableDictionary{TKey, TValue}.BindTarget"/> methods.
        /// </summary>
        [Test]
        public void TestBindViaBindTarget()
        {
            BindableDictionary<int, string> parentBindable = new BindableDictionary<int, string>();

            BindableDictionary<int, string> bindable1 = new BindableDictionary<int, string>();
            IBindableDictionary<int, string> bindable2 = new BindableDictionary<int, string>();

            bindable1.BindTarget = parentBindable;
            bindable2.BindTarget = parentBindable;

            parentBindable.Add(5, "Test");

            Assert.That(bindable1[5], Is.EqualTo("Test"));
            Assert.That(bindable2[5], Is.EqualTo("Test"));
        }

        #endregion

        #region BindCollectionChanged

        [Test]
        public void TestBindCollectionChangedWithoutRunningImmediately()
        {
            var dictionary = new BindableDictionary<int, string> { { 1, "One" } };

            NotifyCollectionChangedEventArgs triggeredArgs = null;
            dictionary.BindCollectionChanged((_, args) => triggeredArgs = args);

            Assert.That(triggeredArgs, Is.Null);
        }

        [Test]
        public void TestBindCollectionChangedWithRunImmediately()
        {
            var dictionary = new BindableDictionary<int, string>();

            NotifyCollectionChangedEventArgs triggeredArgs = null;
            dictionary.BindCollectionChanged((_, args) => triggeredArgs = args, true);

            Assert.That(triggeredArgs.Action, Is.EqualTo(NotifyCollectionChangedAction.Add));
            Assert.That(triggeredArgs.NewItems, Is.EquivalentTo(dictionary));
        }

        #endregion

        #region dictionary[index]

        [Test]
        public void TestGetRetrievesObjectAtIndex()
        {
            bindableStringDictionary.Add(0, "0");
            bindableStringDictionary.Add(1, "1");
            bindableStringDictionary.Add(2, "2");

            Assert.AreEqual("1", bindableStringDictionary[1]);
        }

        [Test]
        public void TestSetMutatesObjectAtIndex()
        {
            bindableStringDictionary.Add(0, "0");
            bindableStringDictionary.Add(1, "1");
            bindableStringDictionary[1] = "2";

            Assert.AreEqual("2", bindableStringDictionary[1]);
        }

        [Test]
        public void TestGetWhileDisabledDoesNotThrowInvalidOperationException()
        {
            bindableStringDictionary.Add(0, "0");
            bindableStringDictionary.Disabled = true;

            Assert.AreEqual("0", bindableStringDictionary[0]);
        }

        [Test]
        public void TestSetWhileDisabledThrowsInvalidOperationException()
        {
            bindableStringDictionary.Add(0, "0");
            bindableStringDictionary.Disabled = true;

            Assert.Throws<InvalidOperationException>(() => bindableStringDictionary[0] = "1");
        }

        [Test]
        public void TestSetNotifiesSubscribers()
        {
            bindableStringDictionary.Add(0, "0");

            NotifyCollectionChangedEventArgs triggeredArgs = null;
            bindableStringDictionary.CollectionChanged += (_, args) => triggeredArgs = args;

            bindableStringDictionary[0] = "1";

            Assert.That(triggeredArgs.Action, Is.EqualTo(NotifyCollectionChangedAction.Replace));
            Assert.That(triggeredArgs.OldItems, Is.EquivalentTo("0".Yield()));
            Assert.That(triggeredArgs.NewItems, Is.EquivalentTo("1".Yield()));
            Assert.That(triggeredArgs.OldStartingIndex, Is.Zero);
            Assert.That(triggeredArgs.NewStartingIndex, Is.Zero);
        }

        [Test]
        public void TestSetNotifiesBoundLists()
        {
            bindableStringDictionary.Add(0, "0");

            var list = new BindableDictionary<int, string>();
            list.BindTo(bindableStringDictionary);

            NotifyCollectionChangedEventArgs triggeredArgs = null;
            list.CollectionChanged += (_, args) => triggeredArgs = args;

            bindableStringDictionary[0] = "1";

            Assert.That(triggeredArgs.Action, Is.EqualTo(NotifyCollectionChangedAction.Replace));
            Assert.That(triggeredArgs.OldItems, Is.EquivalentTo("0".Yield()));
            Assert.That(triggeredArgs.NewItems, Is.EquivalentTo("1".Yield()));
            Assert.That(triggeredArgs.OldStartingIndex, Is.Zero);
            Assert.That(triggeredArgs.NewStartingIndex, Is.Zero);
        }

        #endregion

        #region .Add(item)

        [TestCase("a random string")]
        [TestCase("", Description = "Empty string")]
        [TestCase(null)]
        public void TestAddWithStringAddsStringToEnumerator(string str)
        {
            bindableStringDictionary.Add(0, str);

            Assert.Contains(str, bindableStringDictionary);
        }

        [TestCase("a random string")]
        [TestCase("", Description = "Empty string")]
        [TestCase(null)]
        public void TestAddWithStringNotifiesSubscriber(string str)
        {
            NotifyCollectionChangedEventArgs triggeredArgs = null;
            bindableStringDictionary.CollectionChanged += (_, args) => triggeredArgs = args;

            bindableStringDictionary.Add(0, str);

            Assert.That(triggeredArgs.Action, Is.EqualTo(NotifyCollectionChangedAction.Add));
            Assert.That(triggeredArgs.NewItems, Is.EquivalentTo(str.Yield()));
            Assert.That(triggeredArgs.NewStartingIndex, Is.Zero);
        }

        [TestCase("a random string")]
        [TestCase("", Description = "Empty string")]
        [TestCase(null)]
        public void TestAddWithStringNotifiesSubscriberOnce(string str)
        {
            var triggeredArgs = new List<NotifyCollectionChangedEventArgs>();
            bindableStringDictionary.CollectionChanged += (_, args) => triggeredArgs.Add(args);

            bindableStringDictionary.Add(0, str);

            Assert.That(triggeredArgs, Has.Count.EqualTo(1));
        }

        [TestCase("a random string")]
        [TestCase("", Description = "Empty string")]
        [TestCase(null)]
        public void TestAddWithStringNotifiesMultipleSubscribers(string str)
        {
            NotifyCollectionChangedEventArgs triggeredArgsA = null;
            NotifyCollectionChangedEventArgs triggeredArgsB = null;
            NotifyCollectionChangedEventArgs triggeredArgsC = null;
            bindableStringDictionary.CollectionChanged += (_, args) => triggeredArgsA = args;
            bindableStringDictionary.CollectionChanged += (_, args) => triggeredArgsB = args;
            bindableStringDictionary.CollectionChanged += (_, args) => triggeredArgsC = args;

            bindableStringDictionary.Add(0, str);

            Assert.That(triggeredArgsA, Is.Not.Null);
            Assert.That(triggeredArgsB, Is.Not.Null);
            Assert.That(triggeredArgsC, Is.Not.Null);
        }

        [TestCase("a random string")]
        [TestCase("", Description = "Empty string")]
        [TestCase(null)]
        public void TestAddWithStringNotifiesMultipleSubscribersOnlyAfterTheAdd(string str)
        {
            NotifyCollectionChangedEventArgs triggeredArgsA = null;
            NotifyCollectionChangedEventArgs triggeredArgsB = null;
            NotifyCollectionChangedEventArgs triggeredArgsC = null;
            bindableStringDictionary.CollectionChanged += (_, args) => triggeredArgsA = args;
            bindableStringDictionary.CollectionChanged += (_, args) => triggeredArgsB = args;
            bindableStringDictionary.CollectionChanged += (_, args) => triggeredArgsC = args;

            Assert.That(triggeredArgsA, Is.Null);
            Assert.That(triggeredArgsB, Is.Null);
            Assert.That(triggeredArgsC, Is.Null);

            bindableStringDictionary.Add(0, str);
        }

        [TestCase("a random string")]
        [TestCase("", Description = "Empty string")]
        [TestCase(null)]
        public void TestAddWithStringNotifiesBoundList(string str)
        {
            var list = new BindableDictionary<int, string>();
            list.BindTo(bindableStringDictionary);

            bindableStringDictionary.Add(0, str);

            Assert.Contains(str, list);
        }

        [TestCase("a random string")]
        [TestCase("", Description = "Empty string")]
        [TestCase(null)]
        public void TestAddWithStringNotifiesBoundLists(string str)
        {
            var listA = new BindableDictionary<int, string>();
            var listB = new BindableDictionary<int, string>();
            var listC = new BindableDictionary<int, string>();
            listA.BindTo(bindableStringDictionary);
            listB.BindTo(bindableStringDictionary);
            listC.BindTo(bindableStringDictionary);

            bindableStringDictionary.Add(0, str);

            Assert.Contains(str, listA);
            Assert.Contains(str, listB);
            Assert.Contains(str, listC);
        }

        [TestCase("a random string")]
        [TestCase("", Description = "Empty string")]
        [TestCase(null)]
        public void TestAddWithDisabledListThrowsInvalidOperationException(string str)
        {
            bindableStringDictionary.Disabled = true;

            Assert.Throws<InvalidOperationException>(() => { bindableStringDictionary.Add(0, str); });
        }

        [TestCase("a random string")]
        [TestCase("", Description = "Empty string")]
        [TestCase(null)]
        public void TestAddWithListContainingItemsDoesNotOverrideItems(string str)
        {
            const string item = "existing string";
            bindableStringDictionary.Add(0, item);

            bindableStringDictionary.Add(0, str);

            Assert.Contains(item, bindableStringDictionary);
        }

        #endregion

        #region .AddRange(items)

        [Test]
        public void TestAddRangeAddsItemsToEnumerator()
        {
            var items = new Dictionary<int, string>
            {
                { 0, "A" },
                { 1, "B" },
                { 2, "C" },
                { 3, "D" },
            };

            bindableStringDictionary.AddRange(items);

            Assert.Multiple(() =>
            {
                foreach (var item in items)
                    Assert.Contains(item, bindableStringDictionary);
            });
        }

        [Test]
        public void TestAddRangeNotifiesBoundLists()
        {
            var items = new Dictionary<int, string>
            {
                { 0, "test1" },
                { 1, "test2" },
                { 2, "test3" },
            };
            var list = new BindableDictionary<int, string>();
            bindableStringDictionary.BindTo(list);

            NotifyCollectionChangedEventArgs triggeredArgs = null;
            list.CollectionChanged += (_, args) => triggeredArgs = args;

            bindableStringDictionary.AddRange(items);

            Assert.That(triggeredArgs.Action, Is.EqualTo(NotifyCollectionChangedAction.Add));
            Assert.That(triggeredArgs.NewItems, Is.EquivalentTo(items));
        }

        [Test]
        public void TestAddRangeEnumeratesOnlyOnce()
        {
            BindableDictionary<int, string> list1 = new BindableDictionary<int, string>();
            BindableDictionary<int, string> list2 = new BindableDictionary<int, string>();
            list2.BindTo(list1);

            int counter = 0;

            IEnumerable<int> valueEnumerable()
            {
                yield return counter++;
            }

            list1.AddRange(valueEnumerable());

            Assert.That(list1, Is.EquivalentTo(0.Yield()));
            Assert.That(list2, Is.EquivalentTo(0.Yield()));
            Assert.That(counter, Is.EqualTo(1));
        }

        #endregion

        #region .Move(item)

        [Test]
        public void TestMoveWithDisabledListThrowsInvalidOperationException()
        {
            bindableStringDictionary.AddRange(new[] { "0", "1", "2" });
            bindableStringDictionary.Disabled = true;

            Assert.Throws(typeof(InvalidOperationException), () => bindableStringDictionary.Move(0, 2));
        }

        [Test]
        public void TestMoveMovesTheItem()
        {
            bindableStringDictionary.AddRange(new[] { "0", "1", "2" });

            bindableStringDictionary.Move(0, 1);

            Assert.That(bindableStringDictionary, Is.EquivalentTo(new[] { "1", "0", "2" }));
        }

        [Test]
        public void TestMoveNotifiesSubscriber()
        {
            bindableStringDictionary.AddRange(new[] { "0", "1", "2" });

            NotifyCollectionChangedEventArgs triggeredArgs = null;
            bindableStringDictionary.CollectionChanged += (_, args) => triggeredArgs = args;

            bindableStringDictionary.Move(0, 1);

            Assert.That(triggeredArgs.Action, Is.EqualTo(NotifyCollectionChangedAction.Move));
            Assert.That(triggeredArgs.OldItems, Is.EquivalentTo("0".Yield()));
            Assert.That(triggeredArgs.NewItems, Is.EquivalentTo("0".Yield()));
            Assert.That(triggeredArgs.OldStartingIndex, Is.EqualTo(0));
            Assert.That(triggeredArgs.NewStartingIndex, Is.EqualTo(1));
        }

        [Test]
        public void TestMoveNotifiesSubscribers()
        {
            bindableStringDictionary.AddRange(new[] { "0", "1", "2" });

            NotifyCollectionChangedEventArgs triggeredArgsA = null;
            NotifyCollectionChangedEventArgs triggeredArgsB = null;
            NotifyCollectionChangedEventArgs triggeredArgsC = null;
            bindableStringDictionary.CollectionChanged += (_, args) => triggeredArgsA = args;
            bindableStringDictionary.CollectionChanged += (_, args) => triggeredArgsB = args;
            bindableStringDictionary.CollectionChanged += (_, args) => triggeredArgsC = args;

            bindableStringDictionary.Move(0, 1);

            Assert.That(triggeredArgsA, Is.Not.Null);
            Assert.That(triggeredArgsB, Is.Not.Null);
            Assert.That(triggeredArgsC, Is.Not.Null);
        }

        [Test]
        public void TestMoveNotifiesBoundList()
        {
            bindableStringDictionary.AddRange(new[] { "0", "1", "2" });
            var list = new BindableDictionary<int, string>();
            list.BindTo(bindableStringDictionary);

            bindableStringDictionary.Move(0, 1);

            Assert.That(list, Is.EquivalentTo(new[] { "1", "0", "2" }));
        }

        [Test]
        public void TestMoveNotifiesBoundLists()
        {
            bindableStringDictionary.AddRange(new[] { "0", "1", "2" });
            var listA = new BindableDictionary<int, string>();
            listA.BindTo(bindableStringDictionary);
            var listB = new BindableDictionary<int, string>();
            listB.BindTo(bindableStringDictionary);
            var listC = new BindableDictionary<int, string>();
            listC.BindTo(bindableStringDictionary);

            bindableStringDictionary.Move(0, 1);

            Assert.Multiple(() =>
            {
                Assert.That(listA, Is.EquivalentTo(new[] { "1", "0", "2" }));
                Assert.That(listB, Is.EquivalentTo(new[] { "1", "0", "2" }));
                Assert.That(listC, Is.EquivalentTo(new[] { "1", "0", "2" }));
            });
        }

        [Test]
        public void TestMoveNotifiesBoundListSubscription()
        {
            bindableStringDictionary.AddRange(new[] { "0", "1", "2" });
            var list = new BindableDictionary<int, string>();
            list.BindTo(bindableStringDictionary);

            NotifyCollectionChangedEventArgs triggeredArgs = null;
            list.CollectionChanged += (_, args) => triggeredArgs = args;

            bindableStringDictionary.Move(0, 1);

            Assert.That(triggeredArgs.Action, Is.EqualTo(NotifyCollectionChangedAction.Move));
            Assert.That(triggeredArgs.OldItems, Is.EquivalentTo("0".Yield()));
            Assert.That(triggeredArgs.NewItems, Is.EquivalentTo("0".Yield()));
            Assert.That(triggeredArgs.OldStartingIndex, Is.EqualTo(0));
            Assert.That(triggeredArgs.NewStartingIndex, Is.EqualTo(1));
        }

        [Test]
        public void TestMoveNotifiesBoundListSubscriptions()
        {
            bindableStringDictionary.AddRange(new[] { "0", "1", "2" });
            var listA = new BindableDictionary<int, string>();
            listA.BindTo(bindableStringDictionary);

            NotifyCollectionChangedEventArgs triggeredArgsA1 = null;
            NotifyCollectionChangedEventArgs triggeredArgsA2 = null;
            listA.CollectionChanged += (_, args) => triggeredArgsA1 = args;
            listA.CollectionChanged += (_, args) => triggeredArgsA2 = args;

            var listB = new BindableDictionary<int, string>();
            listB.BindTo(bindableStringDictionary);

            NotifyCollectionChangedEventArgs triggeredArgsB1 = null;
            NotifyCollectionChangedEventArgs triggeredArgsB2 = null;
            listB.CollectionChanged += (_, args) => triggeredArgsB1 = args;
            listB.CollectionChanged += (_, args) => triggeredArgsB2 = args;

            bindableStringDictionary.Move(0, 1);

            Assert.That(triggeredArgsA1, Is.Not.Null);
            Assert.That(triggeredArgsA2, Is.Not.Null);
            Assert.That(triggeredArgsB1, Is.Not.Null);
            Assert.That(triggeredArgsB2, Is.Not.Null);
        }

        #endregion

        #region .Insert

        [Test]
        public void TestInsertInsertsItemAtIndex()
        {
            bindableStringDictionary.Add(0, "0");
            bindableStringDictionary.Add(2, "2");

            bindableStringDictionary.Insert(1, "1");

            Assert.Multiple(() =>
            {
                Assert.AreEqual("0", bindableStringDictionary[0]);
                Assert.AreEqual("1", bindableStringDictionary[1]);
                Assert.AreEqual("2", bindableStringDictionary[2]);
            });
        }

        [Test]
        public void TestInsertNotifiesSubscribers()
        {
            bindableStringDictionary.Add(0, "0");
            bindableStringDictionary.Add(2, "2");

            NotifyCollectionChangedEventArgs triggeredArgs = null;
            bindableStringDictionary.CollectionChanged += (_, args) => triggeredArgs = args;

            bindableStringDictionary.Insert(1, "1");

            Assert.That(triggeredArgs.Action, Is.EqualTo(NotifyCollectionChangedAction.Add));
            Assert.That(triggeredArgs.NewItems, Has.One.Items.EqualTo("1"));
            Assert.That(triggeredArgs.NewStartingIndex, Is.EqualTo(1));
        }

        [Test]
        public void TestInsertNotifiesBoundLists()
        {
            bindableStringDictionary.Add(0, "0");
            bindableStringDictionary.Add(2, "2");

            var list = new BindableDictionary<int, string>();
            list.BindTo(bindableStringDictionary);

            NotifyCollectionChangedEventArgs triggeredArgs = null;
            list.CollectionChanged += (_, args) => triggeredArgs = args;

            bindableStringDictionary.Insert(1, "1");

            Assert.That(triggeredArgs.Action, Is.EqualTo(NotifyCollectionChangedAction.Add));
            Assert.That(triggeredArgs.NewItems, Has.One.Items.EqualTo("1"));
            Assert.That(triggeredArgs.NewStartingIndex, Is.EqualTo(1));
        }

        [Test]
        public void TestInsertInsertsItemAtIndexInBoundList()
        {
            bindableStringDictionary.Add(0, "0");
            bindableStringDictionary.Add(2, "2");

            var list = new BindableDictionary<int, string>();
            list.BindTo(bindableStringDictionary);

            bindableStringDictionary.Insert(1, "1");

            Assert.Multiple(() =>
            {
                Assert.AreEqual("0", list[0]);
                Assert.AreEqual("1", list[1]);
                Assert.AreEqual("2", list[2]);
            });
        }

        #endregion

        #region .Remove(item)

        [Test]
        public void TestRemoveWithDisabledListThrowsInvalidOperationException()
        {
            const string item = "hi";
            bindableStringDictionary.Add(0, item);
            bindableStringDictionary.Disabled = true;

            Assert.Throws(typeof(InvalidOperationException), () => bindableStringDictionary.Remove(item));
        }

        [Test]
        public void TestRemoveWithAnItemThatIsNotInTheListReturnsFalse()
        {
            bool gotRemoved = bindableStringDictionary.Remove(0, "hm");

            Assert.IsFalse(gotRemoved);
        }

        [Test]
        public void TestRemoveWhenListIsDisabledThrowsInvalidOperationException()
        {
            const string item = "item";
            bindableStringDictionary.Add(0, item);
            bindableStringDictionary.Disabled = true;

            Assert.Throws<InvalidOperationException>(() => { bindableStringDictionary.Remove(item); });
        }

        [Test]
        public void TestRemoveWithAnItemThatIsInTheListReturnsTrue()
        {
            const string item = "item";
            bindableStringDictionary.Add(0, item);

            bool gotRemoved = bindableStringDictionary.Remove(0, item);

            Assert.IsTrue(gotRemoved);
        }

        [Test]
        public void TestRemoveNotifiesSubscriber()
        {
            const string item = "item";
            bindableStringDictionary.Add(0, item);

            NotifyCollectionChangedEventArgs triggeredArgs = null;
            bindableStringDictionary.CollectionChanged += (_, args) => triggeredArgs = args;

            bindableStringDictionary.Remove(item);

            Assert.That(triggeredArgs.Action, Is.EqualTo(NotifyCollectionChangedAction.Remove));
            Assert.That(triggeredArgs.OldItems, Has.One.Items.EqualTo(item));
            Assert.That(triggeredArgs.OldStartingIndex, Is.EqualTo(0));
        }

        [Test]
        public void TestRemoveNotifiesSubscriberWithCorrectReference()
        {
            var item = new TestAlwaysEqualModel();

            var bindableObjectList = new BindableList<TestAlwaysEqualModel> { item };

            NotifyCollectionChangedEventArgs triggeredArgs = null;
            bindableObjectList.CollectionChanged += (_, args) => triggeredArgs = args;

            bindableObjectList.Remove(new TestAlwaysEqualModel());

            Assert.That(triggeredArgs.OldItems[0] == item, Is.True);
        }

        [Test]
        public void TestRemoveDoesntNotifySubscribersOnNoOp()
        {
            const string item = "item";
            bindableStringDictionary.Add(0, item);

            NotifyCollectionChangedEventArgs triggeredArgs = null;

            bindableStringDictionary.Remove(item);

            bindableStringDictionary.CollectionChanged += (_, args) => triggeredArgs = args;

            bindableStringDictionary.Remove(item);

            Assert.That(triggeredArgs, Is.Null);
        }

        [Test]
        public void TestRemoveNotifiesSubscribers()
        {
            const string item = "item";
            bindableStringDictionary.Add(0, item);

            NotifyCollectionChangedEventArgs triggeredArgsA = null;
            NotifyCollectionChangedEventArgs triggeredArgsB = null;
            NotifyCollectionChangedEventArgs triggeredArgsC = null;
            bindableStringDictionary.CollectionChanged += (_, args) => triggeredArgsA = args;
            bindableStringDictionary.CollectionChanged += (_, args) => triggeredArgsB = args;
            bindableStringDictionary.CollectionChanged += (_, args) => triggeredArgsC = args;

            bindableStringDictionary.Remove(item);

            Assert.That(triggeredArgsA, Is.Not.Null);
            Assert.That(triggeredArgsB, Is.Not.Null);
            Assert.That(triggeredArgsC, Is.Not.Null);
        }

        [Test]
        public void TestRemoveNotifiesBoundList()
        {
            const string item = "item";
            bindableStringDictionary.Add(0, item);
            var list = new BindableDictionary<int, string>();
            list.BindTo(bindableStringDictionary);

            bindableStringDictionary.Remove(item);

            Assert.IsEmpty(list);
        }

        [Test]
        public void TestRemoveNotifiesBoundLists()
        {
            const string item = "item";
            bindableStringDictionary.Add(0, item);
            var listA = new BindableDictionary<int, string>();
            listA.BindTo(bindableStringDictionary);
            var listB = new BindableDictionary<int, string>();
            listB.BindTo(bindableStringDictionary);
            var listC = new BindableDictionary<int, string>();
            listC.BindTo(bindableStringDictionary);

            bindableStringDictionary.Remove(item);

            Assert.Multiple(() =>
            {
                Assert.False(listA.Contains(item));
                Assert.False(listB.Contains(item));
                Assert.False(listC.Contains(item));
            });
        }

        [Test]
        public void TestRemoveNotifiesBoundListSubscription()
        {
            const string item = "item";
            bindableStringDictionary.Add(0, item);
            var list = new BindableDictionary<int, string>();
            list.BindTo(bindableStringDictionary);

            NotifyCollectionChangedEventArgs triggeredArgs = null;
            list.CollectionChanged += (_, args) => triggeredArgs = args;

            bindableStringDictionary.Remove(item);

            Assert.That(triggeredArgs.Action, Is.EqualTo(NotifyCollectionChangedAction.Remove));
            Assert.That(triggeredArgs.OldItems, Has.One.Items.EqualTo(item));
            Assert.That(triggeredArgs.OldStartingIndex, Is.EqualTo(0));
        }

        [Test]
        public void TestRemoveNotifiesBoundListSubscriptions()
        {
            const string item = "item";
            bindableStringDictionary.Add(0, item);
            var listA = new BindableDictionary<int, string>();
            listA.BindTo(bindableStringDictionary);

            NotifyCollectionChangedEventArgs triggeredArgsA1 = null;
            NotifyCollectionChangedEventArgs triggeredArgsA2 = null;
            listA.CollectionChanged += (_, args) => triggeredArgsA1 = args;
            listA.CollectionChanged += (_, args) => triggeredArgsA2 = args;

            var listB = new BindableDictionary<int, string>();
            listB.BindTo(bindableStringDictionary);

            NotifyCollectionChangedEventArgs triggeredArgsB1 = null;
            NotifyCollectionChangedEventArgs triggeredArgsB2 = null;
            listB.CollectionChanged += (_, args) => triggeredArgsB1 = args;
            listB.CollectionChanged += (_, args) => triggeredArgsB2 = args;

            bindableStringDictionary.Remove(item);

            Assert.That(triggeredArgsA1, Is.Not.Null);
            Assert.That(triggeredArgsA2, Is.Not.Null);
            Assert.That(triggeredArgsB1, Is.Not.Null);
            Assert.That(triggeredArgsB2, Is.Not.Null);
        }

        [Test]
        public void TestRemoveDoesNotNotifySubscribersBeforeItemIsRemoved()
        {
            const string item = "item";
            bindableStringDictionary.Add(0, item);

            NotifyCollectionChangedEventArgs triggeredArgs = null;
            bindableStringDictionary.CollectionChanged += (_, args) => triggeredArgs = args;

            Assert.That(triggeredArgs, Is.Null);
        }

        #endregion

        #region .RemoveRange(index, count)

        [TestCase(1, 0, 1)]
        [TestCase(0, 0, 0)]
        [TestCase(1000, 999, 1)]
        [TestCase(3, 1, 1)]
        [TestCase(10, 0, 9)]
        [TestCase(10, 0, 0)]
        public void TestRemoveRangeRemovesRange(int totalCount, int startIndex, int removeCount)
        {
            for (int i = 0; i < totalCount; i++)
                bindableStringDictionary.Add(i, $"test{i}");

            bindableStringDictionary.RemoveRange(startIndex, removeCount);

            Assert.AreEqual(totalCount - removeCount, bindableStringDictionary.Count);

            var remainingItems = new List<string>();

            for (int i = 0; i < startIndex; i++)
                remainingItems.Add("test" + i);
            for (int i = startIndex + removeCount; i < totalCount; i++)
                remainingItems.Add("test" + i);

            CollectionAssert.AreEqual(remainingItems, bindableStringDictionary);
        }

        [Test]
        public void TestRemoveRangeNotifiesSubscribers()
        {
            bindableStringDictionary.Add(0, "0");
            bindableStringDictionary.Add(1, "1");

            NotifyCollectionChangedEventArgs triggeredArgs = null;
            bindableStringDictionary.CollectionChanged += (_, args) => triggeredArgs = args;

            bindableStringDictionary.RemoveRange(1, 1);

            Assert.AreEqual(1, bindableStringDictionary.Count);
            Assert.AreEqual("0", bindableStringDictionary.Single());

            Assert.That(triggeredArgs.Action, Is.EqualTo(NotifyCollectionChangedAction.Remove));
            Assert.That(triggeredArgs.OldItems, Has.One.Items.EqualTo("1"));
            Assert.That(triggeredArgs.OldStartingIndex, Is.EqualTo(1));
        }

        [Test]
        public void TestRemoveRangeNotifiesBoundLists()
        {
            bindableStringDictionary.Add(0, "0");
            bindableStringDictionary.Add(1, "1");

            var list = new BindableDictionary<int, string>();
            list.BindTo(bindableStringDictionary);

            NotifyCollectionChangedEventArgs triggeredArgs = null;
            list.CollectionChanged += (_, args) => triggeredArgs = args;

            bindableStringDictionary.RemoveRange(0, 1);

            Assert.That(triggeredArgs.Action, Is.EqualTo(NotifyCollectionChangedAction.Remove));
            Assert.That(triggeredArgs.OldItems, Has.One.Items.EqualTo("0"));
            Assert.That(triggeredArgs.OldStartingIndex, Is.EqualTo(0));
        }

        [Test]
        public void TestRemoveRangeDoesNotNotifyBoundListsWhenCountIsZero()
        {
            bindableStringDictionary.Add(0, "0");

            var list = new BindableDictionary<int, string>();
            list.BindTo(bindableStringDictionary);

            NotifyCollectionChangedEventArgs triggeredArgs = null;
            list.CollectionChanged += (_, args) => triggeredArgs = args;

            bindableStringDictionary.RemoveRange(0, 0);

            Assert.That(triggeredArgs, Is.Null);
        }

        #endregion

        #region .RemoveAt(index)

        [Test]
        public void TestRemoveAtRemovesItemAtIndex()
        {
            bindableStringDictionary.Add(0, "0");
            bindableStringDictionary.Add(1, "1");
            bindableStringDictionary.Add(2, "2");

            bindableStringDictionary.RemoveAt(1);

            Assert.AreEqual("0", bindableStringDictionary[0]);
            Assert.AreEqual("2", bindableStringDictionary[1]);
        }

        [Test]
        public void TestRemoveAtWithDisabledListThrowsInvalidOperationException()
        {
            bindableStringDictionary.Add(0, "abc");
            bindableStringDictionary.Disabled = true;

            Assert.Throws<InvalidOperationException>(() => bindableStringDictionary.RemoveAt(0));
        }

        [Test]
        public void TestRemoveAtNotifiesSubscribers()
        {
            bindableStringDictionary.Add(0, "abc");

            NotifyCollectionChangedEventArgs triggeredArgs = null;
            bindableStringDictionary.CollectionChanged += (_, args) => triggeredArgs = args;

            bindableStringDictionary.RemoveAt(0);

            Assert.That(triggeredArgs.Action, Is.EqualTo(NotifyCollectionChangedAction.Remove));
            Assert.That(triggeredArgs.OldItems, Has.One.Items.EqualTo("abc"));
            Assert.That(triggeredArgs.OldStartingIndex, Is.EqualTo(0));
        }

        [Test]
        public void TestRemoveAtNotifiesBoundLists()
        {
            bindableStringDictionary.Add(0, "abc");

            var list = new BindableDictionary<int, string>();
            list.BindTo(bindableStringDictionary);

            NotifyCollectionChangedEventArgs triggeredArgs = null;
            list.CollectionChanged += (_, args) => triggeredArgs = args;

            bindableStringDictionary.RemoveAt(0);

            Assert.That(triggeredArgs.Action, Is.EqualTo(NotifyCollectionChangedAction.Remove));
            Assert.That(triggeredArgs.OldItems, Has.One.Items.EqualTo("abc"));
            Assert.That(triggeredArgs.OldStartingIndex, Is.EqualTo(0));
        }

        #endregion

        #region .RemoveAll(match)

        [Test]
        public void TestRemoveAllRemovesMatchingElements()
        {
            bindableStringDictionary.Add(0, "0");
            bindableStringDictionary.Add(1, "0");
            bindableStringDictionary.Add(2, "0");
            bindableStringDictionary.Add(3, "1");
            bindableStringDictionary.Add(4, "2");

            bindableStringDictionary.RemoveAll(m => m == "0");

            Assert.AreEqual(2, bindableStringDictionary.Count);
            Assert.Multiple(() =>
            {
                Assert.AreEqual("1", bindableStringDictionary[0]);
                Assert.AreEqual("2", bindableStringDictionary[1]);
            });
        }

        [Test]
        public void TestRemoveAllNotifiesSubscribers()
        {
            bindableStringDictionary.Add(0, "0");
            bindableStringDictionary.Add(1, "0");

            NotifyCollectionChangedEventArgs triggeredArgs = null;
            bindableStringDictionary.CollectionChanged += (_, args) => triggeredArgs = args;

            bindableStringDictionary.RemoveAll(m => m == "0");

            Assert.That(triggeredArgs.Action, Is.EqualTo(NotifyCollectionChangedAction.Remove));
            Assert.That(triggeredArgs.OldItems, Is.EquivalentTo(new[] { "0", "0" }));
        }

        [Test]
        public void TestRemoveAllNoopDoesntNotifySubscibers()
        {
            bindableStringDictionary.Add(0, "0");
            bindableStringDictionary.Add(1, "0");

            NotifyCollectionChangedEventArgs triggeredArgs = null;
            bindableStringDictionary.CollectionChanged += (_, args) => triggeredArgs = args;

            bindableStringDictionary.RemoveAll(m => m == "1");

            Assert.That(triggeredArgs, Is.Null);
        }

        [Test]
        public void TestRemoveAllNotifiesBoundLists()
        {
            bindableStringDictionary.Add(0, "0");
            bindableStringDictionary.Add(1, "0");

            var dictionary = new BindableDictionary<int, string>();
            dictionary.BindTo(bindableStringDictionary);

            NotifyCollectionChangedEventArgs triggeredArgs = null;
            dictionary.CollectionChanged += (_, args) => triggeredArgs = args;

            bindableStringDictionary.RemoveAll(m => m == "0");

            Assert.That(triggeredArgs.Action, Is.EqualTo(NotifyCollectionChangedAction.Remove));
            Assert.That(triggeredArgs.OldItems, Is.EquivalentTo(new[] { "0", "0" }));
        }

        #endregion

        #region .Clear()

        [Test]
        public void TestClear()
        {
            for (int i = 0; i < 5; i++)
                bindableStringDictionary.Add(i, "testA");

            bindableStringDictionary.Clear();

            Assert.IsEmpty(bindableStringDictionary);
        }

        [Test]
        public void TestClearWithDisabledListThrowsInvalidOperationException()
        {
            for (int i = 0; i < 5; i++)
                bindableStringDictionary.Add(i, "testA");
            bindableStringDictionary.Disabled = true;

            Assert.Throws(typeof(InvalidOperationException), () => bindableStringDictionary.Clear());
        }

        [Test]
        public void TestClearWithEmptyDisabledListThrowsInvalidOperationException()
        {
            bindableStringDictionary.Disabled = true;

            Assert.Throws(typeof(InvalidOperationException), () => bindableStringDictionary.Clear());
        }

        [Test]
        public void TestClearUpdatesCountProperty()
        {
            for (int i = 0; i < 5; i++)
                bindableStringDictionary.Add(i, "testA");

            bindableStringDictionary.Clear();

            Assert.AreEqual(0, bindableStringDictionary.Count);
        }

        [Test]
        public void TestClearNotifiesSubscriber()
        {
            for (int i = 0; i < 5; i++)
                bindableStringDictionary.Add(i, "testA");

            NotifyCollectionChangedEventArgs triggeredArgs = null;
            bindableStringDictionary.CollectionChanged += (_, args) => triggeredArgs = args;

            bindableStringDictionary.Clear();

            Assert.That(triggeredArgs.Action, Is.EqualTo(NotifyCollectionChangedAction.Remove));
            Assert.That(triggeredArgs.OldItems, Is.EquivalentTo(new[] { "testA", "testA", "testA", "testA", "testA" }));
            Assert.That(triggeredArgs.OldStartingIndex, Is.EqualTo(0));
        }

        [Test]
        public void TestClearDoesNotNotifySubscriberBeforeClear()
        {
            for (int i = 0; i < 5; i++)
                bindableStringDictionary.Add(i, "testA");

            NotifyCollectionChangedEventArgs triggeredArgs = null;
            bindableStringDictionary.CollectionChanged += (_, args) => triggeredArgs = args;

            Assert.That(triggeredArgs, Is.Null);

            bindableStringDictionary.Clear();
        }

        [Test]
        public void TestClearNotifiesSubscribers()
        {
            for (int i = 0; i < 5; i++)
                bindableStringDictionary.Add(i, "testA");

            NotifyCollectionChangedEventArgs triggeredArgsA = null;
            NotifyCollectionChangedEventArgs triggeredArgsB = null;
            NotifyCollectionChangedEventArgs triggeredArgsC = null;
            bindableStringDictionary.CollectionChanged += (_, args) => triggeredArgsA = args;
            bindableStringDictionary.CollectionChanged += (_, args) => triggeredArgsB = args;
            bindableStringDictionary.CollectionChanged += (_, args) => triggeredArgsC = args;

            bindableStringDictionary.Clear();

            Assert.That(triggeredArgsA, Is.Not.Null);
            Assert.That(triggeredArgsB, Is.Not.Null);
            Assert.That(triggeredArgsC, Is.Not.Null);
        }

        [Test]
        public void TestClearNotifiesBoundBindable()
        {
            var bindableList = new BindableDictionary<int, string>();
            bindableList.BindTo(bindableStringDictionary);
            for (int i = 0; i < 5; i++)
                bindableStringDictionary.Add(i, "testA");
            for (int i = 0; i < 5; i++)
                bindableList.Add(i, "testA");

            bindableStringDictionary.Clear();

            Assert.IsEmpty(bindableList);
        }

        [Test]
        public void TestClearNotifiesBoundBindables()
        {
            var bindableListA = new BindableDictionary<int, string>();
            bindableListA.BindTo(bindableStringDictionary);
            var bindableListB = new BindableDictionary<int, string>();
            bindableListB.BindTo(bindableStringDictionary);
            var bindableListC = new BindableDictionary<int, string>();
            bindableListC.BindTo(bindableStringDictionary);
            for (int i = 0; i < 5; i++)
                bindableStringDictionary.Add(i, "testA");
            for (int i = 0; i < 5; i++)
                bindableListA.Add(i, "testA");
            for (int i = 0; i < 5; i++)
                bindableListB.Add(i, "testA");
            for (int i = 0; i < 5; i++)
                bindableListC.Add(i, "testA");

            bindableStringDictionary.Clear();

            Assert.Multiple(() =>
            {
                Assert.IsEmpty(bindableListA);
                Assert.IsEmpty(bindableListB);
                Assert.IsEmpty(bindableListC);
            });
        }

        [Test]
        public void TestClearDoesNotNotifyBoundBindablesBeforeClear()
        {
            var bindableListA = new BindableDictionary<int, string>();
            bindableListA.BindTo(bindableStringDictionary);
            var bindableListB = new BindableDictionary<int, string>();
            bindableListB.BindTo(bindableStringDictionary);
            var bindableListC = new BindableDictionary<int, string>();
            bindableListC.BindTo(bindableStringDictionary);
            for (int i = 0; i < 5; i++)
                bindableStringDictionary.Add(i, "testA");
            for (int i = 0; i < 5; i++)
                bindableListA.Add(i, "testA");
            for (int i = 0; i < 5; i++)
                bindableListB.Add(i, "testA");
            for (int i = 0; i < 5; i++)
                bindableListC.Add(i, "testA");

            Assert.Multiple(() =>
            {
                Assert.IsNotEmpty(bindableListA);
                Assert.IsNotEmpty(bindableListB);
                Assert.IsNotEmpty(bindableListC);
            });

            bindableStringDictionary.Clear();
        }

        #endregion

        #region .CopyTo(array, index)

        [Test]
        public void TestCopyTo()
        {
            for (int i = 0; i < 5; i++)
                bindableStringDictionary.Add(i, $"test{i}");
            string[] array = new string[5];

            bindableStringDictionary.CopyTo(array, 0);

            CollectionAssert.AreEquivalent(bindableStringDictionary, array);
        }

        #endregion

        #region .Disabled

        [Test]
        public void TestDisabledWhenSetToTrueNotifiesSubscriber()
        {
            bool? isDisabled = null;
            bindableStringDictionary.DisabledChanged += b => isDisabled = b;

            bindableStringDictionary.Disabled = true;

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(isDisabled);
                Assert.IsTrue(isDisabled.Value);
            });
        }

        [Test]
        public void TestDisabledWhenSetToTrueNotifiesSubscribers()
        {
            bool? isDisabledA = null;
            bool? isDisabledB = null;
            bool? isDisabledC = null;
            bindableStringDictionary.DisabledChanged += b => isDisabledA = b;
            bindableStringDictionary.DisabledChanged += b => isDisabledB = b;
            bindableStringDictionary.DisabledChanged += b => isDisabledC = b;

            bindableStringDictionary.Disabled = true;

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(isDisabledA);
                Assert.IsTrue(isDisabledA.Value);
                Assert.IsNotNull(isDisabledB);
                Assert.IsTrue(isDisabledB.Value);
                Assert.IsNotNull(isDisabledC);
                Assert.IsTrue(isDisabledC.Value);
            });
        }

        [Test]
        public void TestDisabledWhenSetToCurrentValueDoesNotNotifySubscriber()
        {
            bindableStringDictionary.DisabledChanged += b => Assert.Fail();

            bindableStringDictionary.Disabled = bindableStringDictionary.Disabled;
        }

        [Test]
        public void TestDisabledWhenSetToCurrentValueDoesNotNotifySubscribers()
        {
            bindableStringDictionary.DisabledChanged += b => Assert.Fail();
            bindableStringDictionary.DisabledChanged += b => Assert.Fail();
            bindableStringDictionary.DisabledChanged += b => Assert.Fail();

            bindableStringDictionary.Disabled = bindableStringDictionary.Disabled;
        }

        [Test]
        public void TestDisabledNotifiesBoundLists()
        {
            var list = new BindableDictionary<int, string>();
            list.BindTo(bindableStringDictionary);

            bindableStringDictionary.Disabled = true;

            Assert.IsTrue(list.Disabled);
        }

        #endregion

        #region .GetEnumberator()

        [Test]
        public void TestGetEnumeratorDoesNotReturnNull()
        {
            Assert.NotNull(bindableStringDictionary.GetEnumerator());
        }

        [Test]
        public void TestGetEnumeratorWhenCopyConstructorIsUsedDoesNotReturnTheEnumeratorOfTheInputtedEnumerator()
        {
            string[] array = { "" };
            var list = new BindableDictionary<int, string>(array);

            var enumerator = list.GetEnumerator();

            Assert.AreNotEqual(array.GetEnumerator(), enumerator);
        }

        #endregion

        #region .Description

        [Test]
        public void TestDescriptionWhenSetReturnsSetValue()
        {
            const string description = "The list used for testing.";

            bindableStringDictionary.Description = description;

            Assert.AreEqual(description, bindableStringDictionary.Description);
        }

        #endregion

        #region .Parse(obj)

        [Test]
        public void TestParseWithNullClearsList()
        {
            bindableStringDictionary.Add(0, "a item");

            bindableStringDictionary.Parse(null);

            Assert.IsEmpty(bindableStringDictionary);
        }

        [Test]
        public void TestParseWithArray()
        {
            IEnumerable<string> strings = new[] { "testA", "testB" };

            bindableStringDictionary.Parse(strings);

            CollectionAssert.AreEquivalent(strings, bindableStringDictionary);
        }

        [Test]
        public void TestParseWithDisabledListThrowsInvalidOperationException()
        {
            bindableStringDictionary.Disabled = true;

            Assert.Multiple(() =>
            {
                Assert.Throws(typeof(InvalidOperationException), () => bindableStringDictionary.Parse(null));
                Assert.Throws(typeof(InvalidOperationException), () => bindableStringDictionary.Parse(new object[]
                {
                    "test", "testabc", "asdasdasdasd"
                }));
            });
        }

        [Test]
        public void TestParseWithInvalidArgumentTypesThrowsArgumentException()
        {
            Assert.Multiple(() =>
            {
                Assert.Throws(typeof(ArgumentException), () => bindableStringDictionary.Parse(1));
                Assert.Throws(typeof(ArgumentException), () => bindableStringDictionary.Parse(""));
                Assert.Throws(typeof(ArgumentException), () => bindableStringDictionary.Parse(new object()));
                Assert.Throws(typeof(ArgumentException), () => bindableStringDictionary.Parse(1.1));
                Assert.Throws(typeof(ArgumentException), () => bindableStringDictionary.Parse(1.1f));
                Assert.Throws(typeof(ArgumentException), () => bindableStringDictionary.Parse("test123"));
                Assert.Throws(typeof(ArgumentException), () => bindableStringDictionary.Parse(29387L));
            });
        }

        [Test]
        public void TestParseWithNullNotifiesClearSubscribers()
        {
            string[] strings = { "testA", "testB", "testC" };
            bindableStringDictionary.AddRange(strings);

            var triggeredArgs = new List<NotifyCollectionChangedEventArgs>();
            bindableStringDictionary.CollectionChanged += (_, args) => triggeredArgs.Add(args);

            bindableStringDictionary.Parse(null);

            Assert.That(triggeredArgs, Has.Count.EqualTo(1));
            Assert.That(triggeredArgs.First().Action, Is.EqualTo(NotifyCollectionChangedAction.Remove));
            Assert.That(triggeredArgs.First().OldItems, Is.EquivalentTo(strings));
            Assert.That(triggeredArgs.First().OldStartingIndex, Is.EqualTo(0));
        }

        [Test]
        public void TestParseWithItemsNotifiesAddRangeAndClearSubscribers()
        {
            bindableStringDictionary.Add(0, "test123");
            IEnumerable<string> strings = new[] { "testA", "testB" };

            var triggeredArgs = new List<NotifyCollectionChangedEventArgs>();
            bindableStringDictionary.CollectionChanged += (_, args) => triggeredArgs.Add(args);

            bindableStringDictionary.Parse(strings);

            Assert.That(triggeredArgs, Has.Count.EqualTo(2));
            Assert.That(triggeredArgs.First().Action, Is.EqualTo(NotifyCollectionChangedAction.Remove));
            Assert.That(triggeredArgs.First().OldItems, Is.EquivalentTo("test123".Yield()));
            Assert.That(triggeredArgs.First().OldStartingIndex, Is.EqualTo(0));
            Assert.That(triggeredArgs.ElementAt(1).Action, Is.EqualTo(NotifyCollectionChangedAction.Add));
            Assert.That(triggeredArgs.ElementAt(1).NewItems, Is.EquivalentTo(strings));
            Assert.That(triggeredArgs.ElementAt(1).NewStartingIndex, Is.EqualTo(0));
        }

        #endregion

        #region GetBoundCopy()

        [Test]
        public void TestBoundCopyWithAdd()
        {
            var boundCopy = bindableStringDictionary.GetBoundCopy();

            NotifyCollectionChangedEventArgs triggeredArgs = null;
            boundCopy.CollectionChanged += (_, args) => triggeredArgs = args;

            bindableStringDictionary.Add(0, "test");

            Assert.That(triggeredArgs.Action, Is.EqualTo(NotifyCollectionChangedAction.Add));
            Assert.That(triggeredArgs.NewItems, Is.EquivalentTo("test".Yield()));
            Assert.That(triggeredArgs.NewStartingIndex, Is.EqualTo(0));
        }

        #endregion

        private class TestAlwaysEqualModel : IEquatable<TestAlwaysEqualModel>
        {
            public bool Equals(TestAlwaysEqualModel other) => true;
        }
    }
}
