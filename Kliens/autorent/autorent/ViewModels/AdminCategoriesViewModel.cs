﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using autorent.Commands;
using autorent.Models;
using autorent.Services;
using autorent.Stores;

namespace autorent.ViewModels
{
    public class AdminCategoriesViewModel : ViewModelBase
    {
        private readonly AccountStore _accountStore;      
        public ICommand DeleteCommand { get;}
        public ICommand CreateCommand { get;}      
        private List<Category> _categories;
        private Category _selectedCategory;
        private string _newCategoryName;
        public string NewCategoryName
        {
            get => _newCategoryName;
            set
            {
                _newCategoryName = value;
                OnPropertyChanged(nameof(NewCategoryName));
            }
        }
        public Category SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged(nameof(SelectedCategory));
            }
        }
        public List<Category>TableData
        {
            get => _categories;
            set
            {
                _categories = value;
                OnPropertyChanged(nameof(TableData));
            }
        }
        private bool _isTextBoxVisible = false;
        public bool IsTextBoxVisible
        {
            get => _isTextBoxVisible;
            set
            {
                _isTextBoxVisible = value;
                OnPropertyChanged(nameof(IsTextBoxVisible));               
            }
        }
        public AdminCategoriesViewModel(AccountStore accountStore)
        {
            _accountStore = accountStore;
            UpdateData();
            DeleteCommand=new AdminCategoriesDeleteCommand(this,_accountStore);
            CreateCommand=new AdminCategoriesCreateCommand(this,_accountStore);
        }
        public void UpdateData()
        {
            try
            {
                TableData = APICommunicationService.GetListOfObject<Category>("/categories", _accountStore.CurrentAccount.Token);
                if (TableData == null)
                    TableData = new List<Category>();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
