using kumablogB.Models;
using kumablogB.Common;
using Microsoft.EntityFrameworkCore;

namespace kumablogB.Services;

public class UserService
{
    private readonly AppDbContext _db;
    private readonly AuthService _auth;

    public UserService(AppDbContext db, AuthService auth)
    {
        _db = db;
        _auth = auth;
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

    public async Task<ServiceResult<string>> CreateUserAsync(CreateUserRequest cur)
    {
        ServiceResult<string> result = new();
        Users? user = await _db.Users.FirstOrDefaultAsync(u => u.UserId == cur.UserId);

        if (user != null)
        {
            result.Success = false;
            result.Error = "UserId already exists.";
            return result;
        }

        user = _db.Users.FirstOrDefault(u => u.Email == cur.Email);

        if (user != null)
        {
            result.Success = false;
            result.Error = "Email already exists.";
            return result;
        }

        if (cur.Password.Length < 4)
        {
            result.Success = false;
            result.Error = "Password must be at least 4 characters long.";
            return result;
        }

        string newUserId = $"user_{Guid.NewGuid().ToString()}";

        string hashedPassword = await _auth.HashPassword(cur.Password);

        user = new Users
        {
            Id = newUserId,
            UserId = cur.UserId,
            UserName = cur.UserName,
            Email = cur.Email,
            Password = hashedPassword,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _db.Users.Add(user);

        await _db.SaveChangesAsync();

        result.Data = newUserId;

        return result;
    }

}