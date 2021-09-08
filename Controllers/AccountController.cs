using CycleUpAPI.Entities;
using CycleUpAPI.Identity;
using CycleUpAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CycleUpAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly CycleContext cycleContext;
        private readonly IPasswordHasher<User> passwordHasher;
        private readonly IJwtProvider jwtProvider;

        public AccountController(CycleContext cycleContext, IPasswordHasher<User> passwordHasher, IJwtProvider jwtProvider)
        {
            this.cycleContext = cycleContext;
            this.passwordHasher = passwordHasher;
            this.jwtProvider = jwtProvider;
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody]UserLoginDTO userLoginDTO)
        {
            var user = cycleContext.Users
                .Include(user => user.Role)
                .FirstOrDefault(user => user.Email == userLoginDTO.Email);

            if (user == null)
            {
                return BadRequest("Invalid Username or Password");
            }

            var passwordVerificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, userLoginDTO.Password);

            if(passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                return BadRequest("Invalid username or password");
            }

            var token = jwtProvider.GenerateJwtToken(user);

            return Ok(token);
        }


        [HttpPost("register")]
        public ActionResult Register([FromBody]RegisterUserDTO registerUserDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newUser = new User()
            {
                Email = registerUserDTO.Email,
                DateOfBirth = registerUserDTO.DateOfBirth,
                RoleId = registerUserDTO.RoleId
            };

            var passwordHash = passwordHasher.HashPassword(newUser, registerUserDTO.Password);
            newUser.PasswordHash = passwordHash;
            
            cycleContext.Users.Add(newUser);
            cycleContext.SaveChanges();

            return Ok();
        }
    }
}
