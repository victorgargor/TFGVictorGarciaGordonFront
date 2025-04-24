namespace BlazorAppVictor.Auth
{
    public interface ILoginService
    {
        Task Login(string token);
        Task Logout();
    }
}
