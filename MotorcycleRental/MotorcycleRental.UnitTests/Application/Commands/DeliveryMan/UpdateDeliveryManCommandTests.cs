using MotorcycleRental.Application.Commands.DeliveryMan;
using System.ComponentModel.DataAnnotations;

public class UpdateDeliveryManCommandTests
{
    private bool ValidateModel(object model, out List<ValidationResult> results)
    {
        var context = new ValidationContext(model, null, null);
        results = new List<ValidationResult>();
        return Validator.TryValidateObject(model, context, results, true);
    }

    [Fact]
    public void UpdateDeliveryManCommand_WithValidCnhImage_ShouldBeValid()
    {
        var command = new UpdateDeliveryManCommand
        {
            CnhImage = "base64string",
            Id = "123"
        };

        var isValid = ValidateModel(command, out var results);

        Assert.True(isValid);
        Assert.Empty(results);
    }

    [Fact]
    public void UpdateDeliveryManCommand_WithoutCnhImage_ShouldFailValidation()
    {
        var command = new UpdateDeliveryManCommand
        {
            Id = "123"
        };

        var isValid = ValidateModel(command, out var results);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains("CnhImage"));
    }
}
