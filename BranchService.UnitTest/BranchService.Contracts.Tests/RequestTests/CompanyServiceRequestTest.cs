using BranchService.Contracts.Requests;
using MessagePack;
using Shouldly;

namespace BranchService.UnitTest.BranchService.Contracts.Tests.RequestTests;

public class CompanyServiceRequestTest
{
    [Fact]
    public void CompanyServiceRequest_ShouldSerializeAndDeserializeCorrectly()
    {
        var originalRequest = new CompanyServiceRequest
        {
            RequestId = Guid.NewGuid(),
            CompanyId = 1,
            CompanyServiceId = 1,
            RequestedAt = DateTimeOffset.UtcNow
        };

        var bytes = MessagePackSerializer.Serialize(originalRequest);
        var deserializedRequest = MessagePackSerializer.Deserialize<CompanyServiceRequest>(bytes);
        
        deserializedRequest.RequestId.ShouldBe(originalRequest.RequestId);
        deserializedRequest.CompanyId.ShouldBe(originalRequest.CompanyId);
        deserializedRequest.CompanyServiceId.ShouldBe(originalRequest.CompanyServiceId);
        deserializedRequest.RequestedAt.ShouldBe(originalRequest.RequestedAt);
    }
}