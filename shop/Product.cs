using System.ComponentModel.DataAnnotations;

public class Product
{


    [Key]
    public string ID { get; set; }
    public string Name { get; set; }
    public string Price { get; set; }
    public string Description { get; set; }




}