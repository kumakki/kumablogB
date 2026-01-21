using kumablogB.Models;
using kumablogB.Common;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace kumablogB.Services
{
    public class AuthService
    {
        private readonly AppDbContext _db;

        public AuthService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<ServiceResult<string>> Login(string usernameOrEmail, string password)
        {
            ServiceResult<string> result = new();

            if (string.IsNullOrEmpty(usernameOrEmail))
            {
                result.Success = false;
                result.Error = "Username or email is required.";
            }

            Users? user = await _db.Users.FirstOrDefaultAsync(u => (u.UserName == usernameOrEmail || u.Email == usernameOrEmail) && u.DeletedAt == null);

            if (user == null)
            {
                result.Success = false;
                result.Error = "User not found.";
                return result;
            }

            string hashedPassword = HashPassword(password);

            if (user.Password != hashedPassword)
            {
                result.Success = false;
                result.Error = "Incorrect password.";
                return result;
            }

            string token = $"auth_{Guid.NewGuid().ToString()}";

            Sessions session = new()
            {
                AuthToken = token,
                UserId = user.UserId,
            };

            _db.Sessions.Add(session);

            await _db.SaveChangesAsync();

            result.Data = token;

            return result;
        }

        public string HashPassword(string password)
        {
            char[] chars = password.ToCharArray();
            double[] ritsu = [1.0, 1.25, 1.5, 1.75, 2.0, 2.25, 2.5, 2.75, 3.0];
            int[] keta = [2, 3, 3, 3, 4, 4, 4, 4, 5];
            string hashed = "";
            for (int i = 0; i < ritsu.Length; i++)
            {
                int sum = 0;
                foreach (char c in chars)
                {
                    sum += (int)Math.Pow((double)c, ritsu[i]);
                }
                hashed += sum.ToString("X").Substring(0, keta[i]);
            }
            return hashed;
        }
    }
}
