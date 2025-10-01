using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System.Collections.ObjectModel;


namespace Grocery.App.ViewModels
{
    public partial class BoughtProductsViewModel : BaseViewModel
    {
        private readonly IBoughtProductsService _boughtProductsService;

        [ObservableProperty]
        Product selectedProduct;
        
        public ObservableCollection<BoughtProducts> BoughtProductsList { get; set; } = [];
        public ObservableCollection<Product> Products { get; set; }

        public BoughtProductsViewModel(IBoughtProductsService boughtProductsService, IProductService productService)
        {
            _boughtProductsService = boughtProductsService;
            Products = new(productService.GetAll());
        }
        

        /// <summary>
        /// Wordt automatisch aangeroepen wanneer een nieuw product geselecteerd wordt in de Picker.
        /// Haalt alle klanten en boodschappenlijsten op die dit product gekocht hebben.
        /// </summary>
        /// <param name="oldValue">Het vorige geselecteerde product</param>
        /// <param name="newValue">Het nieuw geselecteerde product</param>
        partial void OnSelectedProductChanged(Product? oldValue, Product newValue)
        {
            if (newValue != null)
            {
                System.Diagnostics.Debug.WriteLine($"Product geselecteerd: {newValue.Name} (Id: {newValue.Id})");
                
                BoughtProductsList.Clear();
                var boughtProducts = _boughtProductsService.Get(newValue.Id);
                
                System.Diagnostics.Debug.WriteLine($"Aantal BoughtProducts ontvangen: {boughtProducts.Count}");
                
                foreach (var item in boughtProducts)
                {
                    System.Diagnostics.Debug.WriteLine($"Toevoegen aan lijst: {item.Client.Name} - {item.GroceryList.Name}");
                    BoughtProductsList.Add(item);
                }
                
                System.Diagnostics.Debug.WriteLine($"BoughtProductsList Count: {BoughtProductsList.Count}");
            }
        }

        /// <summary>
        /// Command om een nieuw product te selecteren vanuit de code-behind van de View.
        /// Update de SelectedProduct property, wat vervolgens OnSelectedProductChanged triggert.
        /// </summary>
        /// <param name="product">Het product dat geselecteerd moet worden</param>
        [RelayCommand]
        public void NewSelectedProduct(Product product)
        {
            SelectedProduct = product;
        }
    }
}