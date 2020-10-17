using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public IEnumerable<UserDTO> Index()
        {
            return context.Users.Select(x => new UserDTO { Id = x.Id, Role = x.Role.Name, Name = x.Name, Email = x.Email });
        }
        [HttpGet]
        [Route("roles")]
        public IEnumerable<RoleDTO> Roles()
        {
            return context.Roles.Select(x=>new RoleDTO() { Id = x.Id, Name = x.Name });
        }

        [HttpGet]
        [Route("all")]
        public IEnumerable<Role> All()
        {
            return context.Roles.Include(x=>x.Users).ToList();
        }

        [HttpPost]
        public IActionResult Add([FromBody] UserDTO user)
        {
            if (user != null)
            {
                context.Users.Add(new User { 
                    Id = user.Id,
                    Email = user.Email,
                    Name = user.Name,
                    Role =  context.Roles.FirstOrDefault(x=>x.Name == user.Role)});
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
            res.Role = context.Roles.FirstOrDefault(x => x.Name == user.Role);

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
                return Ok(new UserDTO { Id = user.Id, Name = user.Name, Email = user.Email, Role = user.Role.Name });
            }
            return NotFound();
        }
    }
}
