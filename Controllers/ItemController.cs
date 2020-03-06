using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheElectricCityAPI.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

/*
  Mark challenges you to move the JsonSerializerSettings to the Startup.cs file
  so you don't have to define it every time you use JsonSerializer
*/

namespace TheElectricCityAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ItemController : ControllerBase
  {
    public DatabaseContext electricCityDb = new DatabaseContext();

    // Create a GET endpoint for all items in your inventory
    // Update the GET all items endpoint to need a location
    [HttpGet("{locationId}")]
    public ActionResult GetAllItemsForLocation(int locationId)
    {
      var itemsquery =
          from item in electricCityDb.InventoryItems
          join loc in electricCityDb.LocationItems on item.ID equals loc.InventoryItemID
          select new { item, loc.LocationID };

      return new ContentResult()
      {
        Content = JsonConvert.SerializeObject(itemsquery,
        new JsonSerializerSettings
        {
          ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        }),
        ContentType = "application/json",
        StatusCode = 200
      };
    }

    // Create a GET endpoint for each item
    // Update the GET endpoint for each item to need a location
    [HttpGet("{itemId}/{locationId}")]
    public ActionResult GetItemForLocation(int itemId, int locationId)
    {
      var itemsquery =
          from item in electricCityDb.InventoryItems
          join loc in electricCityDb.LocationItems on item.ID equals loc.InventoryItemID
          where item.ID == itemId && loc.LocationID == locationId
          select new { item, loc.LocationID };

      return new ContentResult()
      {
        Content = JsonConvert.SerializeObject(itemsquery,
        new JsonSerializerSettings
        {
          ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        }),
        ContentType = "application/json",
        StatusCode = 200
      };
    }

    // Create a GET endpoint to get all items that are out of stock
    // Update GET endpoint to get all items that are out of stock for a location.
    [HttpGet("outofstock/{locationId}")]
    public ActionResult GetOutOfStockItems(int locationId)
    {
      return new ContentResult()
      {
        Content = JsonConvert.SerializeObject(electricCityDb.LocationItems
                             .Where(i => i.NumberInStock == 0 && i.LocationID == locationId),
        new JsonSerializerSettings
        {
          ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        }),
        ContentType = "application/json",
        StatusCode = 200
      };
    }

    // Create a GET endpoint that allows them to search for an item based on SKU
    [HttpGet("bysku/{sku}")]
    public ActionResult GetItemBySku(string sku)
    {
      return new ContentResult()
      {
        Content = JsonConvert.SerializeObject(electricCityDb.InventoryItems.Where(i => i.SKU == sku),
        new JsonSerializerSettings
        {
          ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        }),
        ContentType = "application/json",
        StatusCode = 200
      };
    }

    // Create a POST endpoint that allows a client to add an item to the inventory
    // Update the POST endpoint that allows a user/client to add an item to the inventory to a location
    [HttpPost("{locationId}")]
    public async Task<ActionResult> AddItem(InventoryItem itemToAdd, int locationId)
    {
      var locationEntry = new LocationItem()
      {
        LocationID = locationId
      };

      itemToAdd.LocationItems.Add(locationEntry);

      await electricCityDb.InventoryItems.AddAsync(itemToAdd);
      await electricCityDb.SaveChangesAsync();

      return new ContentResult()
      {
        Content = JsonConvert.SerializeObject(itemToAdd,
        new JsonSerializerSettings
        {
          ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        }),
        ContentType = "application/json",
        StatusCode = 201
      };
    }

    // Create a PUT endpoint that allows a client to update an item
    [HttpPut("{Id}")]
    public async Task<ActionResult> UpdateItem(int Id, InventoryItem itemToUpdate)
    {
      itemToUpdate.ID = Id;
      electricCityDb.Entry(itemToUpdate).State = EntityState.Modified;
      await electricCityDb.SaveChangesAsync();

      return new ContentResult()
      {
        Content = JsonConvert.SerializeObject(itemToUpdate,
        new JsonSerializerSettings
        {
          ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        }),
        ContentType = "application/json",
        StatusCode = 200
      };
    }

    // Create a PUT endpoint that allows a user/client to update location specific item info
    [HttpPut("{itemId}/{locationId}")]
    public async Task<ActionResult> UpdateLocationItem(int itemId, int locationId, int NumOfStockToUpdate)
    {
      var itemToUpdate = await electricCityDb.LocationItems
                               .Where(l => l.InventoryItemID == itemId && l.LocationID == locationId)
                               .FirstOrDefaultAsync();

      itemToUpdate.NumberInStock += NumOfStockToUpdate;

      await electricCityDb.SaveChangesAsync();

      return new ContentResult()
      {
        Content = JsonConvert.SerializeObject(itemToUpdate,
        new JsonSerializerSettings
        {
          ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        }),
        ContentType = "application/json",
        StatusCode = 200
      };
    }

    // Create a DELETE endpoint that allows a client to delete an item
    [HttpDelete("{itemId}")]
    public async Task<ActionResult> DeleteItem(int itemId)
    {
      var itemToDelete = await electricCityDb.InventoryItems.FirstOrDefaultAsync(i => i.ID == itemId);
      electricCityDb.InventoryItems.Remove(itemToDelete);
      await electricCityDb.SaveChangesAsync();

      return Ok();
    }

    //Update the DELETE endpoint that allows a user/client to delete an item for a location
    [HttpDelete("{itemId}/{locationId}")]
    public async Task<ActionResult> DeleteLocationItem(int itemId, int locationId)
    {
      var itemToDelete = await electricCityDb.LocationItems
                               .FirstOrDefaultAsync(i => i.InventoryItemID == itemId && i.LocationID == locationId);
      electricCityDb.LocationItems.Remove(itemToDelete);
      await electricCityDb.SaveChangesAsync();

      return Ok();
    }
  }
}