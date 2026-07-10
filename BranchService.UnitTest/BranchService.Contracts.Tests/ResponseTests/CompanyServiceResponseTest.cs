using BranchService.Contracts.Responses;
using MessagePack;
using Shouldly;

namespace BranchService.UnitTest.BranchService.Contracts.Tests.ResponseTests;

public class CompanyServiceResponseTest
{
    [Fact]
    public void CompanyServiceResponse_ShouldSerializeAndDeserializeCorrectly()
    {
        var originalResponse = new CompanyServiceResponse
        {
            RequestId = Guid.NewGuid(),
            CompanyId = 1,
            BranchId = 1,
            CompanyServiceId = 1,
            CompanyServiceName = "Test Name",
            ServiceDuration = 45,
            IsValid = true,
            ErrorMessage = null
        };

        var bytes = MessagePackSerializer.Serialize(originalResponse);
        var deserializedResponse = MessagePackSerializer.Deserialize<CompanyServiceResponse>(bytes);
        
        deserializedResponse.RequestId.ShouldBe(originalResponse.RequestId);
        deserializedResponse.CompanyId.ShouldBe(originalResponse.CompanyId);
        deserializedResponse.BranchId.ShouldBe(originalResponse.BranchId);
        deserializedResponse.CompanyServiceId.ShouldBe(originalResponse.CompanyServiceId);
        deserializedResponse.CompanyServiceName.ShouldBe(originalResponse.CompanyServiceName);
        deserializedResponse.ServiceDuration.ShouldBe(originalResponse.ServiceDuration);
        deserializedResponse.IsValid.ShouldBe(originalResponse.IsValid);
        deserializedResponse.ErrorMessage.ShouldBe(originalResponse.ErrorMessage);
    }
}