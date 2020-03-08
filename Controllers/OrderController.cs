using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheElectricCityAPI.Models;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace TheElectricCityAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class OrderController : ControllerBase
  {
    public DatabaseContext electricCityDb = new DatabaseContext();

    // Create an endpoint to get all orders in database
    [HttpGet]
    public ActionResult GetAllOrders()
    {
      return new ContentResult()
      {
        Content = JsonConvert.SerializeObject(electricCityDb.Orders.Include(o => o.OrderItems),
        new JsonSerializerSettings
        {
          ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        }),
        ContentType = "application/json",
        StatusCode = 200
      };
    }

    // Create an endpoint to get all orders for a specific location
    [HttpGet("{locationId}")]
    public ActionResult GetLocationOrders(int locationId)
    {
      return new ContentResult()
      {
        Content = JsonConvert.SerializeObject(electricCityDb.Orders.Include(o => o.OrderItems).Where(o => o.LocationId == locationId),
        new JsonSerializerSettings
        {
          ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        }),
        ContentType = "application/json",
        StatusCode = 200
      };
    }

    // Create an endpoint to add an order to the database (by location)
    [HttpPost("{locationId}")]
    public async Task<ActionResult> AddOrder(Order orderToAdd, int locationId)
    {
      if (VerifyOrder(orderToAdd, locationId))
      {
        orderToAdd.LocationId = locationId;

        await electricCityDb.Orders.AddAsync(orderToAdd);

        foreach (var item in orderToAdd.OrderItems)
        {
          var itemToUpdate = await electricCityDb.LocationItems
                                   .Where(i => i.LocationID == locationId && i.InventoryItemID == item.InventoryItemID)
                                   .FirstOrDefaultAsync();

          itemToUpdate.NumberInStock -= item.QuantityOrdered;
        }

        await electricCityDb.SaveChangesAsync();

        return new ContentResult()
        {
          Content = JsonConvert.SerializeObject(orderToAdd,
            new JsonSerializerSettings
            {
              ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }),
          ContentType = "application/json",
          StatusCode = 201
        };
      }
      else
      {
        return Ok(new { message = "Order could not be completed. There is not enough stock to fulfill the order." });
      }
    }

    // Add a method to verify that there is enough stock for all items in the order.
    public bool VerifyOrder(Order orderToVerify, int locationId)
    {
      var verified = true;

      foreach (var item in orderToVerify.OrderItems)
      {
        var itemToVerify = electricCityDb.LocationItems
                                 .Where(i => i.LocationID == locationId && i.InventoryItemID == item.InventoryItemID)
                                 .FirstOrDefault();

        if (itemToVerify.NumberInStock < item.QuantityOrdered)
        {
          verified = false;
          break;
        }
      }

      return verified;
    }

    // Create an endpoint that will update an item within an order with a new quantity
    // quantityToUpdate will be positive if adding to the order, negative if subtracting from the order
    [HttpPatch("{orderId}/{itemId}/{quantityToUpdate}")]
    public async Task<ActionResult> UpdateAmountOrdered(int orderId, int itemId, int quantityToUpdate)
    {
      var order = await electricCityDb.Orders.Include(o => o.OrderItems).Where(o => o.ID == orderId).FirstOrDefaultAsync();

      var itemToUpdate = await electricCityDb.LocationItems.Where(o => o.LocationID == order.LocationId).FirstOrDefaultAsync();

      var orderItemToUpdate = await electricCityDb.OrderItems
                              .Where(o => o.OrderID == orderId && o.InventoryItemID == itemId)
                              .FirstOrDefaultAsync();

      if (itemToUpdate.NumberInStock < quantityToUpdate)
      {
        return new ContentResult()
        {
          Content = "You cannot make this change because there is not enough stock available.",
          ContentType = "plain/text",
          StatusCode = 406
        };
      }
      else
      {
        itemToUpdate.NumberInStock -= quantityToUpdate;

        orderItemToUpdate.QuantityOrdered += quantityToUpdate;

        await electricCityDb.SaveChangesAsync();

        return new ContentResult()
        {
          Content = JsonConvert.SerializeObject(order,
          new JsonSerializerSettings
          {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
          }),
          ContentType = "application/json",
          StatusCode = 200
        };
      }
    }

    // Create an endpoint that will delete an entire order from the system
    // This process should also return the stock to the stores inventory
    [HttpDelete("{orderId}")]
    public async Task<ActionResult> DeleteOrder(int orderId)
    {
      var orderToDelete = await electricCityDb.Orders.Where(o => o.ID == orderId).FirstOrDefaultAsync();

      var orderItemsToDelete = electricCityDb.OrderItems.Where(i => i.OrderID == orderId);

      foreach (var item in orderItemsToDelete)
      {
        var itemToUpdate = await electricCityDb.LocationItems
                           .Where(i => i.LocationID == orderToDelete.LocationId && i.InventoryItemID == item.InventoryItemID)
                           .FirstOrDefaultAsync();

        itemToUpdate.NumberInStock += item.QuantityOrdered;
      }

      electricCityDb.Orders.Remove(orderToDelete);

      await electricCityDb.SaveChangesAsync();

      return Ok(new { message = "Order has been deleted successfully." });
    }
  }
}