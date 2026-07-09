using BranchService.Contracts.Responses;
using MessagePack;
using Shouldly;

namespace BranchService.UnitTest.BranchService.Contracts.Tests.ResponseTests;

public class BranchConfigurationResponseTest
{
    [Fact]
    public void BranchConfigurationResponse_ShouldSerializeAndDeserializeCorrectly()
    {
        var originalResponse = new BranchConfigurationResponse
        {
            RequestId = Guid.NewGuid(),
            BranchId = 1,
            MaxTickets = 50,
            IsOpen = true,
            ErrorMessage = null
        };

        var bytes = MessagePackSerializer.Serialize(originalResponse);
        var deserializedResponse = MessagePackSerializer.Deserialize<BranchConfigurationResponse>(bytes);
        
        deserializedResponse.RequestId.ShouldBe(originalResponse.RequestId);
        deserializedResponse.BranchId.ShouldBe(originalResponse.BranchId);
        deserializedResponse.MaxTickets.ShouldBe(originalResponse.MaxTickets);
        deserializedResponse.IsOpen.ShouldBe(originalResponse.IsOpen);
        deserializedResponse.ErrorMessage.ShouldBe(originalResponse.ErrorMessage);
    }
}