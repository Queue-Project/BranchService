using BranchService.Contracts.Responses;
using MessagePack;
using Shouldly;

namespace BranchService.UnitTest.BranchService.Contracts.Tests.ResponseTests;

public class BranchResponseTest
{
    [Fact]
    public void BranchResponse_ShouldSerializeAndDeserializeCorrectly()
    {
        var originalResponse = new BranchResponse
        {
            RequestId = Guid.NewGuid(),
            CompanyId = 1,
            BranchId = 1,
            BranchName = "Test Name",
            IsValid = true,
            ErrorMessage = null
        };

        var bytes = MessagePackSerializer.Serialize(originalResponse);
        var deserializedResponse = MessagePackSerializer.Deserialize<BranchResponse>(bytes);
        
        deserializedResponse.RequestId.ShouldBe(originalResponse.RequestId);
        deserializedResponse.CompanyId.ShouldBe(originalResponse.CompanyId);
        deserializedResponse.BranchId.ShouldBe(originalResponse.BranchId);
        deserializedResponse.BranchName.ShouldBe(originalResponse.BranchName);
        deserializedResponse.IsValid.ShouldBe(originalResponse.IsValid);
        deserializedResponse.ErrorMessage.ShouldBe(originalResponse.ErrorMessage);
    }
}