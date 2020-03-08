using System;
using System.Collections.Generic;

namespace TheElectricCityAPI.Models
{
  public class InventoryItem
  {
    public int ID { get; set; }
    public string SKU { get; set; }
    public string ProductName { get; set; }
    public string ProductType { get; set; }
    public string Manufacturer { get; set; }
    public string ItemDescription { get; set; }
    public int NumberInStock { get; set; }
    public double Price { get; set; }
    public DateTime DateOrdered { get; set; } = DateTime.Now;

    public List<LocationItem> LocationItems { get; set; } = new List<LocationItem>();
    public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
  }
}