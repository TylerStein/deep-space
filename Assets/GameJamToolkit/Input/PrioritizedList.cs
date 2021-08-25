using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrioritizedListElement<T> where T : class
{
    public readonly int priority = 0;
    public readonly T value;

    public PrioritizedListElement(T value, int priority = 0) {
        this.value = value;
        this.priority = priority;
    }
}

public class PrioritizedList<T> where T : class
{
    [SerializeField] private List<PrioritizedListElement<T>> list;
    [SerializeField] private int count;

    public PrioritizedList() {
        list = new List<PrioritizedListElement<T>>();
        count = 0;
    }

    public T Top() {
        if (count <= 0) return null;
        else return list[count - 1].value;
    }

    public void Add(T item, int priority = 0) {
        list.Add(new PrioritizedListElement<T>(item, priority));
        Sort();
    }

    public void Remove(T item) {
        int index = list.FindIndex((x) => x.value == item);
        if (index != -1) list.RemoveAt(index);
        Sort();
    }

    private void Sort() {
        count = list.Count;
        list.Sort((a, b) => a.priority - b.priority);
    }
}
