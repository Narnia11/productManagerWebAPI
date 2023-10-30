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

  //GET https://localhost:8000/products
  //GET https://localhost:8000/products?productName={productName}
  [HttpGet]
  public IEnumerable<ProductDto> GetProducts([FromQuery] string? productName)
  {
    var products = productName is null
        ? context.Products.ToList()
        : context.Products.Where(x => x.ProductName.Contains(productName)).ToList();

var productsDto = products.Select(products => new ProductDto
    {
      Id = products.Id,
      ProductName = products.ProductName,
      SerialNum = products.SerialNum,
      ProductDesc = products.ProductDesc,
      ImageUrl = products.ImageUrl,
      Price = products.Price
    });

    return productsDto;
  }

  //GET https://localhost:8000/products/{sku}
  [HttpGet("{serialNum}")]
  public ActionResult<ProductDto> GetProduct(string serialNum)
  {
    var product = context.Products.FirstOrDefault(x => x.SerialNum == serialNum);

    if (product is null)
      return NotFound();

      var productDto = new ProductDto
    {
      Id = product.Id,
      ProductName = product.ProductName,
      SerialNum = product.SerialNum,
      ProductDesc = product.ProductDesc,
      ImageUrl = product.ImageUrl,
      Price = product.Price
    };
    
    return productDto;
  }

  //POST https://localhost:8000/products
  [HttpPost]
  public ActionResult<ProductDto> CreateProduct(Product product)
  {
    //"Products" is defined in DbContextApplication
    context.Products.Add(product);

    context.SaveChanges();

    var productDto = new ProductDto
    {
      Id = product.Id,
      ProductName = product.ProductName,
      SerialNum = product.SerialNum,
      ProductDesc = product.ProductDesc,
      ImageUrl = product.ImageUrl,
      Price = product.Price
    };

    return CreatedAtAction(
      nameof(GetProduct),
      new { serialNum = product.SerialNum },
      product);
  }

  //DELETE https://localhost:8000/products/{sku}
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

  public class ProductDto
  {
    public int Id { get; set; }
    public string ProductName { get; set; }
    public string SerialNum { get; set; }
    public string ProductDesc { get; set; }
    public string ImageUrl { get; set; }
    public int Price { get; set; }
  }
}

