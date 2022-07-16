using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace ShoppingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingController : ControllerBase
    {

        
        /*private static List<Shopping> items = new List<Shopping>
            {
                new Shopping {
                    Id = 1, 
                    Name="Laptop", 
                    Price = 1200, 
                    Company= "Apple", 
                    Quantity=1
                },
                new Shopping {
                    Id = 2,
                    Name="EarBuds",
                    Price = 150,
                    Company= "Samsung",
                    Quantity=2
                },
                new Shopping {
                    Id = 3,
                    Name="TV",
                    Price = 300,
                    Company= "Sony",
                    Quantity=1
                }
            };*/
        private readonly DataContext _context;

        public ShoppingController(DataContext context)
        {
            _context = context;
        }

        [HttpGet] // find
        public async Task<ActionResult<List<Shopping>>> Get()
        {
            return Ok(await _context.Shoppings.ToListAsync());
        }
        //[HttpGet("{id}")] // find by id
        //public async Task<ActionResult<Shopping>> Get(int id)
        //{
        //    var item = await _context.Shoppings.FindAsync(id);
        //    if (item == null)
        //        return BadRequest("Item not found");
        //    return Ok(item);
        //}
        [Route("Add")]
        [HttpPost] // add
        public async Task<ActionResult<List<Cart>>> AddProduct(int ProdId)
        {

          /*  Cart cart = _context.Cart.Where(x => x.Id == item.ProdId).FirstOrDefault();
            if (cart != null)
            {
                cart.Quantity += 1;
            }
            else
            {
            }*/

            var cart = _context.Cart.ToList().FirstOrDefault(x => x.ProdId == ProdId);
            if(cart == null)
            {
                _context.Cart.Add(new Cart { ProdId = ProdId, Quantity = 1});

            }
            else
            {
                cart.Quantity += 1;
                _context.Update(cart);
            }
            await _context.SaveChangesAsync();
            return Ok(await _context.Cart.ToListAsync());
        }

        [Route("show")] 
        [HttpGet] // show
        public async Task<ActionResult<List<Cart>>> ShowCart()
        {
            var cartProducts = from product in await _context.Shoppings.ToListAsync()
                               join
                               cart in await _context.Cart.ToListAsync() on product.Id equals cart.ProdId
                               select new { cart.ProdId ,product.Name, product.Company, product.Price, cart.Quantity };
            
            return Ok(cartProducts);
        }

        //[HttpPut] // update
        //public async Task<ActionResult<List<Shopping>>> UpdateProduct(Shopping request)
        //{
        //    var dbitem = await _context.Shoppings.FindAsync(request.Id);
        //    if (dbitem == null)
        //        return BadRequest("Item not found");


        //    dbitem.Name = request.Name;
        //    dbitem.Company = request.Company; 
        //    dbitem.Price = request.Price;
        //    dbitem.Quantity = request.Quantity;   

        //    await _context.SaveChangesAsync();

        //    return Ok(await _context.Shoppings.ToListAsync());
        //}
        [Route("delete")]
        [HttpDelete] // delete by id
        public async Task<ActionResult<List<Cart>>> Delete(int ProdId)
        {
            var dbitem = _context.Cart.ToList().FirstOrDefault(x => x.ProdId == ProdId);
            if (dbitem == null)
                return BadRequest("Item not found");
            dbitem.Quantity--;
            if(dbitem.Quantity < 0)
                _context.Cart.Remove(dbitem);
            else
                _context.Cart.Update(dbitem);
            await _context.SaveChangesAsync();
            return Ok(await _context.Cart.ToListAsync());
        }
     
    }
}
