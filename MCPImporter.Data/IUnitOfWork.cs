using System.Threading.Tasks;

namespace MCPImporter.Data
{
    public interface IUnitOfWork
    {
        void Save();
        Task SaveAsync();
    }
}
