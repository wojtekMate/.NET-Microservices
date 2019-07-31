namespace Mikro.Api.Repositories
{
    public interface IRepository
    {
        int? Get(int number);
        void Insert(int number, int result);
    }
}