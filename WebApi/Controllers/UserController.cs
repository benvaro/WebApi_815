using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WebApi.Entities;
using WebApi.Model;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly ApplicationContext context;
        public UserController(ApplicationContext _context)
        {
            context = _context;
        }

        [HttpGet]
        public IEnumerable<User> Index()
        {
            return context.Users.ToList();
        }

        [HttpPost]
        public IActionResult Add([FromBody] UserDTO user)
        {
            if (user != null)
            {
                context.Users.Add(new User { Id = user.Id, Email = user.Email, Name = user.Name });
                context.SaveChanges();
                return Ok();
            }
            else
                return BadRequest();
        }

        [HttpPut]
        public IActionResult Update(int id, UserDTO user)
        {
            var res = context.Users.FirstOrDefault(x => x.Id == id);
            res.Name = user.Name;
            res.Email = user.Email;

            context.Update(res);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(int id)
        {
            User user = null;
            if (id <= context.Users.Count())
            {
                user = context.Users.Find(id);
                return Ok(new UserDTO { Id = user.Id, Name = user.Name, Email = user.Email });
            }
            return NotFound();
        }
    }
}
