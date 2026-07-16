namespace BranchService.Application.Requests;

public class UpdateCompanyServiceRequest
{
    public string ServiceName { get; set; }
    public string ServiceDescription { get; set; }
    public int ServiceDuration { get; set; }
}