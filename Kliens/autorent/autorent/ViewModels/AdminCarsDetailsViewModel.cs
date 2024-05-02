using autorent.Commands;
using autorent.Models;
using autorent.Services;
using autorent.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace autorent.ViewModels
{
    public class AdminCarsDetailsViewModel : ViewModelBase
    {
        private readonly SelectedCarStore _selectedCarStore;
        private readonly AccountStore _accountStore;
        private readonly WebsocketDataUpdateService _websocketDataUpdateService;

        public Car SelectedCar;
        public Car EditedCar;

        public ICommand DeleteCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand SaveEditCommand { get; }
        public ICommand CreateCommand { get; }

        public bool HasSelectedCar => SelectedCar != null;
        public string Brand => SelectedCar?.Brand ?? "";
        public string Model => SelectedCar?.Model ?? "";
        public string Category => SelectedCar?.Category ?? "";
        public int DailyPrice => SelectedCar?.DailyPrice ?? 0;

        private ObservableCollection<Category> _categoryList;
        public IEnumerable<Category> CategoryList => _categoryList;

        private Category _selectedCategory;
        public Category SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged(nameof(SelectedCategory));
            }
        }


        private bool _isEditEnabled = false;
        public bool IsEditEnabled
        {
            get => _isEditEnabled;
            set
            {
                _isEditEnabled = value;
                OnPropertyChanged(nameof(IsEditEnabled));
                OnPropertyChanged(nameof(IsEditButtonEnabled));
            }
        }
        public bool IsEditButtonEnabled => !IsEditEnabled;
        public string EBrand
        {
            get => EditedCar?.Brand ?? "";
            set
            {
                if (EditedCar == null) return;
                EditedCar.Brand = value;
            }
        }
        public string EModel
        {
            get => EditedCar?.Model ?? "";
            set
            {
                if (EditedCar == null) return;
                EditedCar.Model = value;
            }
        }
        public string ECategory
        {
            get => EditedCar?.Category ?? "";
            set
            {
                if (EditedCar == null) return;
                EditedCar.Category = value;
            }
        }
        public int EDailyPrice
        {
            get => EditedCar?.DailyPrice ?? 0;
            set
            {
                if (EditedCar == null) return;
                EditedCar.DailyPrice = value;
            }
        }

        public AdminCarsDetailsViewModel(SelectedCarStore selectedCarStore, AccountStore accountStore, WebsocketDataUpdateService websocketDataUpdateService)
        {
            _categoryList = new ObservableCollection<Category>();

            _selectedCarStore = selectedCarStore;
            _accountStore = accountStore;
            _websocketDataUpdateService = websocketDataUpdateService;
            _websocketDataUpdateService.OnCategoryCreated += _websocketDataUpdateService_OnCategoryCreated;
            _websocketDataUpdateService.OnCategoryDeleted += _websocketDataUpdateService_OnCategoryDeleted;

            updateCategories();
            updateSelectedCar();

            DeleteCommand = new AdminCarsDeleteCommand(this, _selectedCarStore, _accountStore);
            CancelCommand = new AdminCarsCancelCommand(this, _selectedCarStore);
            EditCommand = new AdminCarsEditCommand(this);
            SaveEditCommand = new AdminCarsSaveCommand(this, _selectedCarStore, _accountStore);
            CreateCommand = new AdminCarsCreateCommand(_selectedCarStore);
            //((RentCommand)RentCommand).RentalSuccessful += OnRentalSuccessful;

            _selectedCarStore.SelectedCarChanged += OnSelectedCarChanged;
        }
        public void OnSelectedCarChanged()
        {
            if (_selectedCarStore.SelectedCar?.Id != -1)
                IsEditEnabled = false;

            updateSelectedCar();

            OnPropertyChanged(nameof(HasSelectedCar));
            OnPropertyChanged(nameof(Brand));
            OnPropertyChanged(nameof(Model));
            OnPropertyChanged(nameof(Category));
            OnPropertyChanged(nameof(DailyPrice));

            OnPropertyChanged(nameof(EBrand));
            OnPropertyChanged(nameof(EModel));
            OnPropertyChanged(nameof(ECategory));
            OnPropertyChanged(nameof(EDailyPrice));
        }
        private void updateSelectedCar()
        {
            if (_selectedCarStore.SelectedCar == null)
            {
                SelectedCar = null;
                EditedCar = null;
            }
            else if(_selectedCarStore.SelectedCar.Id == -1)
            {
                SelectedCar = _selectedCarStore.SelectedCar;
                EditedCar = _selectedCarStore.SelectedCar;
                IsEditEnabled = true;
            }
            else
            {
                SelectedCar = APICommunicationService.GetObject<Car>($"/cars/{_selectedCarStore.SelectedCar.Id}", _accountStore.CurrentAccount.Token);
                EditedCar = new Car(SelectedCar.Id, SelectedCar.Category, SelectedCar.Brand, SelectedCar.Model, SelectedCar.DailyPrice, new List<string>());

                Category cat = null;
                IEnumerable<Category> scat = CategoryList.Where(x => x.Name == EditedCar.Category);
                if (scat.Count() > 0)
                    cat = scat.First();


                SelectedCategory = cat;
            }
            OnPropertyChanged(nameof(SelectedCar));
            OnPropertyChanged(nameof(EditedCar));
        }
        private void updateCategories()
        {
            try
            {
                List<Category> cat = APICommunicationService.GetListOfObject<Category>("/categories", _accountStore.CurrentAccount.Token);
                if (cat == null)
                    cat = new List<Category>();

                foreach (Category category in cat)
                {
                    _categoryList.Add(category);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void _websocketDataUpdateService_OnCategoryCreated(Category category)
        {
            try
            {
                _categoryList = new ObservableCollection<Category>(CategoryList.Append(category));
                OnPropertyChanged(nameof(CategoryList));
            }
            catch { }
        }
        private void _websocketDataUpdateService_OnCategoryDeleted(int idOfDeletedItem)
        {
            try
            {
                Category deleteThis = CategoryList.Where(x => x.Id == idOfDeletedItem)?.First();
                if (deleteThis != null)
                {
                    _categoryList = new ObservableCollection<Category>(CategoryList.Except(new ObservableCollection<Category>() { deleteThis }));
                }
                OnPropertyChanged(nameof(CategoryList));
            }
            catch { }
        }


        public override void Dispose()
        {
            _selectedCarStore.SelectedCarChanged -= OnSelectedCarChanged;
            _selectedCarStore.SelectedCar = null;
            _websocketDataUpdateService.OnCategoryCreated -= _websocketDataUpdateService_OnCategoryCreated;
            _websocketDataUpdateService.OnCategoryDeleted -= _websocketDataUpdateService_OnCategoryDeleted;

            base.Dispose();
        }
    }
}
