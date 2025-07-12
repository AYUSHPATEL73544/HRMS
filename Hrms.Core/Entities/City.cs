namespace Hrms.Core.Entities
{
    public class City : EntityWithStatusTracking<int>
    {
        public string Name { get; set; }
        public int StateId { get; set; }
    }
}
