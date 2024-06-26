﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EventSuite.Core.DTOs.Requests.User;
using EventSuite.Core.DTOs.Responses.User;
using EventSuite.Core.Models;
using EventSuite.BLL.Services.Implementations;
using EventSuite.BLL.Services.Interfaces;

namespace EventSuite.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _usersService;
        private readonly Serilog.ILogger _logger;

        public UserController(IMapper mapper, IUserService usersService, Serilog.ILogger logger)
        {
            _mapper = mapper;
            _usersService = usersService;
            _logger = logger;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("users")]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _usersService.GetUsersAsync();
            if (!result.Any() || result == null)
            {
                _logger.Error("No users found");
                return NotFound("No users found");
            }
            List<UserResponse> users = new ();
            foreach (var user in result)
            {
                var currentUser = _mapper.Map<UserResponse>(user);
                currentUser.Role = await _usersService.GetRole(user);
                users.Add(currentUser);
            }
            return Ok(users);
        }

        [Authorize]
        [HttpGet("{userName}")]
        public async Task<IActionResult> GetUserByName([FromRoute] string userName)
        {
            var result = await _usersService.GetUserByNameAsync(userName);
            if (result is null)
            {
                _logger.Error($"User with username:{userName} not found");
                return NotFound($"User with username:{userName} not found");
            }
            var user = _mapper.Map<UserResponse>(result);
            user.Role = await _usersService.GetRole(result);
            return Ok(user);
        }

        [Authorize]
        [HttpDelete("{userName}")]
        public async Task<IActionResult> DeleteUser([FromRoute] string userName)
        {
            var result = await _usersService.DeleteUserByNameAsync(userName);
            if (result is null)
            {
                _logger.Error($"User with username:{userName} not found");
                return NotFound($"User with username:{userName} not found");
            }
            if (!result.Succeeded)
            {
                _logger.Error($"User with username:{userName} not deleted");
                return BadRequest(result);
            }
            return NoContent();
        }

        [Authorize]
        [HttpPut("{username}")]
        public async Task<IActionResult> UpdateUser([FromRoute] string username, [FromBody] UpdateUserRequest updateUserRequest)
        {
            if (!ModelState.IsValid)
            {
                _logger.Error($"Invalid user request");
                return BadRequest(ModelState);
            }
            var user = _mapper.Map<User>(updateUserRequest);
            var result = await _usersService.UpdateUserAsync(username, user);
            if (result is null)
            {
                _logger.Error($"User with username:{username} not found");
                return NotFound($"User with username:{username} not found");
            }
            if (!result.Succeeded)
            {
                _logger.Error($"User with username:{username} not updated");
                return BadRequest(result);
            }
            return Ok(_mapper.Map<UserResponse>(_usersService.GetUserByNameAsync(updateUserRequest.Email).Result));
        }
    }
}
