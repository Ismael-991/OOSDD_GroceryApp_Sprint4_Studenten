using CommunityToolkit.Mvvm.ComponentModel;

namespace Grocery.Core.Models
{
    public partial class BestSellingProducts : Model
    {
        public int Stock { get; set; }
        
        [ObservableProperty]
        private int _nrOfSells;
        
        [ObservableProperty]
        private int _ranking;

        /// <summary>
        /// Constructor om een nieuw BestSellingProducts object aan te maken met alle verkoopstatistieken
        /// </summary>
        /// <param name="productId">Het unieke ID van het product</param>
        /// <param name="name">De naam van het product</param>
        /// <param name="stock">De huidige voorraad</param>
        /// <param name="nrOfSells">Het aantal keer dat het product verkocht is</param>
        /// <param name="ranking">De ranking positie in de lijst</param>

        public BestSellingProducts(int productId, string name, int stock, int nrOfSells, int ranking) 
            : base(productId, name)
        {
            Stock = stock;
            NrOfSells = nrOfSells;
            Ranking = ranking;
        }
    }
}
