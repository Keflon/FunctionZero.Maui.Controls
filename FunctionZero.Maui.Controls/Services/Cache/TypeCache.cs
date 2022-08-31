using FunctionZero.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionZero.Maui.Services.Cache
{
    public class BucketDictionary<TKey, TContent>
    {
        //private Dictionary<DataTemplate, Stack<ListItemZero>> _cache;
        private readonly Dictionary<TKey, List<TContent>> _cache;
        public BucketDictionary(Action<TKey, TContent> cacheAction = null, Func<TKey, TContent, object, bool> cacheRetrievePredicate = null)
        {
            _cache = new();
            CacheAction = cacheAction ?? ((a, b) => { });
            CacheRetrievePredicate = cacheRetrievePredicate ?? ((a, b, c) => true);
        }

        public Action<TKey, TContent> CacheAction { get; }
        public Func<TKey, TContent, object, bool> CacheRetrievePredicate { get; }


        public void PushToBucket(TKey template, TContent item)
        {
            CacheAction(template, item);

            if (_cache.TryGetValue(template, out var list))
            {
                list.Add(item);
            }
            else
            {
                var newList = new List<TContent>
                {
                    item
                };
                _cache.Add(template, newList);
            }
        }

        //internal bool OldTryPopFromBucket(TKey template, out TContent content)
        //{
        //    TContent contentValue = default;

        //    if (_cache.TryGetValue(template, out var typeStack))
        //        if (typeStack.TryPop(out var view))
        //            contentValue = view;

        //    content = contentValue;
        //    return contentValue != null;
        //}

        internal bool TryPopFromBucket(TKey template, out TContent content, object data = null)
        {
            TContent contentValue = default;

            if (_cache.TryGetValue(template, out var typeList))
            { 
                foreach (var item in typeList)
                {
                    if (CacheRetrievePredicate(template, item, data))
                    {
                        contentValue = item;

                        typeList.Remove(item);
                        break;
                    }
                }
            }
            content = contentValue;
            return contentValue != null;
        }
    }
}
