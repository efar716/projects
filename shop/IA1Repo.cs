public interface IA1Repo
{



    public IEnumerable<Product> GetAllProducts();


    public IEnumerable<Comment> GetAllComments();
    public Comment AddComment(Comment comment);

    


    public void SaveChanges();





}