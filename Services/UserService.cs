using kumablogB.Models;
using Microsoft.EntityFrameworkCore;

namespace kumablogB.Services;

public class UserService
{
    private readonly AppDbContext _db;

    public UserService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Users>> GetAllUsersAsync()
    {
        return await _db.Users.ToListAsync();
    }
}