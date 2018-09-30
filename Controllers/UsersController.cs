using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using JobTracker.API.Data;
using JobTracker.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobTracker.API.Controllers
{

    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IMapper _mapper;
        private readonly MessageRepository _repo;
        public UsersController(MessageRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

                [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var userFromRepo = await _repo.GetUsers();

            var usersToReturn = _mapper.Map<IEnumerable<UserForDetailedDto>>(userFromRepo);

            return Ok(usersToReturn);
        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _repo.GetUser(id);

            var userToReturn = _mapper.Map<UserForDetailedDto>(user);

            return Ok(userToReturn);
        }

    }
}