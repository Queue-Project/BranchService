using MessagePack;

namespace BranchService.Contracts.Responses;

[MessagePackObject]
public class ServiceDetailsResponse
{
   [Key(0)] public int ServiceId { get; set; }
   [Key(1)] public int CompanyId { get; set; }
   [Key(2)] public int BranchId { get; set; }

   [Key(3)]public string ServiceName { get; set; } = string.Empty;
   [Key(4)] public string CompanyName { get; set; } = string.Empty;
   [Key(5)] public string BranchName { get; set; } = string.Empty;
    
   [Key(6)] public string? Description { get; set; }

   [Key(7)] public int ServiceDuration { get; set; }
}