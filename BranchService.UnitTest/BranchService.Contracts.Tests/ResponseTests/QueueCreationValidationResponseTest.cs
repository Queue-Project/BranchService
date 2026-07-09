using BranchService.Contracts.Responses;
using MessagePack;
using Shouldly;

namespace BranchService.UnitTest.BranchService.Contracts.Tests.ResponseTests;

public class QueueCreationValidationResponseTest
{
    [Fact]
    public void QueueCreationValidationResponse_ShouldSerializeAndDeserializeCorrectly()
    {
        var originalResponse = new QueueCreationValidationResponse()
        {
            RequestId = Guid.NewGuid(),
            IsWithinWorkingHours = true,
            WorkingHoursMessage = "Test Message",
            IsWithinBreakTime = false,
            BreakTimeMessage = "Test Break Time",
            MaxTicketsPerDay = 50,
            IsValid = true,
            ErrorMessage = null
        };

        var bytes = MessagePackSerializer.Serialize(originalResponse);
        var deserializedResponse = MessagePackSerializer.Deserialize<QueueCreationValidationResponse>(bytes);
        
        deserializedResponse.RequestId.ShouldBe(originalResponse.RequestId);
        deserializedResponse.IsWithinWorkingHours.ShouldBe(originalResponse.IsWithinWorkingHours);
        deserializedResponse.WorkingHoursMessage.ShouldBe(originalResponse.WorkingHoursMessage);
        deserializedResponse.IsWithinBreakTime.ShouldBe(originalResponse.IsWithinBreakTime);
        deserializedResponse.BreakTimeMessage.ShouldBe(originalResponse.BreakTimeMessage);
        deserializedResponse.MaxTicketsPerDay.ShouldBe(originalResponse.MaxTicketsPerDay);
        deserializedResponse.IsValid.ShouldBe(originalResponse.IsValid);
        deserializedResponse.ErrorMessage.ShouldBe(originalResponse.ErrorMessage);
    }
}