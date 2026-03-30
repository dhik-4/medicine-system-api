using MedicineSystemAPI.Models;

namespace MedicineSystemAPI.Interfaces
{
    public interface IUserRepository
    {
        Task<User> ValidateUsers(string userName, string Password, CancellationToken cancellationToken);
    }
}