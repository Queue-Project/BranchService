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
            CompanyServiceId = 1,
            CompanyServiceName = "Test Name",
            IsValid = true,
            ErrorMessage = null
        };

        var bytes = MessagePackSerializer.Serialize(originalResponse);
        var deserializedResponse = MessagePackSerializer.Deserialize<CompanyServiceResponse>(bytes);
        
        deserializedResponse.RequestId.ShouldBe(originalResponse.RequestId);
        deserializedResponse.CompanyId.ShouldBe(originalResponse.CompanyId);
        deserializedResponse.CompanyServiceId.ShouldBe(originalResponse.CompanyServiceId);
        deserializedResponse.CompanyServiceName.ShouldBe(originalResponse.CompanyServiceName);
        deserializedResponse.IsValid.ShouldBe(originalResponse.IsValid);
        deserializedResponse.ErrorMessage.ShouldBe(originalResponse.ErrorMessage);
    }
}