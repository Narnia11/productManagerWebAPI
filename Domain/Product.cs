using System.ComponentModel.DataAnnotations;

namespace ProductManagerWebAPI.Domain;

public class Product
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



