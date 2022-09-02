using Microsoft.EntityFrameworkCore.ChangeTracking;

public class A2Repo : IA2Repo
{


    private readonly A2DBContext _dbContext;


    public A2Repo(A2DBContext dbContext)
    {
        _dbContext = dbContext;
    }


    public string AddUser(User user){
        var present = _dbContext.Users.FirstOrDefault(e => e.UserName == user.UserName);
        if (present == null)
        {

            EntityEntry<User> e = _dbContext.Users.Add(user);
            User c = e.Entity;
            _dbContext.SaveChanges();
            return "User successfully registered.";

        }
        else
        {
            
         return "Username not available.";

                }


    }

    public GameRecord AddGameRecord(string username)
    {
       
            GameRecord new_record = new GameRecord { GameID = System.Guid.NewGuid().ToString(), Player1 = username, State = "wait" };
            EntityEntry<GameRecord> e = _dbContext.GameRecords.Add(new_record);
            GameRecord f = e.Entity;
            _dbContext.SaveChanges();
            return f;
    }


    public GameRecord getRecord()
    {
        GameRecord old_record = _dbContext.GameRecords.FirstOrDefault(e => e.State == "wait");
        return old_record;

    }


    public Boolean GameRecordExist()
    {

        var present = _dbContext.GameRecords.FirstOrDefault(e => e.State == "wait");
        if (present == null) return false;

        else return true;

    }

    public Boolean GameRecordForMoveExist(string gameId)
    {

        var present = _dbContext.GameRecords.FirstOrDefault(e => e.GameID == gameId);
        if (present == null) return false;

        else return true;

    }

    public void DeleteGameRecord(string gameId)
    {
        GameRecord record = _dbContext.GameRecords.FirstOrDefault(e => e.GameID == gameId);
        if (record != null)
        {
            _dbContext.GameRecords.Remove(record);
            _dbContext.SaveChanges();
        }


    }

    public Boolean IsPlayerLinkedToGame(string username, string gameId)
    {
        var LinkPlayerToGame = _dbContext.GameRecords.FirstOrDefault(e => (e.Player1 == username || e.Player2 == username) && e.GameID == gameId);

        if (LinkPlayerToGame == null) return false;

        else return true;
    }


    public Boolean IsPlayerInGame(string username)
    {
        var exist = _dbContext.GameRecords.FirstOrDefault(e => e.Player1 == username || e.Player2 == username);
        if (exist == null) return false;

        else return true;
    }



    public GameRecord GetRecordForMove(string gameId)
    {

        GameRecord record = _dbContext.GameRecords.FirstOrDefault(e => e.GameID == gameId);
        return record;
        
    }




    public bool ValidLogin(string userName, string password)
    {
        var  c = _dbContext.Users.FirstOrDefault(e => e.UserName == userName && e.Password == password);
        if (c == null)
            return false;
        else
            return true;
    }

    public void SaveChanges()
    {
        _dbContext.SaveChanges();
    }



}