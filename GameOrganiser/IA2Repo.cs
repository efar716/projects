public interface IA2Repo
{

    public string AddUser(User user);
    public bool ValidLogin(string userName, string password);
    public GameRecord AddGameRecord(string username);
    public GameRecord getRecord();
    public Boolean GameRecordExist();
    public Boolean GameRecordForMoveExist(string gameId);
    public Boolean IsPlayerInGame(string username);
    public Boolean IsPlayerLinkedToGame(string username, string gameId);
    public void DeleteGameRecord(string gameId);
    public GameRecord GetRecordForMove(string gameId);
    public void SaveChanges();

}
