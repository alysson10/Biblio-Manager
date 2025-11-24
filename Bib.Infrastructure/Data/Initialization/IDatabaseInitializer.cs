using System.Threading.Tasks;

namespace Bib.Infrastructure.Data.Initialization
{
    public interface IDatabaseInitializer
    {
        Task InitializeAsync();
    }
}
