using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheElectricCityAPI.Models;
using System.Text.Json;

namespace TheElectricCityAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class LocationController : ControllerBase
  {
    public DatabaseContext electricCityDb = new DatabaseContext();

    //A GET endpoint that gets all locations
    [HttpGet]
    public ActionResult GetAllLocations()
    {
      return new ContentResult()
      {
        Content = JsonSerializer.Serialize(electricCityDb.Locations),
        ContentType = "application/json",
        StatusCode = 200
      };
    }

    // A POST endpoint that allows a user to create a location
    [HttpPost]
    public async Task<ActionResult> AddLocation(Location locationToAdd)
    {
      await electricCityDb.Locations.AddAsync(locationToAdd);
      await electricCityDb.SaveChangesAsync();

      return new ContentResult()
      {
        Content = JsonSerializer.Serialize(locationToAdd),
        ContentType = "application/json",
        StatusCode = 201
      };
    }
  }
}