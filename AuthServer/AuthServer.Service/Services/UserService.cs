using AuthServer.Core.Dtos;
using AuthServer.Core.Models;
using AuthServer.Core.Services;
using AuthServer.Service.Mapper;
using Microsoft.AspNetCore.Identity;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<UserApp> _userManager;

        public UserService(UserManager<UserApp> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Response<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto)
        {
            var user = new UserApp() {Email = createUserDto.Email, UserName = createUserDto.UserName };

            var result = await _userManager.CreateAsync(user, createUserDto.Password);

            if (!result.Succeeded)
            {
                var error = result.Errors.Select(x => x.Description).ToList();

                return Response<UserAppDto>.Fail(new ErrorDto(error, true), 404);
            }

            return Response<UserAppDto>.Success(ObjectMapper.Mapper.Map<UserAppDto>(user),200);
        }

        public async Task<Response<UserAppDto>> GetUserByNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return Response<UserAppDto>.Fail("UserName not Found", 404, true);
            }

            return Response<UserAppDto>.Success(ObjectMapper.Mapper.Map<UserAppDto>(user), 200);
        }
    }
}
