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

    public async Task<ServiceResult<List<UserResult>>> GetAllUsersAsync()
    {
        ServiceResult<List<UserResult>> result = new();
        result.Data = new List<UserResult>();

        List<Users> users = await _db.Users.ToListAsync();
        
        if (users.Count == 0)
        {
            result.Success = false;
            result.Error = "No users found.";
        }

        foreach (Users u in users)
        {
            result.Data.Add(new UserResult
            {
                Id = u.Id,
                UserId = u.UserId,
                UserName = u.UserName,
                Email = u.Email,
                Profile = u.Profile
            });
        }

        return result;
    }

    public async Task<ServiceResult<UserResult>> GetUserByIdAsync(string id)
    {
        ServiceResult<UserResult> result = new();

        Users? user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
        {
            result.Success = false;
            result.Error = "User not found.";
            return result;
        }

        result.Data = new UserResult
        {
            Id = user.Id,
            UserId = user.UserId,
            UserName = user.UserName,
            Email = user.Email,
            Profile = user.Profile
        };

        return result;
    }

    public async Task<ServiceResult<UserResult>> GetUserByUserIdAsync(string id)
    {
        ServiceResult<UserResult> result = new();

        Users? user = await _db.Users.FirstOrDefaultAsync(u => u.UserId == id);

        if (user == null)
        {
            result.Success = false;
            result.Error = "User not found.";
            return result;
        }

        result.Data = new UserResult
        {
            Id = user.Id,
            UserId = user.UserId,
            UserName = user.UserName,
            Email = user.Email,
            Profile = user.Profile
        };

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