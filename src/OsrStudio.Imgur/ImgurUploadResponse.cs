using Newtonsoft.Json;

namespace OsrStudio.Imgur
{
    class ImgurUploadResponse : ImgurResponse
    {
        [JsonProperty("data")]
        public ImgurData Data { get; set; }
    }
}