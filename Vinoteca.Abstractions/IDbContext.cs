namespace Vinoteca.Abstractions
{
    public interface IDbContext<T> : IDbOperation<T> where T : class
    {


    }
}
