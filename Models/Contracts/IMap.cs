
namespace Models.Contracts
{
    public interface IMap<P, T>
    {
        P Map(T origin);
    }
}
