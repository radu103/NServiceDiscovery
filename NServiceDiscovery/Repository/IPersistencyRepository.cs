
namespace NServiceDiscovery.Repository
{
    public interface IPersistencyRepository
    {
        bool LoadDataFromPersistency();

        bool SaveDataFromPersistency();
    }
}