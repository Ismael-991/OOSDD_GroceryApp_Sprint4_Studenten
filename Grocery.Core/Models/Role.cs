namespace Grocery.Core.Models
{
    /// <summary>
    /// Enum die de verschillende gebruikersrollen in de applicatie definieert.
    /// Wordt gebruikt om toegang tot bepaalde functies te beperken (zoals het bekijken van gekochte producten).
    /// </summary>
    public enum Role
    {
        /// <summary>
        /// Standaard rol zonder speciale rechten
        /// </summary>
        None,
        
        /// <summary>
        /// Administrator rol met toegang tot beheersfuncties zoals het bekijken van gekochte producten per klant
        /// </summary>
        Admin
    }
}