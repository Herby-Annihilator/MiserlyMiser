using Microsoft.Extensions.DependencyInjection;
using MiserlyMiser.Infrastructure.Commands;
using MiserlyMiser.Models.Dto;
using MiserlyMiser.Models.Entities;
using MiserlyMiser.Models.Repositories.Interfaces;
using MiserlyMiser.Models.Services;
using MiserlyMiser.Models.Services.Interfaces;
using MiserlyMiser.ViewModels.Base;
using MiserlyMiser.ViewModels.ViewableEntities;
using MiserlyMiser.Views.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MiserlyMiser.ViewModels
{
    public class CategoriesPageViewModel : UserDialogViewModel<Category>
    {
        protected ICategoryRepository _categoryRepository;
        public CategoriesPageViewModel(ICategoryRepository categoryRepository) : this()
        {
            _categoryRepository = categoryRepository;
            BuiltViewableCategoriesCollection();
            UpdateCategories();
        }
        public CategoriesPageViewModel()
        {
            _propertyChangedHandler = new PropertyChangedEventHandler(ItemPropertyChanged);
            _collectionChangedhandler = new NotifyCollectionChangedEventHandler(ItemsCollectionChanged);
            ViewableCategories = new ObservableCollection<ViewableCategory>();
            ViewableCategories.CollectionChanged += _collectionChangedhandler;

            CreateCategoryCommand = new LambdaCommand(OnCreateCategoryCommandExecuted, CanCreateCategoryCommandExecute);
            EditCategoryCommand = new LambdaCommand(OnEditCategoryCommandExecuted, CanEditCategoryCommandExecute);
            DeleteCategoryCommand = new LambdaCommand(OnDeleteCategoryCommandExecuted, CanDeleteCategoryCommandExecute);         
        }
        protected PropertyChangedEventHandler _propertyChangedHandler;
        protected NotifyCollectionChangedEventHandler _collectionChangedhandler;

        public ObservableCollection<ViewableCategory> ViewableCategories { get; protected set; }

        protected ViewableCategory _selectedItem;
        public ViewableCategory SelectedItem 
        { 
            get => _selectedItem;
            set
            {
                Set(ref _selectedItem, value);
                UpdateCategories();
            }
        }

        public ObservableCollection<Category> Categories { get; protected set; } = new ObservableCollection<Category>();

        #region Commands

        #region CreateCategoryCommand
        public ICommand CreateCategoryCommand { get; }
        private void OnCreateCategoryCommandExecuted(object p)
        {
            try
            {
                Status = "";
                IUserDialog<Category> userDialog = App.Services.GetRequiredService<DefaultUserDialog<CategoryDialogWindowViewModel, CategoryWindowDialog, Category>>();
                EntityDto<Category> dto = new EntityDto<Category>("Создать категорию", null, _categoryRepository);
                if (userDialog.Show(dto))
                {
                    ViewableCategories.Clear();
                    BuiltViewableCategoriesCollection();
                    UpdateCategories();
                }
            }
            catch (Exception ex)
            {
                Status = ex.Message;
            }
        }
        private bool CanCreateCategoryCommandExecute(object p) => true;
        #endregion

        #region EditCategoryCommand
        public ICommand EditCategoryCommand { get; }
        private void OnEditCategoryCommandExecuted(object p)
        {
            try
            {
                Status = "";
                IUserDialog<Category> userDialog = App.Services.GetRequiredService<DefaultUserDialog<CategoryDialogWindowViewModel, CategoryWindowDialog, Category>>();
                EntityDto<Category> dto = new EntityDto<Category>("Редактировать категорию", SelectedItem.Category, _categoryRepository);
                if (userDialog.Show(dto))
                {
                    ViewableCategories.Clear();
                    BuiltViewableCategoriesCollection();
                    UpdateCategories();
                }
            }
            catch (Exception ex)
            {
                Status = ex.Message;
            }
        }
        private bool CanEditCategoryCommandExecute(object p) => SelectedItem != null;
        #endregion

        #region DeleteCategoryCommand
        public ICommand DeleteCategoryCommand { get; }
        private void OnDeleteCategoryCommandExecuted(object p)
        {
            try
            {
                Status = "";
                _categoryRepository.Delete(SelectedItem.Category);
                ViewableCategories.Clear();
                BuiltViewableCategoriesCollection();
                UpdateCategories();
            }
            catch (Exception ex)
            {
                Status = ex.Message;
            }
        }
        private bool CanDeleteCategoryCommandExecute(object p) => SelectedItem != null;
        #endregion

        #endregion

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

        protected void UpdateCategories()
        {
            Categories.Clear();
            if (SelectedItem == null)
            {
                foreach (var item in ViewableCategories)
                {
                    if (item.Parent == null)
                        Categories.Add(item.Category);
                }
                return;
            }               
            else
            {
                var category = SelectedItem;
                foreach (var item in category.Children)
                {
                    Categories.Add(item.Category);
                }
                return;
            }
        }

        public override void SetProperties()
        {
            
        }

        protected virtual void BuiltViewableCategoriesCollection()
        {
            if (_categoryRepository == null)
                _categoryRepository = App.Services.GetRequiredService<ICategoryRepository>();
            ICollection<Category> categories = _categoryRepository.GetAll();
            ViewableCategory viewableCategory;
            if (ViewableCategories == null)
            {
                ViewableCategories = new ObservableCollection<ViewableCategory>();
                ViewableCategories.CollectionChanged += _collectionChangedhandler;
            }
            foreach (var item in categories)
            {
                viewableCategory = new ViewableCategory() { Category = item };
                if (item.Parent == null)
                {
                    ViewableCategories.Add(viewableCategory);
                    AddChildren(viewableCategory, categories);
                }
            }
        }

        protected static void AddChildren(ViewableCategory viewableCategory, ICollection<Category> categories)
        {
            if (categories == null)
                return;
            if (viewableCategory == null)
                return;
            if (viewableCategory.Category == null)
                return;
            ViewableCategory child;
            foreach (Category category in categories)
            {
                if (category.Parent?.Id == viewableCategory.Category.Id)
                {
                    child = new ViewableCategory() { Category = category };
                    child.Parent = viewableCategory;
                    viewableCategory.Children.Add(child);
                    AddChildren(child, categories);
                }
            }
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
