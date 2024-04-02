using System.Collections;

namespace Editor.Core.Collections;

public class OrderedCollection<T> : ICollection<T>
{
    private readonly SortedDictionary<int, List<T>> _items = new();
    private readonly Func<T, int> _keyGetter;

    public OrderedCollection(Func<T, int> keyGetter)
    {
        _keyGetter = keyGetter;
    }
    
    public IEnumerator<T> GetEnumerator()
    {
        Sort();

        foreach (var item in _items.Values.SelectMany(renderers => renderers))
        {
            yield return item;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(T item)
    {
        var key = _keyGetter(item);

        if (!_items.TryGetValue(key, out var list))
        {
            list = new List<T>();
            _items[key] = list;
        }

        list.Add(item);
    }

    public void Clear()
    {
        _items.Clear();
    }

    public bool Contains(T item)
    {
        return _items.Values.Any(x => x.Contains(item));
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        foreach (var item in this)
        {
            array[arrayIndex++] = item;
        }
    }

    public bool Remove(T item)
    {
        return _items[_keyGetter(item)].Remove(item);
    }
    
    private void Sort()
    {
        var itemsToMove = new List<T>();

        foreach (var (weight, items) in _items)
        {
            var i = 0;
            while (i < items.Count)
            {
                if (_keyGetter(items[i]) == weight)
                {
                    i++;
                }
                else
                {
                    itemsToMove.Add(items[i]);
                    items.RemoveAt(i);
                }
            }
        }
        
        foreach (var renderer in itemsToMove)
        {
            Add(renderer);
        }
    }

    public int Count => _items.Sum(x => x.Value.Count);
    public bool IsReadOnly => false;
}