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

        public async Task<(ServiceResult<LoginResult>, string)> Login(string usernameOrEmail, string password)
        {
            ServiceResult<LoginResult> result = new();
            result.Data = new LoginResult();
            string errorMessage = "Validation failed";

            if (string.IsNullOrEmpty(usernameOrEmail))
            {
                result.Success = false;
                result.Error = errorMessage;
                result.Data.ErrorEmailOrUserId = "メールアドレスまたはユーザーIDが違います。";
                return (result, "");
            }

            Users? user = await _db.Users.FirstOrDefaultAsync(u => (u.UserName == usernameOrEmail || u.Email == usernameOrEmail) && u.DeletedAt == null);

            if (user == null)
            {
                result.Success = false;
                result.Error = errorMessage;
                result.Data.ErrorEmailOrUserId = "メールアドレスまたはユーザーIDが違います。";
                return (result, "");
            }

            string hashedPassword = await HashPassword(password);

            if (user.Password != hashedPassword)
            {
                result.Success = false;
                result.Error = errorMessage;
                result.Data.ErrorPassword = "パスワードが違います。";
                return (result, "");
            }

            string token = $"auth_{Guid.NewGuid().ToString()}";

            Sessions session = new()
            {
                AuthToken = token,
                UserId = user.Id,
            };

            _db.Sessions.Add(session);

            await _db.SaveChangesAsync();

            return (result, token);
        }

        public async Task<string> HashPassword(string password)
        {
            string hashed = "";
            await Task.Run(() =>
            {
                char[] chars = password.ToCharArray();
                double[] ritsu = [1.0, 1.25, 1.5, 1.75, 2.0, 2.25, 2.5, 2.75, 3.0];
                int[] keta = [2, 3, 3, 3, 4, 4, 4, 4, 5];
                for (int i = 0; i < ritsu.Length; i++)
                {
                    int sum = 0;
                    foreach (char c in chars)
                    {
                        sum += (int)Math.Pow((double)c, ritsu[i]);
                    }
                    hashed += sum.ToString("X").Substring(0, keta[i]);
                }
            });
            return hashed;
        }

        public async Task<ServiceResult<string>> CheckSession(string token)
        {
            ServiceResult<string> result = new();

            Sessions? session = await _db.Sessions.FirstOrDefaultAsync(s => s.AuthToken == token);

            if (session == null)
            {
                result.Success = false;
                result.Error = "Invalid token.";
                return result;
            }

            Users? users = await _db.Users.FirstOrDefaultAsync(u => u.Id == session.UserId && u.DeletedAt == null);

            if (users == null)
            {
                result.Success = false;
                result.Error = "User not found.";
                return result;
            }

            result.Data = users.Id;

            return result;
        }
            
    }
}
