using Newtonsoft.Json.Linq;

namespace OsrStudio.Models
{
    public interface IRecentItemSerializer
    {
        bool CanSerialize(IRecentItem Item);

        bool CanDeserialize(JObject Item);

        JObject Serialize(IRecentItem Item);

        IRecentItem Deserialize(JObject Item);
    }
}