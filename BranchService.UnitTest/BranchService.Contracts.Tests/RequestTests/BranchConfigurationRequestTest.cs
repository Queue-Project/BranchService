using BranchService.Contracts.Requests;
using MessagePack;
using Shouldly;

namespace BranchService.UnitTest.BranchService.Contracts.Tests.RequestTests;

public class BranchConfigurationRequestTest
{
    [Fact]
    public void BranchRequest_ShouldSerializeAndDeserializeCorrectly()
    {
        var originalRequest = new BranchConfigurationRequest()
        {
            RequestId = Guid.NewGuid(),
            BranchId = 1,
            ServiceId = 1,
            StartTime = DateTimeOffset.UtcNow
        };

        var bytes = MessagePackSerializer.Serialize(originalRequest);
        var deserializedRequest = MessagePackSerializer.Deserialize<BranchConfigurationRequest>(bytes);
        
        
        deserializedRequest.RequestId.ShouldBe(originalRequest.RequestId);
        deserializedRequest.BranchId.ShouldBe(originalRequest.BranchId);
        deserializedRequest.ServiceId.ShouldBe(originalRequest.ServiceId);
        deserializedRequest.StartTime.ShouldBe(originalRequest.StartTime);
    }
}