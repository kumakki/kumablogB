namespace kumablogB.Services
{
    public class SessionService
    {
        private readonly AppDbContext _db;

        public SessionService(AppDbContext db)
        {
            _db = db;
        }


    }
}
