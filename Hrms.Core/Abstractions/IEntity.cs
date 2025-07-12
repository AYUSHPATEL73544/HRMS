namespace Hrms.Core.Abstractions
{
    public interface IEntity<T>
    {
        T Id { get; set; }
    }
}
