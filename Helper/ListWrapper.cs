using System.Collections.Generic;
public class ListWrapper<T>
{
    public List<T> list;

    public T this[int key]
    {
        get => list[key];
        set => list[key] = value;
    }
}
