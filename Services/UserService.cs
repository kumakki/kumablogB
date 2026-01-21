using kumablogB.Models;
using kumablogB.Common;
using Microsoft.EntityFrameworkCore;

namespace kumablogB.Services;

public class UserService
{
    private readonly AppDbContext _db;

    public UserService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<ServiceResult<List<Users>>> GetAllUsersAsync()
    {
        ServiceResult<List<Users>> result = new()
        {
            Data = await _db.Users.ToListAsync()
        };

        if (result.Data.Count == 0)
        {
            result.Success = false;
            result.Error = "No users found.";
        }

        return result;
    }

    public async Task<ServiceResult<Users>> GetUserByIdAsync(string id)
    {
        ServiceResult<Users> result = new()
        {
            Data = await _db.Users.FirstOrDefaultAsync(u => u.Id == id)
        };

        if (result.Data == null)
        {
            result.Success = false;
            result.Error = "User not found.";
        }

        return result;
    }

    public async Task<ServiceResult<Users>> GetUserByUserIdAsync(string id)
    {
        ServiceResult<Users> result = new()
        {
            Data = await _db.Users.FirstOrDefaultAsync(u => u.UserId == id)
        };

        if (result.Data == null)
        {
            result.Success = false;
            result.Error = "User not found.";
        }

        return result;
    }

}