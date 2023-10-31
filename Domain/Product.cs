using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ProductManagerWebAPI.Domain;

[Index(nameof(SerialNum), IsUnique = true)]
public class Product
{
    public int Id { get; set; }
    public string ProductName { get; set; }
    public string SerialNum { get; set; }
    public string ProductDesc { get; set; }
    public string ImageUrl { get; set; }
    public int Price { get; set; }
}



