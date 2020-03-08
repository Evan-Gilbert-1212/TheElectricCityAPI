using System;
using System.Collections.Generic;

namespace TheElectricCityAPI.Models
{
  public class Order
  {
    public int ID { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.Now;
    public string OrderEmail { get; set; }
    public int LocationId { get; set; }

    public Location Location { get; set; }
    public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
  }
}