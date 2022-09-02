using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

public class GameRecord
{

    [Key]
    public int ID { get; set; }
    public string GameID { get; set; }
    public string State { get; set; }
    public string? Player1 { get; set; }
    public string? Player2 { get; set; }
    public string? LastMovePlayer1 { get; set; }
    public string? LastMovePlayer2 { get; set; }

}
