using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.Core.Services
{
    public class BoughtProductsService : IBoughtProductsService
    {
        private readonly IGroceryListItemsRepository _groceryListItemsRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IProductRepository _productRepository;
        private readonly IGroceryListRepository _groceryListRepository;
        
        public BoughtProductsService(IGroceryListItemsRepository groceryListItemsRepository, IGroceryListRepository groceryListRepository, IClientRepository clientRepository, IProductRepository productRepository)
        {
            _groceryListItemsRepository=groceryListItemsRepository;
            _groceryListRepository=groceryListRepository;
            _clientRepository=clientRepository;
            _productRepository=productRepository;
        }
        
         /// <summary>
        /// Haalt alle gekochte producten op voor een specifiek product.
        /// Zoekt in alle boodschappenlijsten naar items met het gegeven productId
        /// en combineert deze met klant- en boodschappenlijst-informatie.
        /// </summary>
        /// <param name="productId">Het ID van het product waarvoor gekochte producten gezocht worden</param>
        /// <returns>Een lijst van BoughtProducts met klant, boodschappenlijst en product informatie</returns>
        public List<BoughtProducts> Get(int? productId)
        {
            List<BoughtProducts> boughtProductsList = new();
            
            System.Diagnostics.Debug.WriteLine($"BoughtProductsService.Get aangeroepen met productId: {productId}");
            
            if (productId == null)
                return boughtProductsList;
            
            var allItems = _groceryListItemsRepository.GetAll();
            System.Diagnostics.Debug.WriteLine($"Totaal aantal GroceryListItems: {allItems.Count}");
            
            var groceryListItems = allItems.Where(gli => gli.ProductId == productId).ToList();
            System.Diagnostics.Debug.WriteLine($"Items met ProductId {productId}: {groceryListItems.Count}");
            
            foreach (var item in groceryListItems)
            {
                var groceryList = _groceryListRepository.Get(item.GroceryListId);
                if (groceryList != null)
                {
                    var client = _clientRepository.Get(groceryList.ClientId);
                    var product = _productRepository.Get(item.ProductId);
                    
                    if (client != null && product != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"Gevonden: Client={client.Name}, GroceryList={groceryList.Name}, Product={product.Name}");
                        boughtProductsList.Add(new BoughtProducts(client, groceryList, product));
                    }
                }
            }
            
            System.Diagnostics.Debug.WriteLine($"Totaal BoughtProducts: {boughtProductsList.Count}");
            return boughtProductsList;
        }
    }
}