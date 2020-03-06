using System;
using System.Collections.Generic;

namespace TheElectricCityAPI.Models
{
  public class Location
  {
    public int ID { get; set; }
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string AddressCity { get; set; }
    public string AddressState { get; set; }
    public string AddressZip { get; set; }
    public string ManagerName { get; set; }
    public string PhoneNumber { get; set; }

    public List<LocationItem> LocationItems { get; set; } = new List<LocationItem>();
  }
}