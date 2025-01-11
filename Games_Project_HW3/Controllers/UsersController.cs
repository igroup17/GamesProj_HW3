using Microsoft.AspNetCore.Mvc;
using HW1.Models;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HW1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
       

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UsersController>
        [HttpPost("register")]
        public int PostRegister([FromBody] User user)
        {
            return user.Insert();
        }

        // POST api/<UsersController>
        [HttpPost("login")]
        public User PostLogin([FromBody] User user)
        {
            return user.LogIn();
        }


        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpPut("updateUser")]
        public User PUT([FromBody] User user)
        {
            return user.UpdateUserRequest();
        }
    }
}
