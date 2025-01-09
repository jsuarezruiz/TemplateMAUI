using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace TemplateMAUI.Controls
{
    /// <summary>
    /// The StepBarItems class represents a collection of StepBarItem objects within a step bar control. 
    /// It inherits from the Element class and implements the IList<StepBarItem> and INotifyCollectionChanged interfaces to provide functionality for managing the collection 
    /// of steps and notifying when the collection changes.
    /// </summary>
    public class StepBarItems : Element, IList<StepBarItem>, INotifyCollectionChanged
    {
        readonly ObservableCollection<StepBarItem> _stepBarItems;

        public StepBarItems(IEnumerable<StepBarItem> stepBarItems)
        {
            _stepBarItems = new ObservableCollection<StepBarItem>(stepBarItems) ?? throw new ArgumentNullException(nameof(stepBarItems));
            _stepBarItems.CollectionChanged += OnStepBarItemsChanged;
        }

        public StepBarItems() : this(Enumerable.Empty<StepBarItem>())
        {

        }

        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add { _stepBarItems.CollectionChanged += value; }
            remove { _stepBarItems.CollectionChanged -= value; }
        }

        public StepBarItem this[int index]
        {
            get => _stepBarItems.Count > index ? _stepBarItems[index] : null;
            set => _stepBarItems[index] = value;
        }

        public int Count => _stepBarItems.Count;

        public bool IsReadOnly => false;

        public void Add(StepBarItem item)
        {
            _stepBarItems.Add(item);
        }

        public void Clear()
        {
            _stepBarItems.Clear();
        }

        public bool Contains(StepBarItem item)
        {
            return _stepBarItems.Contains(item);
        }

        public void CopyTo(StepBarItem[] array, int arrayIndex)
        {
            _stepBarItems.CopyTo(array, arrayIndex);
        }

        public IEnumerator<StepBarItem> GetEnumerator()
        {
            return _stepBarItems.GetEnumerator();
        }

        public int IndexOf(StepBarItem item)
        {
            return _stepBarItems.IndexOf(item);
        }

        public void Insert(int index, StepBarItem item)
        {
            _stepBarItems.Insert(index, item);
        }

        public bool Remove(StepBarItem item)
        {
            return _stepBarItems.Remove(item);
        }

        public void RemoveAt(int index)
        {
            _stepBarItems.RemoveAt(index);
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            object bc = BindingContext;

            foreach (BindableObject item in _stepBarItems)
                SetInheritedBindingContext(item, bc);
        }

        void OnStepBarItemsChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            if (notifyCollectionChangedEventArgs.NewItems == null)
                return;

            object bc = BindingContext;

            foreach (BindableObject item in notifyCollectionChangedEventArgs.NewItems)
                SetInheritedBindingContext(item, bc);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _stepBarItems.GetEnumerator();
        }
    }
}