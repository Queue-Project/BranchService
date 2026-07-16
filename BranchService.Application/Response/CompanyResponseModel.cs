using BranchService.Domain.Enums;

namespace BranchService.Application.Response;

public class CompanyResponseModel
{
    public int Id { get; set; }
    public string CompanyName { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public string EmailAddress { get; set; }
    public CompanyCategory CompanyCategory { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}