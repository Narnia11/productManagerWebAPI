using ProductManagerWebAPI.Domain;
using Microsoft.AspNetCore.Mvc;
using ProductManagerWebAPI.Data;
using System.ComponentModel.DataAnnotations;

namespace ProductManagerWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
  private readonly ApplicationDbContext context; //reference to database context.

  //Constructor for the controller(as an instance of the database context)
  public ProductsController(ApplicationDbContext context)
  {
    this.context = context;
  }

  //GET https://localhost:8000/products
  //GET https://localhost:8000/products?productName={productName}
  /// <summary>
  /// Fetches all products or filters by product name.
  /// </summary>
  /// <param name="productName">Filter on productName</param>
  /// <returns>Array of products</returns>
  [HttpGet]
  [Produces("application/json")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  public IEnumerable<ProductDto> GetProducts([FromQuery] string? productName)
  {
    IEnumerable<Product> products = string.IsNullOrEmpty(productName)
        ? context.Products.ToList()
        : context.Products.Where(x => x.ProductName == productName);

    //Create DTOs(Data Transfer Objects) for the products to send as responses.
    IEnumerable<ProductDto> productDtos = products.Select(x => new ProductDto
    {
      Id = x.Id,
      ProductName = x.ProductName,
      SerialNum = x.SerialNum,
      ProductDesc = x.ProductDesc,
      ImageUrl = x.ImageUrl,
      Price = x.Price
    });

    return productDtos;
  }

  //GET https://localhost:8000/products/{sku}
  /// <summary>
  /// Searches a product by serialNum
  /// </summary>
  [HttpGet("{serialNum}")]
  [Produces("application/json")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public ActionResult<ProductDto> GetProduct(string serialNum)
  {
    var product = context.Products.FirstOrDefault(x => x.SerialNum == serialNum);

    if (product is null)
      return NotFound();// 404 Not Found

    //Create a DTO for the found product
    var productDto = new ProductDto
    {
      Id = product.Id,
      ProductName = product.ProductName,
      SerialNum = product.SerialNum,
      ProductDesc = product.ProductDesc,
      ImageUrl = product.ImageUrl,
      Price = product.Price
    };

    return productDto;// 200 OK
  }

  /// <summary>
  /// Creates new product
  /// </summary>
  [HttpPost]
  [Consumes("application/json")]
  [Produces("application/json")]
  [ProducesResponseType(StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  
  //checks CreateProductRequest requirements&conditions
  public ActionResult<ProductDto> CreateProduct(CreateProductRequest request)
  {
    // Create a new product entity based on the data in the request.
    var product = new Product
    {
      ProductName = request.ProductName,
      SerialNum = request.SerialNum,
      ProductDesc = request.ProductDesc,
      ImageUrl = request.ImageUrl,
      Price = request.Price
    };

    // Check if a product with the same serialNum already exists
    if (context.Products.Any(p => p.SerialNum == product.SerialNum))
    {
      return BadRequest("A product with the same serialNum already exists.");
    }

    // If no duplicate serialNum is found, proceed to add the product.
    context.Products.Add(product);
    context.SaveChanges();

    // Create a DTO for the newly created product
    var productDto = new ProductDto
    {
      Id = product.Id,
      ProductName = product.ProductName,
      SerialNum = product.SerialNum,
      ProductDesc = product.ProductDesc,
      ImageUrl = product.ImageUrl,
      Price = product.Price
    };

    return CreatedAtAction(  // 201 Created
        nameof(GetProduct),
        new { serialNum = product.SerialNum },
        productDto);
  }

  //DELETE https://localhost:8000/products/{sku}
  /// <summary>
  /// Deletes product by serialNum
  /// </summary>
  [HttpDelete("{serialNum}")]
  [Produces("application/json")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public IActionResult DeleteProduct(string serialNum)
  {
    // Retrieve the product with the specified serial number from the database.
    var product = context.Products.FirstOrDefault(x => x.SerialNum == serialNum);

    if (product is null)
    {
      return NotFound(); // 404 Not Found
    }
    context.Products.Remove(product);

    context.SaveChanges();

    return NoContent(); // 204 No Content
  }

  /// <summary>
  /// Data to create product
  /// </summary>
  public class CreateProductRequest //class for the request to create a product.
  {
    [Required]
    [MaxLength(50)]
    public string ProductName { get; set; }

    [Required]
    [MaxLength(10)]
    public string SerialNum { get; set; }

    [Required]
    [MaxLength(50)]
    public string ProductDesc { get; set; }

    [Required]
    [MaxLength(100)]
    public string ImageUrl { get; set; }
    public int Price { get; set; }
  }


  public class ProductDto //class for the product DTO (Data Transfer Object)
  {
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string ProductName { get; set; }

    [Required]
    [MaxLength(10)]
    public string SerialNum { get; set; }

    [Required]
    [MaxLength(50)]
    public string ProductDesc { get; set; }

    [Required]
    [MaxLength(100)]
    public string ImageUrl { get; set; }
    public int Price { get; set; }
  }
}

