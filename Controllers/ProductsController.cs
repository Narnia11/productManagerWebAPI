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
    public IEnumerable<Product> GetProducts([FromQuery] string? productName)
    {
        var products = productName is null 
            ? context.Products.ToList()
            : context.Products.Where(x => x.ProductName.Contains(productName)).ToList();

        return products;
    }

  [HttpGet("{serialNum}")]
  public ActionResult<Product> GetProduct(string serialNum)
  {
    var product = context.Products.FirstOrDefault(x => x.SerialNum == serialNum);

    if (product is null)
    {
      return NotFound();
    }
    return product;
  }

  [HttpPost]
  public IActionResult CreateProduct(Product product)
  {
    //"Products" is defined in DbContextApplication
    context.Products.Add(product);

    context.SaveChanges();

    return CreatedAtAction(
      nameof(GetProduct),
      new { serialNum = product.SerialNum },
      product);
  }

  [HttpDelete("{serialNum}")]
  public IActionResult DeleteProduct(string serialNum)
  {
    var product = context.Products.FirstOrDefault(x => x.SerialNum == serialNum);

    if (product is null)
    {
      return NotFound(); // 404 Not Found
    }
    context.Products.Remove(product);

    context.SaveChanges();

    return NoContent(); // 204 No Content
  }
}

