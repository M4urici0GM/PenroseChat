using System.Threading.Tasks;
using Penrose.Application.Interfaces;

namespace Penrose.Application.Services
{
    public class HashingService : IHashingService
    {
        public Task<string> HashStringAsync(string value) =>
            Task.FromResult(BCrypt.Net.BCrypt.HashPassword(value));

        public Task<bool> CompareHashStringAsync(string hash, string value) =>
            Task.FromResult(BCrypt.Net.BCrypt.Verify(value, hash));

        public string HashString(string value) => BCrypt.Net.BCrypt.HashPassword(value);

        public bool CompareHashString(string hash, string value) => BCrypt.Net.BCrypt.Verify(value, hash);
    }
}