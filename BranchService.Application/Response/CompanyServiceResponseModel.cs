namespace BranchService.Application.Response;

public class CompanyServiceResponseModel
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public int BranchId { get; set; }
    public string ServiceName { get; set; }
    public string ServiceDescription { get; set; }
    public int ServiceDuration { get; set; }
}