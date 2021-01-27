using System.Threading.Tasks;

namespace BookLibrary.Client
{
    public interface ILibraryHttpDataClient
    {
        Task<T> GetData<T>(string url);

        Task PostData(string url, string json);
        
        Task PutData(string url, string id, string json);

        Task DeleteData(string url);
    }
}