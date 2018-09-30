using System.Collections.Generic;
using System.Linq;
using JobTracker.API.Models;
using Newtonsoft.Json;

namespace JobTracker.API.data
{
    public class Seed
    {
        private readonly DataContext _context;
        public Seed(DataContext context)
        {
            _context = context;
        }

        public void SeedUsers()
        {
           // _context.Users.RemoveRange(_context.Users);
           // _context.SaveChanges();

            if (!_context.Users.Any())
            {
            var userData = System.IO.File.ReadAllText("data/UserSeedData.json");
            var users = JsonConvert.DeserializeObject<List<User>>(userData);
            string password = null;
            foreach (var user in users)
            {
                if (user.Username == "boost")
                {
                    password = "!boost#";
                } else {
                    password = "lionlogin";
                }
                // create the password hash
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.Username = user.Username.ToLower();

                _context.Users.Add(user);
            }

            _context.SaveChanges();
            }

        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
