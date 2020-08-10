using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Text;
using WebRegex.Core.Models;

namespace WebRegex.UI.Core
{
    public class DataHandling
    {
        public BindableCollection<Profile> ListToBindableCollection(List<Profile> objectList)
        {
            var collection = new BindableCollection<Profile>();
            foreach (Profile obj in objectList)
            {
                collection.Add(obj);
            }
            return collection;
        }
        public BindableCollection<Result> ListToBindableCollection(List<Result> objectList)
        {
            var collection = new BindableCollection<Result>();
            foreach (Result obj in objectList)
            {
                collection.Add(obj);
            }
            return collection;
        }
        public BindableCollection<Expression> ListToBindableCollection(List<Expression> objectList)
        {
            var collection = new BindableCollection<Expression>();
            foreach (Expression obj in objectList)
            {
                collection.Add(obj);
            }
            return collection;
        }
    }
}
