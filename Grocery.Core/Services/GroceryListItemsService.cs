using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.Core.Services
{
    public class GroceryListItemsService : IGroceryListItemsService
    {
        private readonly IGroceryListItemsRepository _groceriesRepository;
        private readonly IProductRepository _productRepository;

        public GroceryListItemsService(IGroceryListItemsRepository groceriesRepository, IProductRepository productRepository)
        {
            _groceriesRepository = groceriesRepository;
            _productRepository = productRepository;
        }

        public List<GroceryListItem> GetAll()
        {
            List<GroceryListItem> groceryListItems = _groceriesRepository.GetAll();
            FillService(groceryListItems);
            return groceryListItems;
        }

        public List<GroceryListItem> GetAllOnGroceryListId(int groceryListId)
        {
            List<GroceryListItem> groceryListItems = _groceriesRepository.GetAll().Where(g => g.GroceryListId == groceryListId).ToList();
            FillService(groceryListItems);
            return groceryListItems;
        }

        public GroceryListItem Add(GroceryListItem item)
        {
            return _groceriesRepository.Add(item);
        }

        public GroceryListItem? Delete(GroceryListItem item)
        {
            throw new NotImplementedException();
        }

        public GroceryListItem? Get(int id)
        {
            return _groceriesRepository.Get(id);
        }

        public GroceryListItem? Update(GroceryListItem item)
        {
            return _groceriesRepository.Update(item);
        }

        /// <summary>
        /// Haalt de best verkopende producten op gebaseerd op het aantal keren dat ze in boodschappenlijsten voorkomen.
        /// Groepeert alle grocery list items per product, telt het aantal verkopen, en sorteert op populariteit.
        /// </summary>
        /// <param name="topX">Het aantal top producten dat terug gegeven moet worden (standaard 5)</param>
        /// <returns>Een lijst van BestSellingProducts gesorteerd op aantal verkopen met ranking</returns>
        public List<BestSellingProducts> GetBestSellingProducts(int topX = 5)
        {
            var allItems = _groceriesRepository.GetAll();
            var productIds = allItems.Select(i => i.ProductId).Distinct().ToList();
            var allProducts = _productRepository.GetByIds(productIds)
                .ToDictionary(p => p.Id);

            var grouped = allItems
                .GroupBy(i => i.ProductId)
                .Select(g =>
                {
                    allProducts.TryGetValue(g.Key, out var product);
                    return new
                    {
                        ProductId = g.Key,
                        ProductName = product?.Name ?? "Onbekend product",
                        Stock = product?.Stock ?? 0,
                        TotalAmountSold = g.Sum(item => item.Amount) 
                    };
                })
                .OrderByDescending(x => x.TotalAmountSold)
                .Take(topX)
                .ToList();

            return grouped
                .Select((x, index) => new BestSellingProducts(
                    x.ProductId,
                    x.ProductName,
                    x.Stock,
                    x.TotalAmountSold,
                    index + 1
                ))
                .ToList();
        }


        private void FillService(List<GroceryListItem> groceryListItems)
        {
            foreach (GroceryListItem g in groceryListItems)
            {
                g.Product = _productRepository.Get(g.ProductId) ?? new(0, "", 0);
            }
        }
    }
}
