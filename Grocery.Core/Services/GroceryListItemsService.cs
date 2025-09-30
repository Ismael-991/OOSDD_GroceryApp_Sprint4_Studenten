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

        public List<BestSellingProducts> GetBestSellingProducts(int topX = 5)
        {
            var allItems = _groceriesRepository.GetAll();
            var productIds = allItems.Select(i => i.ProductId).Distinct().ToList();
            var allProducts = _productRepository.GetByIds(productIds) // <-- nieuwe methode in repo
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
                        NrOfSells = g.Count()
                    };
                })
                .OrderByDescending(x => x.NrOfSells)
                .Take(topX)
                .ToList();

            return grouped
                .Select((x, index) => new BestSellingProducts(
                    x.ProductId,
                    x.ProductName,
                    x.Stock,
                    x.NrOfSells,
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
