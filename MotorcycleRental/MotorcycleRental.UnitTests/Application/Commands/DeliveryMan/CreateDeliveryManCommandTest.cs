using MotorcycleRental.Application.Commands.DeliveryMan;
using System.ComponentModel.DataAnnotations;

public class CreateDeliveryManCommandTests
{
    private bool ValidateModel(object model, out List<ValidationResult> results)
    {
        var context = new ValidationContext(model, null, null);
        results = new List<ValidationResult>();
        return Validator.TryValidateObject(model, context, results, true);
    }

    [Fact]
    public void CreateDeliveryManCommand_WithValidData_ShouldBeValid()
    {
        var command = new CreateDeliveryManCommand
        {
            Id = "123",
            Name = "Lucas",
            Cnpj = "12345678000190",
            Birthday = new DateTime(1990, 1, 1),
            CnhNumber = "ABC12345",
            CnhType = "B",
            CnhImage = "base64string"
        };

        var isValid = ValidateModel(command, out var results);

        Assert.True(isValid);
        Assert.Empty(results);
    }

    [Fact]
    public void CreateDeliveryManCommand_WithInvalidCnhType_ShouldFailValidation()
    {
        var command = new CreateDeliveryManCommand
        {
            Id = "123",
            Name = "Lucas",
            Cnpj = "12345678000190",
            Birthday = new DateTime(1990, 1, 1),
            CnhNumber = "ABC12345",
            CnhType = "C", // inválido
            CnhImage = "base64string"
        };

        var isValid = ValidateModel(command, out var results);

        Assert.False(isValid);
        Assert.Contains(results, r => r.ErrorMessage.Contains("CNH deve ser A, B ou AB"));
    }
}
