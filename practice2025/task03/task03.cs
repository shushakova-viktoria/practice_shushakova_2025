﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace task03
{
    public class CustomCollection<T> : IEnumerable<T>
    {
        private readonly List<T> _items = new();

        public void Add(T item) => _items.Add(item);
        public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerable <T> GetReverseEnumerator()
        {
            for (int i = _items.Count - 1; i >= 0; i--)
            {
                yield return _items[i];
            }
        }

        public static IEnumerable <int> GenerateSequence(int start, int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return start + i;
            }
        }

        public IEnumerable <T> FilterAndSort(Func<T, bool> filter, Func<T, IComparable> orderBy)
        {
            return _items
                .Where(filter)
                .OrderBy(orderBy);
        }

    }

}
