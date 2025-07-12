using Hrms.Core.Utilities;

namespace Hrms.Core.Abstractions
{
    internal interface IEntityWithStatusTracking
    {
        Constants.RecordStatus Status { get; set; }
    }
}
