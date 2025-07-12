namespace Hrms.Core.Abstractions
{
    public interface IEntityWithTimeTracking
    {
        DateTime CreatedOn { get; set; }

        DateTime? UpdatedOn { get; set; }

        DateTime EffectiveFrom { get; set; }
        
        DateTime? EffectiveTo { get; set; }
    }
}
