using Microsoft.AspNetCore.Mvc;
using HW1.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HW1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        // GET: api/<GamesController>
        [HttpGet]
        public IEnumerable<Game> Get()
        {
            Game game = new Game(); 
            return game.Read(); // get the full games data
        }
        
        [HttpGet("usersGames/{userId}")]
        public IEnumerable<Game> GetUsersGames(int userId)
        {
            Game game = new Game(); 
            return Game.ReadUserGames(userId); // get the games wish list
        }

        [HttpGet("searchByPrice")] // using QueryString
        public IEnumerable<Game> GetByMinPrice(float minPrice, int userId)
        {
           Game game = new Game();
            return game.GetByMinPrice(minPrice, userId);
        }

        [HttpGet("searchByScore/score/{score}")] // using resource routing
        public IEnumerable<Game> GetByMinScore(int minScore, int userId)
        {
            Game game = new Game();
            return game.GetByMinScore(minScore, userId);
        }


        // POST api/<GamesController>
        [HttpPost("{UserId}")]
        public int Post([FromBody] int GameId, int UserId) 
        {
            Game game = new Game();
            return game.AddGame(GameId, UserId);
        }

        // PUT api/<GamesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<GamesController>/5
        [HttpDelete("deleteGame")]  // using QueryString
        public int DeleteById(int AppID,int userId)
        {
            Game game = new Game();
            return game.DeleteById(AppID, userId);
        }




    }
}
