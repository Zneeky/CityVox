namespace CityVoxWeb.API.Middleware
{
    public class JwtTokenCookieMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtTokenCookieMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var jwtToken = context.Request.Cookies["jwtToken"];
            if (jwtToken != null)
            {
                context.Request.Headers.Append("Authorization", "Bearer " + jwtToken);
            }

            await _next(context);
        }
    }
}
