using Bib.WebAPI.Filters;

namespace Bib.WebAPI.Common.EndpointExtensions
{
    public static class EndpointExtensions
    {
        public static RouteHandlerBuilder ProducesGetResponse<T>(this RouteHandlerBuilder builder)
        {
            return builder
                .Produces<T>(200)
                .Produces(404)
                .Produces(500);
        }

        public static RouteHandlerBuilder ProducesPostResponse<T>(this RouteHandlerBuilder builder)
        {
            return builder
                .Produces<T>(201)
                .Produces(400)
                .Produces(500);
        }

        public static RouteHandlerBuilder ProducesPutResponse(this RouteHandlerBuilder builder)
        {
            return builder
                .Produces(204)
                .Produces(400)
                .Produces(404)
                .Produces(500);
        }

        public static RouteHandlerBuilder ProducesDeleteResponse(this RouteHandlerBuilder builder)
        {
            return builder
                .Produces(204)
                .Produces(400)
                .Produces(404)
                .Produces(500);
        }

        public static RouteGroupBuilder WithValidation<T>(this RouteGroupBuilder group) where T : class
        {
            group.AddEndpointFilter<ValidationFilter<T>>();
            return group;
        }
    }
}
