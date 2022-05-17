using MiserlyMiser.Models.Entities;
using MiserlyMiser.ViewModels.Base;
using MiserlyMiser.ViewModels.ViewableEntities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiserlyMiser.ViewModels
{
    public class CategoriesPageViewModel : UserDialogViewModel<Category>
    {
        public CategoriesPageViewModel()
        {
            _propertyChangedHandler = new PropertyChangedEventHandler(ItemPropertyChanged);
            _collectionChangedhandler = new NotifyCollectionChangedEventHandler(ItemsCollectionChanged);
            ViewableCategories = new ObservableCollection<ViewableCategory>();
            ViewableCategories.CollectionChanged += _collectionChangedhandler;
            
            for (int i = 0; i < 5; i++)
            {
                ViewableCategories.Add(new ViewableCategory());
                ViewableCategories[i].Category = new Category() { Name = $"first {i}" };
                for (int j = 0; j < 5; j++)
                {
                    ViewableCategories[i].Children.Add(new ViewableCategory());
                    ViewableCategories[i].Children[j].Category = new Category() { Name = $"second {j}" };
                }
            }
            
        }
        protected PropertyChangedEventHandler _propertyChangedHandler;
        protected NotifyCollectionChangedEventHandler _collectionChangedhandler;

        public ObservableCollection<ViewableCategory> ViewableCategories { get; protected set; }

        protected ViewableCategory _selectedItem;
        public ViewableCategory SelectedItem { get => _selectedItem; set => Set(ref _selectedItem, value); }

        public ObservableCollection<Category> Categories
        {
            get
            {
                ObservableCollection<Category> categories = new ObservableCollection<Category>();
                if (SelectedItem == null)
                    return categories;
                else
                {
                    var category = SelectedItem;
                    foreach (var item in category.Children)
                    {
                        categories.Add(item.Category);
                    }
                    return categories;
                }
            }
        }

        protected void SubscribePropertyChanged(ViewableCategory item)
        {
            item.PropertyChanged += _propertyChangedHandler;
            item.Children.CollectionChanged += _collectionChangedhandler;
            foreach (var subitem in item.Children)
            {
                SubscribePropertyChanged(subitem);
            }
        }

        protected void UnsubscribePropertyChanged(ViewableCategory item)
        {
            foreach (var subitem in item.Children)
            {
                UnsubscribePropertyChanged(subitem);
            }
            item.Children.CollectionChanged -= _collectionChangedhandler;
            item.PropertyChanged -= _propertyChangedHandler;
        }

        protected void ItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (ViewableCategory item in e.OldItems)
                {
                    UnsubscribePropertyChanged(item);
                }
            }
            if (e.NewItems != null)
            {
                foreach (ViewableCategory item in e.NewItems)
                {
                    SubscribePropertyChanged(item);
                }
            }
        }

        protected virtual void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsSelected")
            {
                SelectedItem = (ViewableCategory)sender;
            }
        }

        public override void SetProperties()
        {
            
        }
    }   

    internal static class ObservableCollectionTree
    {
        internal static ICollection<T> Traverse<T>(this ICollection<T> collection, Func<T, ICollection<T>> getChildren)
        {
            List<T> allItems = new List<T>();
            foreach (var item in collection)
            {
                allItems.Add(item);
                ICollection<T> children = getChildren(item);
                if (children != null)
                    allItems.AddRange(children.Traverse(getChildren));
            }
            return allItems;
        }

        internal static void AddRange<T>(this ICollection<T> collection, ICollection<T> otherCollection)
        {
            if (collection == null || otherCollection == null)
                return;
            foreach (var item in otherCollection)
            {
                collection.Add(item);
            }
        }

        internal static void Each<T>(this IEnumerable<T> collection, Action<T> toDo)
        {
            foreach (var item in collection)
            {
                toDo(item);
            }
        }
    }
}
