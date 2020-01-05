using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TruSport.Model
{
    public class FixtureGroup : ObservableCollection<FixtureListView>
    {
        public string Name { get; private set; }

        public FixtureGroup(string name)
            : base()
        {
            Name = name;
        }

        public FixtureGroup(string name, IEnumerable<FixtureListView> source)
            : base(source)
        {
            Name = name;
        }
    }

    public class Grouping<K, T> : ObservableCollection<T>
    {
        public K Key { get; private set; }

        public Grouping(K key, IEnumerable<T> items)
        {
            Key = key;
            foreach (var item in items)
                this.Items.Add(item);
        }
    }
}
