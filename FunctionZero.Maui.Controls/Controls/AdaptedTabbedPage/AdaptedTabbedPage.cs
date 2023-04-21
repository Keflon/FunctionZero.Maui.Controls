using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FunctionZero.Maui.Controls
{
    /// <summary>
    /// An alternative for TabbedPage for people that run into this bug: https://github.com/dotnet/maui/issues/14572
    /// </summary>
    public class AdaptedTabbedPage : TabbedPage
    {
        public AdaptedTabbedPage()
        {
            this.PropertyChanged += AdaptedTabbedPage_PropertyChanged;
        }

        private async void AdaptedTabbedPage_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedItem))
            {
                if (UseExperimentalSelectedItem)
                    if (Children != null)
                        foreach (var page in Children)
                            if (page.BindingContext == SelectedItem)
                            {
                                CurrentPage = page;
                                break;
                            }
            }
            else if (e.PropertyName == nameof(CurrentPage))
            {
                // Setting SelectedItem directly causes a crash on WinUI.
                await Task.Yield();
                if (UseExperimentalSelectedItem)
                    SelectedItem = CurrentPage?.BindingContext;
            }
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            // If the template changes, use it to rebuild the pages.
            if (propertyName == nameof(ItemTemplate))
                RebuildPages();
        }
        private void RebuildPages()
        {
            Children.Clear();

            if (ItemsSource != null)
                foreach (var item in ItemsSource)
                {
                    var page = GetPageForItem(item);
                    if (page != null)
                        Children.Add(page);
                }
        }

        /// <summary>
        /// For a given item (i.e. a ViewModel) use the ItemTemplate to get the Page,
        /// and set the BindingContext.
        /// </summary>
        /// <param name="item">the item to give to the ItemTemplate</param>
        /// <returns>A page associated with the item.</returns>
        private Page GetPageForItem(object item)
        {
            Page retval = null;
            if (ItemTemplate != null)
            {
                if (ItemTemplate is DataTemplateSelector selector)
                    retval = (Page)selector.SelectTemplate(item, this).CreateContent();
                else
                    retval = (Page)ItemTemplate?.CreateContent();

                retval.BindingContext = item;
            }
            return retval;
        }

        /// <summary>
        /// ATTENTION: Hiding base implementation!
        /// </summary>
        public static new readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(AdaptedTabbedPage), null, BindingMode.OneWay, null, ItemsSourcePropertyChanged);

        /// <summary>
        /// ATTENTION: Hiding base implementation!
        /// </summary>
        public new IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        /// <summary>
        /// If the ItemsSource changes ...
        /// 1. Remove any existing Pages.
        /// 2. Detach from the previous ItemsSource if it was INotifyCollectionChanged.
        /// 2. Regenerate new Pages for any new items.
        /// 3. Attach to the previous ItemsSource if it was INotifyCollectionChanged.
        /// </summary>
        private static void ItemsSourcePropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var self = (AdaptedTabbedPage)bindable;

            self.Children.Clear();

            if (oldvalue is INotifyCollectionChanged oldCollection)
                oldCollection.CollectionChanged -= self.ItemsSourceCollectionChanged;

            if (newvalue is IEnumerable newCollection)
            {
                self.RebuildPages();
                if (newvalue is INotifyCollectionChanged newObservable)
                    newObservable.CollectionChanged += self.ItemsSourceCollectionChanged;
            }
        }

        private void ItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    var insertIndex = e.NewStartingIndex;
                    foreach (var newItem in e.NewItems)
                    {
                        var page = GetPageForItem(newItem);
                        Children.Insert(insertIndex++, page);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    for (int c = 0; c < e.OldItems.Count; c++)
                    {
                        Children.RemoveAt(e.OldStartingIndex);
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    throw new NotImplementedException("Handle if necessary!");

                case NotifyCollectionChangedAction.Move:
                    throw new NotImplementedException("Handle if necessary!");

                case NotifyCollectionChangedAction.Reset:
                    Children.Clear();
                    break;
            }
        }


        public static readonly BindableProperty UseExperimentalSelectedItemProperty = BindableProperty.Create(nameof(UseExperimentalSelectedItem), typeof(bool), typeof(AdaptedTabbedPage), true);

        /// <summary>
        /// ATTENTION: Hiding base implementation!
        /// </summary>
        public bool UseExperimentalSelectedItem
        {
            get { return (bool)GetValue(UseExperimentalSelectedItemProperty); }
            set { SetValue(UseExperimentalSelectedItemProperty, value); }
        }
    }
}
