using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api")]
[ApiController]
public class A2Controller : Controller
{

    private readonly IA2Repo _repository;

    public A2Controller(IA2Repo repository)
    {
        _repository = repository;
    }




    [HttpPost("Register")]
    public ActionResult<string> Register(User user)
    {

        User c = new User { UserName = user.UserName, Password = user.Password, Address = user.Address };
        string message = _repository.AddUser(user);
        _repository.SaveChanges();
        return Ok(message);

    }


    [Authorize(AuthenticationSchemes = "MyAuthentication")]
    [Authorize(Policy = "UserOnly")]
    [HttpGet("GetVersionA")]
    public ActionResult<string> GetVersionA()
    {

        return Ok("1.0.0 (auth)");

    }


    [Authorize(AuthenticationSchemes = "MyAuthentication")]
    [Authorize(Policy = "UserOnly")]
    [HttpGet("PurchaseItem/{id}")]
    public IActionResult PurchaseItem(int id)
    {
        var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
        var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
        var credentials = Encoding.UTF8.GetString(credentialBytes).Split(":");
        var username_wanted = credentials[0];
        Order new_order = new Order { userName = username_wanted, productId = id };

        return Ok(new_order);

    }

    [Authorize(AuthenticationSchemes = "MyAuthentication")]
    [Authorize(Policy = "UserOnly")]
    [HttpGet("PairMe")]
    public ActionResult<GameRecordOut> PairMe()
    {
        var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
        var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
        var credentials = Encoding.UTF8.GetString(credentialBytes).Split(":");
        var username_wanted = credentials[0];
        if (_repository.GameRecordExist())
        {


            GameRecord to_be_updated = _repository.getRecord();
            string namer = to_be_updated.Player1;
            if (namer == username_wanted)
            {


                GameRecordOut output2 = new GameRecordOut
                {
                    gameId = to_be_updated.GameID,
                    state = to_be_updated.State,
                    player1 = to_be_updated.Player1,
                    player2 = to_be_updated.Player2,
                    lastMovePlayer1 = to_be_updated.LastMovePlayer1,
                    lastMovePlayer2 = to_be_updated.LastMovePlayer2
                };


                return Ok(output2);
            }



            to_be_updated.State = "progress";
            to_be_updated.Player2 = username_wanted;
            GameRecord record = to_be_updated;
            _repository.SaveChanges();
            GameRecordOut output = new GameRecordOut
            {
                gameId = record.GameID,
                state = record.State,
                player1 = record.Player1,
                player2 = record.Player2,
                lastMovePlayer1 = record.LastMovePlayer1,
                lastMovePlayer2 = record.LastMovePlayer2
            };
            return Ok(output);
        }

        else
        {

            GameRecord record = _repository.AddGameRecord(username_wanted);
            _repository.SaveChanges();
            GameRecordOut output = new GameRecordOut
            {
                gameId = record.GameID,
                state = record.State,
                player1 = record.Player1,
                player2 = record.Player2,
                lastMovePlayer1 = record.LastMovePlayer1,
                lastMovePlayer2 = record.LastMovePlayer2
            };
            return Ok(output);
        }








    }


    [Authorize(AuthenticationSchemes = "MyAuthentication")]
    [Authorize(Policy = "UserOnly")]
    [HttpGet("TheirMove/{gameId}")]
    public ActionResult<string> GetMove(string gameId)
    {
        var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
        var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
        var credentials = Encoding.UTF8.GetString(credentialBytes).Split(":");
        var username_wanted = credentials[0];
        Boolean linkPlayerToId = _repository.IsPlayerLinkedToGame(username_wanted, gameId);
        Boolean PlayerExists = _repository.IsPlayerInGame(username_wanted);

        if (_repository.GameRecordForMoveExist(gameId))
        {


            GameRecord wanted_record = _repository.GetRecordForMove(gameId);

            if (wanted_record.State == "wait") return Ok("You do not have an opponent yet.");


            else if (wanted_record.State == "progress" && wanted_record.Player1 == username_wanted && !(PlayerExists && !linkPlayerToId))
            {


                if (wanted_record.LastMovePlayer2 == null) return Ok("Your opponent has not moved yet.");

                else return Ok(wanted_record.LastMovePlayer2);


            }

            else if (wanted_record.State == "progress" && wanted_record.Player2 == username_wanted && !(PlayerExists && !linkPlayerToId))
            {



                if (wanted_record.LastMovePlayer1 == null) return Ok("Your opponent has not moved yet.");

                else return Ok(wanted_record.LastMovePlayer1);
            }


            else if (PlayerExists && !linkPlayerToId)
                return Ok("not your game id");


           
                return Ok("You have not started a game.");
        }

        else return Ok("no such gameId");

    }


    [Authorize(AuthenticationSchemes = "MyAuthentication")]
    [Authorize(Policy = "UserOnly")]
    [HttpPost("MyMove")]
    public ActionResult<string> MakeMyMove(GameMove new_move)
    {

        var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
        var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
        var credentials = Encoding.UTF8.GetString(credentialBytes).Split(":");
        var username_wanted = credentials[0];



        if (_repository.GameRecordForMoveExist(new_move.gameId))
        {


            GameRecord wanted_record = _repository.GetRecordForMove(new_move.gameId);

            if (wanted_record.State == "wait") return Ok("You do not have an opponent yet.");


            else if (wanted_record.State == "progress" && wanted_record.Player1 == username_wanted)
            {


                if (wanted_record.LastMovePlayer1 == null)
                {
                    wanted_record.LastMovePlayer1 = new_move.move;
                    wanted_record.LastMovePlayer2 = null;
                    _repository.SaveChanges();
                    return Ok("move registered");

                }

                else return Ok("It is not your turn");


            }

            else if (wanted_record.State == "progress" && wanted_record.Player2 == username_wanted)
            {


                if (wanted_record.LastMovePlayer2 == null)
                {
                    wanted_record.LastMovePlayer2 = new_move.move;
                    wanted_record.LastMovePlayer1 = null;
                    _repository.SaveChanges();
                    return Ok("move registered");

                }

                else return Ok("It is not your turn.");


            }


                return Ok("You have not started a game");
        }

        else return Ok("no such gameId");


    }



    [Authorize(AuthenticationSchemes = "MyAuthentication")]
    [Authorize(Policy = "UserOnly")]
    [HttpGet("QuitGame/{gameId}")]
    public ActionResult<string> QuitGame(string gameId)
    {
        var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
        var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
        var credentials = Encoding.UTF8.GetString(credentialBytes).Split(":");
        var username_wanted = credentials[0];
        Boolean linkPlayerToId = _repository.IsPlayerLinkedToGame(username_wanted,gameId);
        Boolean PlayerExists = _repository.IsPlayerInGame(username_wanted);
        if (_repository.GameRecordForMoveExist(gameId))
        {


            GameRecord wanted_record = _repository.GetRecordForMove(gameId);
            if (wanted_record.Player1 == username_wanted || wanted_record.Player2 == username_wanted)
            {

                _repository.DeleteGameRecord(gameId);
               
                return Ok("game over");


            }


            else if (PlayerExists && !linkPlayerToId)
                return Ok("not your game id");


            else if (!PlayerExists)
                return Ok("You have not started a game.");

            


        }




        return Ok("no such gameId");

    }

}
    




