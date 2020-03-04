using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheElectricCityAPI.Models;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace TheElectricCityAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ItemController : ControllerBase
  {
    public DatabaseContext electricCityDb = new DatabaseContext();

    // Create a GET endpoint for all items in your inventory
    [HttpGet]
    public ActionResult GetAllItems()
    {
      return new ContentResult()
      {
        Content = JsonSerializer.Serialize(electricCityDb.InventoryItems),
        ContentType = "application/json",
        StatusCode = 200
      };
    }

    // Create a GET endpoint for each item
    [HttpGet("{id}")]
    public ActionResult GetItem(int id)
    {
      return new ContentResult()
      {
        Content = JsonSerializer.Serialize(electricCityDb.InventoryItems.FirstOrDefault(i => i.ID == id)),
        ContentType = "application/json",
        StatusCode = 200
      };
    }

    // Create a GET endpoint to get all items that are out of stock
    [HttpGet("outofstock")]
    public ActionResult GetOutOfStockItems()
    {
      return new ContentResult()
      {
        Content = JsonSerializer.Serialize(electricCityDb.InventoryItems.Where(i => i.NumberInStock == 0)),
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
        Content = JsonSerializer.Serialize(electricCityDb.InventoryItems.Where(i => i.SKU == sku)),
        ContentType = "application/json",
        StatusCode = 200
      };
    }

    // Create a POST endpoint that allows a client to add an item to the inventory
    [HttpPost]
    public async Task<ActionResult> AddItem(InventoryItem itemToAdd)
    {
      await electricCityDb.InventoryItems.AddAsync(itemToAdd);
      await electricCityDb.SaveChangesAsync();

      return new ContentResult()
      {
        Content = JsonSerializer.Serialize(itemToAdd),
        ContentType = "application/json",
        StatusCode = 201
      };
    }

    // Create a PUT endpoint that allows a client to update an item
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateItem(int id, InventoryItem itemToUpdate)
    {
      itemToUpdate.ID = id;
      electricCityDb.Entry(itemToUpdate).State = EntityState.Modified;
      await electricCityDb.SaveChangesAsync();

      return new ContentResult()
      {
        Content = JsonSerializer.Serialize(itemToUpdate),
        ContentType = "application/json",
        StatusCode = 200
      };
    }

    // Create a DELETE endpoint that allows a client to delete an item
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteItem(int id)
    {
      var itemToDelete = await electricCityDb.InventoryItems.FirstOrDefaultAsync(i => i.ID == id);
      electricCityDb.InventoryItems.Remove(itemToDelete);
      await electricCityDb.SaveChangesAsync();

      return Ok();
    }
  }
}