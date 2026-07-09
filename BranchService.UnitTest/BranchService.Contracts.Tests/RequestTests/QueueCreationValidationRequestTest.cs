using BranchService.Contracts.Requests;
using MessagePack;
using Shouldly;

namespace BranchService.UnitTest.BranchService.Contracts.Tests.RequestTests;

public class QueueCreationValidationRequestTest
{
    [Fact]
    public void QueueCreationValidationRequest_ShouldSerializeAndDeserializeCorrectly()
    {
        var originalRequest = new QueueCreationValidationRequest
        {
            RequestId = Guid.NewGuid(),
            BranchId = 1,
            RequestedStartTime = DateTimeOffset.UtcNow
        };

        var bytes = MessagePackSerializer.Serialize(originalRequest);
        var deserializedRequest = MessagePackSerializer.Deserialize<QueueCreationValidationRequest>(bytes);
        
        deserializedRequest.RequestId.ShouldBe(originalRequest.RequestId);
        deserializedRequest.BranchId.ShouldBe(originalRequest.BranchId);
        deserializedRequest.RequestedStartTime.ShouldBe(originalRequest.RequestedStartTime);
    }
}