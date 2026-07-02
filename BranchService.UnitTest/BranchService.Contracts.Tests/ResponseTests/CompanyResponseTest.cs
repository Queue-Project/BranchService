using BranchService.Contracts.Responses;
using MessagePack;
using Shouldly;

namespace BranchService.UnitTest.BranchService.Contracts.Tests.ResponseTests;

public class CompanyResponseTest
{
    [Fact]
    public void CompanyResponse_ShouldSerializeAndDeserializeCorrectly()
    {
        var originalResponse = new CompanyResponse
        {
            RequestId = Guid.NewGuid(),
            CompanyId = 1,
            CompanyName = "Test Name",
            IsValid = true,
            ErrorMessage = null
        };

        var bytes = MessagePackSerializer.Serialize(originalResponse);
        var deserializedResponse = MessagePackSerializer.Deserialize<CompanyResponse>(bytes);
        
        deserializedResponse.RequestId.ShouldBe(originalResponse.RequestId);
        deserializedResponse.CompanyId.ShouldBe(originalResponse.CompanyId);
        deserializedResponse.CompanyName.ShouldBe(originalResponse.CompanyName);
        deserializedResponse.IsValid.ShouldBe(originalResponse.IsValid);
        deserializedResponse.ErrorMessage.ShouldBe(originalResponse.ErrorMessage);
        
    }
}