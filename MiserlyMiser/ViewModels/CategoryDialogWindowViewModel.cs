using Microsoft.Extensions.DependencyInjection;
using MiserlyMiser.Infrastructure.Commands;
using MiserlyMiser.Models.Dto;
using MiserlyMiser.Models.Entities;
using MiserlyMiser.Models.Repositories.Interfaces;
using MiserlyMiser.Models.Services;
using MiserlyMiser.Models.Services.Interfaces;
using MiserlyMiser.ViewModels.Base;
using MiserlyMiser.Views.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MiserlyMiser.ViewModels
{
    public class CategoryDialogWindowViewModel : UserDialogViewModel<Category>
    {
        private ICrudRepository<CategoryCharacter> _categoryCharacterRepository;
        private ICategoryRepository _categoryRepository;

        public CategoryDialogWindowViewModel(ICrudRepository<CategoryCharacter> categoryCharacterRepository, ICategoryRepository categoryRepository)
        {
            _categoryCharacterRepository = categoryCharacterRepository;
            _categoryRepository = categoryRepository;
            InitCollections();

            SelectParentCategoryCommand = new LambdaCommand(OnSelectParentCategoryCommandExecuted,
                CanSelectParentCategoryCommandExecute);
            SelectChildCategoriesCommand = new LambdaCommand(OnSelectChildCategoriesCommandExecuted,
                CanSelectChildCategoriesCommandExecute);
            ClearChildCategoriesCommand = new LambdaCommand(OnClearChildCategoriesCommandExecuted,
                CanClearChildCategoriesCommandExecute);
            ClearParentCategoryCommand = new LambdaCommand(OnClearParentCategoryCommandExecuted,
                CanClearParentCategoryCommandExecute);
            CancelCommand = new LambdaCommand(OnCancelCommandExecuted, CanCancelCommandExecute);
            ApplyCommand = new LambdaCommand(OnApplyCommandExecuted, CanApplyCommandExecute);
        }

        private string _name = "";
        public string Name { get => _name; set => Set(ref _name, value); }

        private bool _isEnabled = true;
        public bool IsEnabled { get => _isEnabled; set => Set(ref _isEnabled, value);}

        private CategoryCharacter _selectedCharacter;
        public CategoryCharacter SelectedCharacter { get => _selectedCharacter; set => Set(ref _selectedCharacter, value); }
        public ObservableCollection<CategoryCharacter> CategoryCharacters { get; private set; }

        private Category _parentCategory;
        public Category ParentCategory
        { 
            get => _parentCategory;
            set
            {
                Set(ref _parentCategory, value);
                if (_parentCategory == null)
                {
                    IsEnabled = true;
                    SelectedCharacter = null;
                }                   
                else
                {
                    IsEnabled = false;
                    SelectedCharacter = _parentCategory.CategoryCharacter;
                }                   
            }
        }

        public ObservableCollection<Category> ChildCategories { get; private set; } = new ObservableCollection<Category>();

        #region SelectParentCategoryCommand
        public ICommand SelectParentCategoryCommand { get; }
        private void OnSelectParentCategoryCommandExecuted(object p)
        {
            try
            {
                Status = "";
                IUserDialog<Category> userDialog = App.Services.GetRequiredService<DefaultUserDialog<SelectParentCategoryWindowViewModel, SelectParentCategoryWindow, Category>>();
                EntityDto<Category> dto = new EntityDto<Category>("", null, _categoryRepository);
                if (userDialog.Show(dto))
                {
                    ParentCategory = dto.Entity;
                }
            }
            catch (Exception ex)
            {
                Status = ex.Message;
            }
        }
        private bool CanSelectParentCategoryCommandExecute(object p)
        {
            return true;
        }
        #endregion

        #region SelectChildCategoriesCommand
        public ICommand SelectChildCategoriesCommand { get; }
        private void OnSelectChildCategoriesCommandExecuted(object p)
        {
            try
            {
                Status = "";
                IUserDialog<Category> userDialog = App.Services.GetRequiredService<DefaultUserDialog<SelectChildrenCategoryWindowViewModel, SelectChildrenCategoriesWindow, Category>>();
                EntityDto<Category> dto = new EntityDto<Category>("", new Category(), _categoryRepository);
                if (userDialog.Show(dto))
                {                   
                    if (dto.Entity.ChildCategories != null)
                    {
                        ChildCategories.Clear();
                        foreach (var item in dto.Entity.ChildCategories)
                        {
                            ChildCategories.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Status = ex.Message;
            }
        }
        private bool CanSelectChildCategoriesCommandExecute(object p)
        {
            return true;
        }
        #endregion

        #region ClearChildCategoriesCommand
        public ICommand ClearChildCategoriesCommand { get; }
        private void OnClearChildCategoriesCommandExecuted(object p)
        {
            try
            {
                Status = "";
                ChildCategories.Clear();
            }
            catch (Exception ex)
            {
                Status = ex.Message;
            }
        }
        private bool CanClearChildCategoriesCommandExecute(object p)
        {
            return ChildCategories != null && ChildCategories.Count > 0;
        }
        #endregion

        #region ClearParentCategoryCommand
        public ICommand ClearParentCategoryCommand { get; }
        private void OnClearParentCategoryCommandExecuted(object p)
        {
            try
            {
                Status = "";
                ParentCategory = null;
            }
            catch (Exception ex)
            {
                Status = ex.Message;
            }
        }
        private bool CanClearParentCategoryCommandExecute(object p)
        {
            return ParentCategory != null;
        }
        #endregion

        #region CancelCommand
        public ICommand CancelCommand { get; }
        private void OnCancelCommandExecuted(object p)
        {
            OnCloseWindow(new ClosableViewModelEventArgs(false));
        }
        private bool CanCancelCommandExecute(object p)
        {
            return true;
        }
        #endregion

        #region ApplyCommand
        public ICommand ApplyCommand { get; }
        private void OnApplyCommandExecuted(object p)
        {
            try
            {
                Category category;
                if (_isCreate)
                    category = new Category();
                else
                    category = Dto.Entity;
                category.Name = Name;
                category.CategoryCharacter = SelectedCharacter;
                category.Parent = ParentCategory;
                category.ChildCategories = ChildCategories;
                if (_isCreate)
                    _categoryRepository.Create(category);
                else
                    _categoryRepository.Update(category);
                OnCloseWindow(new ClosableViewModelEventArgs(true));
            }
            catch(Exception ex)
            {
                Status = ex.Message;
            }            
        }
        private bool CanApplyCommandExecute(object p)
        {
            return !string.IsNullOrWhiteSpace(Name) && SelectedCharacter != null;
        }
        #endregion

        public override void SetProperties()
        {
            if (Dto == null)
                return;
            if (Dto.Entity == null)
                return;
            Name = Dto.Entity.Name;
            SelectedCharacter = Dto.Entity.CategoryCharacter;
            ParentCategory = Dto.Entity.Parent;
            if (Dto.Entity.ChildCategories != null)
            {
                foreach (var item in Dto.Entity.ChildCategories)
                {
                    ChildCategories.Add(item);
                }
            }
        }

        private void InitCollections()
        {
            var characters = _categoryCharacterRepository.GetAll();
            if (characters != null && characters.Count > 0)
                CategoryCharacters = new ObservableCollection<CategoryCharacter>(characters);
            else
                CategoryCharacters = new ObservableCollection<CategoryCharacter>();
        }
    }
}
