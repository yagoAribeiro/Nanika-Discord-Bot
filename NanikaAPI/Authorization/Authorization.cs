namespace NanikaAPI.Authorization
{

    public interface IAuthorization
    {
        public bool IsAuthenticated(IHttpContextAccessor httpContextAccessor);
    }
    public class Authorization : IAuthorization
    {
        public bool IsAuthenticated(IHttpContextAccessor httpContextAccessor)
        {
            return httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.Equals(httpContextAccessor.HttpContext.Connection.LocalIpAddress) ?? false;
        }
    }
}
