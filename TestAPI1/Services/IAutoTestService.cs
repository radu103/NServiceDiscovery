using System.Threading.Tasks;

namespace TestAPI1.Services
{
    public interface IAutoTestService
    {
        Task<string> ServiceCallAsync();
    }
}