namespace Hrms.Core.Abstractions
{
    public interface IEntityWithUserTracking
    {
        int CreatedById { get; set; }

        int? UpdatedById { get; set; }
    }
}
