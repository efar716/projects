using System.ComponentModel.DataAnnotations;

public class Comment
{


    [Key]
    public int Id { get; set; }
    
    public string userComment { get; set; }
    public string name { get; set; }
    public string IP { get; set; }

    public string Time { get; set; }


}
