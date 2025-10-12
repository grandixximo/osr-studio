using Newtonsoft.Json;

namespace OsrStudio.Imgur
{
    class ImgurResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }
    }
}