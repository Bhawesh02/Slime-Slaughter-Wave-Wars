
using System.Collections.Generic;

public abstract class PoolService<T>
{

    private class PooledItem<t>
    {
        public t Item;
        public bool IsUsed;
    }
    private List<PooledItem<T>> pooledItems = new();

    public virtual T GetItem()
    {
        if (pooledItems.Count > 0)
        {
            PooledItem<T> item = pooledItems.Find(x => !x.IsUsed);
            if (item != null)
            {
                item.IsUsed = true;
                return item.Item;
            }
        }
        return CreateNewItem();
    }

    protected T CreateNewItem()
    {
        PooledItem<T> item = new()
        {
            Item = CreateItem(),
            IsUsed = true
        };
        pooledItems.Add(item);
        return item.Item;
    }

    protected abstract T CreateItem();

    public virtual void ReturnItem(T item)
    {
        PooledItem<T> usedItem = pooledItems.Find(x => x.Item.Equals(item));
        usedItem.IsUsed = false;
    }

}
