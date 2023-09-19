
using System.Reflection.Metadata.Ecma335;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly TokenService _tokenService;
        private readonly SouqContext _context;
        public AccountController(UserManager<User> userManager , TokenService tokenService, SouqContext context)
        {
            _context = context;
            _tokenService = tokenService;
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDto registerDto){
            var user = new User{UserName=registerDto.UserName, Email=registerDto.Email};
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if(!result.Succeeded){
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return ValidationProblem();
            }
            await _userManager.AddToRoleAsync(user,"Member");
            return StatusCode(201);
        }
          [HttpPost("login")]
          public async Task<ActionResult<UserDto>> Login(LoginDto loginDto){
            var user  = await _userManager.FindByNameAsync(loginDto.UserName);
            if(user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            return Unauthorized();
            var userBasket = await ReTriveBasket(loginDto.UserName);
            var anonBasket = await ReTriveBasket(Request.Cookies["buyerId"]);
            if(anonBasket!= null)
            {
                 if (userBasket!=null) _context.Baskets.Remove(userBasket);
                 anonBasket.BuyerId = user.UserName;
                 Response.Cookies.Delete("buyerId");
                 await _context.SaveChangesAsync();
            }
            return new UserDto{
                Email= user.Email,
                Token = await _tokenService.GenerateToken(user),
                Basket =anonBasket !=null ? anonBasket.MapBasketToDto() : userBasket?.MapBasketToDto()
            };
          }

          [Authorize]
          [HttpGet("currentUser")]
          public async Task<ActionResult<UserDto>> GrtCurrentUser(){
            var user =await _userManager.FindByNameAsync(User.Identity.Name);
            var userBasket = await ReTriveBasket(User.Identity.Name);
            return new UserDto{
                Email= user.Email,
                Token =await _tokenService.GenerateToken(user),
                Basket = userBasket?.MapBasketToDto()
            };
          }
            [Authorize]
            [HttpGet("savedAddress")]
            public async Task<ActionResult<UserAddress>> GetSavedAddress(){
                return await _userManager.Users
                .Where(u=>u.UserName==User.Identity.Name)
                .Select(user=>user.Address).FirstOrDefaultAsync();
            }
           private async Task<Basket> ReTriveBasket(string buyerId)
        {
            if(string.IsNullOrEmpty(buyerId)){
                Response.Cookies.Delete("buyerId");
                return null;
            }
            return await _context.Baskets
           .Include(b => b.Items)
           .ThenInclude(i => i.Product)
           .FirstOrDefaultAsync(b => b.BuyerId == buyerId);
        }

    }
}