using ProductManagerWebAPI.Domain;
using Microsoft.AspNetCore.Mvc;
using ProductManagerWebAPI.Data;

namespace ProductManagerWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
  private readonly ApplicationDbContext context;

  public ProductsController(ApplicationDbContext context)
  {
    this.context = context;
  }

  [HttpGet]
  public IEnumerable<Product> GetProducts()
  {
    var products = context.Products.ToList();

    return products;
  }

  [HttpGet("{serialNum}")]
  public ActionResult<Product> GetProduct(string serialNum)
  {
    // var product = context.Products
    // .FirstOrDefault(x => x.SerialNum == serialNum);
    //captall and small alphabets are accepted:
    var product = context.Products
        .FirstOrDefault(x => x.SerialNum.Equals(serialNum, StringComparison.OrdinalIgnoreCase));

    if (product is null)
    {
      return NotFound();
    }
    return product;
  }

  [HttpPost]
  public IActionResult CreateProduct(Product product)
  {
    context.Products.Add(product);

    context.SaveChanges();

    // //===========>
    // if (string.IsNullOrEmpty(product.ProductName))
    //   return BadRequest(Product Name is required.);

    // if (string.IsNullOrEmpty(product.SerialNum))
    //   return BadRequest(Serial Number is required.);

    //   if (string.IsNullOrEmpty(product.ProductDesc))
    //   return BadRequest(Product Description is required.);

    //   if (string.IsNullOrEmpty(product.ImageUrl))
    //   return BadRequest(Image Url is required.);
    // //<===========


    return CreatedAtAction(
      nameof(GetProduct),
      new { serialNum = product.SerialNum },
      product);
  }

  [HttpDelete("{id}")]
  public IActionResult DeleteProduct(int id)
  {
    var product = context.Products.FirstOrDefault(x => x.Id == id);

    if (product is null)
    {
      return NotFound(); // 404 Not Found
    }
    context.Products.Remove(product);

    context.SaveChanges();

    return NoContent(); // 204 No Content
  }
}

