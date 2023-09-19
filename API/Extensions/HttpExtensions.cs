
using System.Text.Json;
using API.RequestHelpers;

namespace API.Extensions
{
    public static class HttpExtensions
    {
        public static void AddPaginationHeader (this HttpResponse response , PaginationData paginationData)
        {
           var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
           response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationData,options));
           response.Headers.Add("Access-Control-Expose-Headers","Pagination");
           

        }
    }
}