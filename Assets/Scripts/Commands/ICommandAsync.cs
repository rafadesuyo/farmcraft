using System.Threading.Tasks;

public interface ICommandAsync<T>
{
    Task<T> Execute();
}