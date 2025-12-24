using GraphQL;
using GraphQL.Types;
using lab5.Queries;

namespace lab5.Schemas
{
    public class AppSchema : Schema
    {
        public AppSchema(IServiceProvider provider) : base(provider)
        {
            Query = provider.GetRequiredService<StudentQuery>();
        }
    }
}