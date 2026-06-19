using BranchService.Contracts.Requests;
using MessagePack;
using Shouldly;

namespace BranchService.UnitTest.BranchService.Contracts.Tests.RequestTests;

public class CompanyRequestTest
{
    [Fact]
    public void CompanyRequest_ShouldSerializeAndDeserializeCorrectly()
    {
        var originalRequest = new CompanyRequest
        {
            RequestId = Guid.NewGuid(),
            CompanyId = 1,
            RequestedAt = DateTimeOffset.UtcNow
        };

        var bytes = MessagePackSerializer.Serialize(originalRequest);
        var deserializedRequest = MessagePackSerializer.Deserialize<CompanyRequest>(bytes);

        
        deserializedRequest.RequestId.ShouldBe(originalRequest.RequestId);
        deserializedRequest.CompanyId.ShouldBe(originalRequest.CompanyId);
        deserializedRequest.RequestedAt.ShouldBe(originalRequest.RequestedAt);
        
    }
}