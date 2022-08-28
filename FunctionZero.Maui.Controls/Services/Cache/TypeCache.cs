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
        private Dictionary<TKey, Stack<TContent>> _cache;
        public BucketDictionary()
        {

        }
    }
}
