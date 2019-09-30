using System.Threading.Tasks;

namespace TestAPI1.Services
{
    public interface IAutoTestWithLoadBalanceService
    {
        Task<string> ServiceCallAsync();
    }
}