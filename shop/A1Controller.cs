using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

[Route("api")]
[ApiController]
public class Controller_1 : Controller



{


    private readonly IA1Repo _repository;

    public Controller_1(IA1Repo repository)
    {
        _repository = repository;
    }

    [HttpGet("GetLogo")]
    public ActionResult GetLogo()
    {
        string path = Directory.GetCurrentDirectory();
        string imgDir = Path.Combine(path, "Logos");
        string fileName1 = Path.Combine(imgDir, "Logo" + ".png");
        Byte[] b = System.IO.File.ReadAllBytes(fileName1);

        return File(b, "image/png");
    }

    [HttpGet("GetFavIcon")]
    public ActionResult GetFavIcon()

    {
        string path = Directory.GetCurrentDirectory();
        string imgDir = Path.Combine(path, "Logos");
        string fileName1 = Path.Combine(imgDir, "Logo-192x192" + ".png");
        Byte[] b = System.IO.File.ReadAllBytes(fileName1);
        return File(b, "image/png");
    }


    [HttpGet("GetVersion")]

    public ActionResult<string> GetVersion()
    {
        return Ok("1.0.0");


    }







    [HttpGet("AllItems")]
    public ActionResult<IEnumerable<Product>> AllItems()
    {
        IEnumerable<Product> products = _repository.GetAllProducts();
        return Ok(products);
    }

    [HttpGet("GetItems/{name}")]
    public ActionResult<IEnumerable<Product>> GetItems(string name)
    {
        string used_name = name;
        IEnumerable<Product> products = _repository.GetAllProducts().Where(e => ModedContains(e.Name,used_name));
        return Ok(products);
    }

    public static bool ModedContains( string text, string value,
        StringComparison stringComparison = StringComparison.CurrentCultureIgnoreCase)
    {
        return text.IndexOf(value, stringComparison) >= 0;
    }



    [HttpGet("ItemPhoto/{id}")]
    public IActionResult GetItemPhoto(long id)
    {
        long id1 = id;
        string the_id = id1.ToString();
        string path = Directory.GetCurrentDirectory();
        string imgDir = Path.Combine(path, "ItemsImages");
        string fileName1 = Path.Combine(imgDir, the_id + ".jpg");
        string fileName2 = Path.Combine(imgDir, the_id + ".gif");
        string the_default = Path.Combine(imgDir, "default" + ".png");



        if (System.IO.File.Exists(fileName1))
            {
                Byte[] s = System.IO.File.ReadAllBytes(fileName1);
                return File(s, "image/jpg");

            }

            else if(System.IO.File.Exists(fileName2)) {
                Byte[] s = System.IO.File.ReadAllBytes(fileName2);
                return File(s, "image/gif");
            }
        else {
            Byte[] s = System.IO.File.ReadAllBytes(the_default);
            return File(s, "image/png");
        }

        

    }

        

            
        

    


    [HttpPost("WriteComment")]
    public ActionResult<string> WriteComment(CommentDto new_comment)
    {
        var IpAddress = Request.HttpContext.Connection.RemoteIpAddress;
        string ip = IpAddress.ToString();
       
        string time = DateTime.Now.TimeOfDay.ToString();
        Comment c = new Comment { name = new_comment.name, userComment = new_comment.userComment,IP = ip,Time = time };
        Comment addedCustomer = _repository.AddComment(c);
        CommentDto co = new CommentDto { name = addedCustomer.name, userComment = addedCustomer.userComment };

        return Ok(co.userComment);
        

    }



    [HttpGet("GetComments")]
    public ActionResult<IEnumerable<Comment>> GetComments()
    {
        IEnumerable<Comment> comments = _repository.GetAllComments();

        List<CommentDto> asList = new List<CommentDto>();

        int counter = 0;
        int index = comments.Count() - 1;
        for (int i = 0; i< comments.Count(); i++)
        {
            CommentDto co = new CommentDto { name = comments.ElementAt(index).name, userComment = comments.ElementAt(index).userComment };
            asList.Add(co );
            counter++;
            index--;
            if (counter >= 5) { break; }
            
        }

        return Ok(asList);



    }











}