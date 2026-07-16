using BranchService.Contracts.Events.Enums;
using MessagePack;

namespace BranchService.Contracts.Responses;

[MessagePackObject]
public class CompanyDetailsResponse
{
    [Key(0)] public int CompanyId { get; set; }

    [Key(1)] public string CompanyName { get; set; }

    [Key(2)] public CompanyCategory CompanyCategory { get; set; }

    [Key(3)] public string Address { get; set; }

    [Key(4)] public string EmailAddress { get; set; }

    [Key(5)] public string PhoneNumber { get; set; }
}