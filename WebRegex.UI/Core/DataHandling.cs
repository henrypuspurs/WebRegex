using Caliburn.Micro;
using System.Collections.Generic;

namespace WebRegex.UI.Core
{
    public static class DataHandling
    {
        public static BindableCollection<T> ToBindableCollection<T>(IEnumerable<T> objectList)
        {
            var collection = new BindableCollection<T>();
            foreach (T obj in objectList)
            {
                collection.Add(obj);
            }
            return collection;
        }
    }
}
