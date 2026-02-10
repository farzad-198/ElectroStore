using System.Security.Claims;

namespace ElectroStore.Extensions
{
    public static class UserExtensions
    {
        public static string? GetUserId(this ClaimsPrincipal user)
        {

            return user.FindFirstValue(ClaimTypes.NameIdentifier);

        }
    }
}
