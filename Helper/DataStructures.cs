using System;
using UnityEngine;
    

[Serializable]
public struct DictionaryNode<A, B>
{
    public A firstElement;
    public B secondElement;

    public DictionaryNode(A firstElement, B secondElement)
    {
        this.firstElement = firstElement;
        this.secondElement = secondElement;
    }
}

[Serializable]
public struct TrictionaryNode<A, B, C>
{
    public A firstElement;
    public B secondElement;
    public C thirdElement;

    public TrictionaryNode(A firstElement, B secondElement, C thirdElement)
    {
        this.firstElement = firstElement;
        this.secondElement = secondElement;
        this.thirdElement = thirdElement;
    }
}

[Serializable]
public class DoubleLinkedListNode<T>
{
    public T data;
    public DoubleLinkedListNode<T> nextNode;
    public DoubleLinkedListNode<T> previousNode;

    #region Constructors
    public DoubleLinkedListNode() => data = default(T);
    public DoubleLinkedListNode(T data) => this.data = data;
    public DoubleLinkedListNode(DoubleLinkedListNode<T> previousNode, DoubleLinkedListNode<T> nextNode,T data=default(T))
    {
        this.previousNode = previousNode;
        this.nextNode = nextNode;
        this.data = data;
    }
    #endregion
}