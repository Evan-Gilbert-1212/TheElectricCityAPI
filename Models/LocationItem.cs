using System;

namespace TheElectricCityAPI.Models
{
  public class LocationItem
  {
    public int ID { get; set; }
    public int InventoryItemID { get; set; }
    public int NumberInStock { get; set; }
    public int LocationID { get; set; }
    public InventoryItem InventoryItem { get; set; }
    public Location Location { get; set; }
  }
}