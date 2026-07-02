using BranchService.Contracts.Requests;
using MessagePack;
using Shouldly;

namespace BranchService.UnitTest.BranchService.Contracts.Tests.RequestTests;

public class BranchRequestTest
{
    [Fact]
    public void BranchRequest_ShouldSerializeAndDeserializeCorrectly()
    {
        var originalRequest = new BranchRequest
        {
            RequestId = Guid.NewGuid(),
            CompanyId = 1,
            BranchId = 1,
            RequestedAt = DateTimeOffset.UtcNow
        };

        var bytes = MessagePackSerializer.Serialize(originalRequest);
        var deserializedRequest = MessagePackSerializer.Deserialize<BranchRequest>(bytes);

        
        deserializedRequest.RequestId.ShouldBe(originalRequest.RequestId);
        deserializedRequest.CompanyId.ShouldBe(originalRequest.CompanyId);
        deserializedRequest.BranchId.ShouldBe(originalRequest.BranchId);
        deserializedRequest.RequestedAt.ShouldBe(originalRequest.RequestedAt);
        
    }
}