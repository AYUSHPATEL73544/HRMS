using Hrms.Core.Abstractions;

namespace Hrms.Core.Entities
{
    public class Entity<T> : IEntity<T> 
    {
        public T Id { get; set; }
    }
}
