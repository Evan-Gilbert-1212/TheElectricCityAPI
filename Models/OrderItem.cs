namespace TheElectricCityAPI.Models
{
  public class OrderItem
  {
    public int ID { get; set; }
    public int InventoryItemID { get; set; }
    public int QuantityOrdered { get; set; }
    public int OrderID { get; set; }

    public InventoryItem InventoryItem { get; set; }
    public Order Order { get; set; }
  }
}