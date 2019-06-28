using Nest;

namespace DotNetCore.Interface
{
    public interface IElasticsearchService
    {
        ElasticClient GetClient();
    }
}
