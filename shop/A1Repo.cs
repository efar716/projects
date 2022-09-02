using Microsoft.EntityFrameworkCore.ChangeTracking;

public class A1Repo : IA1Repo
{
    private readonly A1DBContext _dbContext;

    public A1Repo(A1DBContext dbContext)
    {
        _dbContext = dbContext;
    }



    public IEnumerable<Product> GetAllProducts()
    {
        IEnumerable<Product> products = _dbContext.Products.ToList();
        return products;
    }

    public IEnumerable<Comment> GetAllComments()
    {
        IEnumerable<Comment> comments = _dbContext.Comments.ToList();
        return comments;
    }





    public Comment AddComment(Comment comment)
    {
        EntityEntry<Comment> e = _dbContext.Comments.Add(comment);
        Comment c = e.Entity;
        _dbContext.SaveChanges();
        return c;
    }


    public void SaveChanges()
    {
        _dbContext.SaveChanges();
    }

}