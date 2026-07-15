using BranchService.Contracts.Requests;
using BranchService.Contracts.Responses;
using MagicOnion;

namespace BranchService.Contracts.Interfaces;

public interface IBranchService: IService<IBranchService>
{
    UnaryResult<BranchResponse> CheckBranchId(BranchRequest request);
    UnaryResult<CompanyResponse> CheckCompanyId(CompanyRequest request);
    UnaryResult<CompanyServiceResponse> CheckCompanyServiceId(CompanyServiceRequest request);
    UnaryResult<QueueCreationValidationResponse> ValidateQueueCreationAsync(QueueCreationValidationRequest request);

    UnaryResult<List<BranchResponse>> GetCompanyBranches(int companyId);
    UnaryResult<List<CompanyServiceResponse>> GetCompanyServices(int companyId);
    UnaryResult<List<CompanyDetailsResponse>> GetCompanyDetails(List<int> companyIds);

    UnaryResult<List<BranchDetailsResponse>> GetBranchDetails(List<int> branchIds);

    UnaryResult<List<ServiceDetailsResponse>> GetServiceDetails(List<int> serviceIds);

}