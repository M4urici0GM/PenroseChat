using System.Threading.Tasks;

namespace Penrose.Application.Interfaces
{
    public interface IHashingService
    {
        Task<string> HashStringAsync(string value);
        Task<bool> CompareHashStringAsync(string hash, string value);
        string HashString(string value);
        bool CompareHashString(string hash, string value);
    }
}