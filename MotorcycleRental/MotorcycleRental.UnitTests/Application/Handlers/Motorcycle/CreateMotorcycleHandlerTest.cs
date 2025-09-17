using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using MotorcycleRental.Application.Commands.DeliveryMan;
using MotorcycleRental.Application.Commands.Motorcycle;
using MotorcycleRental.Application.DTOs.Motorcycle;
using MotorcycleRental.Application.Handlers.Motorcycle;
using MotorcycleRental.Domain.Entities;
using MotorcycleRental.Infrastructure.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace MotorcycleRental.UnitTests.Application.Handlers.Motorcycle
{
    public class CommandHandlerTests
    {
        private bool ValidateModel(object model, out List<ValidationResult> results)
        {
            var context = new ValidationContext(model, null, null);
            results = new List<ValidationResult>();
            return Validator.TryValidateObject(model, context, results, true);
        }

        [Fact]
        public void CreateDeliveryManCommand_ValidData_ShouldBeValid()
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
        public void CreateDeliveryManCommand_InvalidCnhType_ShouldFailValidation()
        {
            var command = new CreateDeliveryManCommand
            {
                Id = "123",
                Name = "Lucas",
                Cnpj = "12345678000190",
                Birthday = new DateTime(1990, 1, 1),
                CnhNumber = "ABC12345",
                CnhType = "C",
                CnhImage = "base64string"
            };

            var isValid = ValidateModel(command, out var results);

            Assert.False(isValid);
            Assert.Contains(results, r => r.ErrorMessage.Contains("CNH deve ser A, B ou AB"));
        }

        [Fact]
        public void UpdateDeliveryManCommand_ValidCnhImage_ShouldBeValid()
        {
            var command = new UpdateDeliveryManCommand
            {
                Id = "123",
                CnhImage = "base64string"
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

        [Fact]
        public async Task CreateMotorcycleHandler_ShouldReturnDto_WhenDataIsValid()
        {
            var repositoryMock = new Mock<IMotorcycleRepository>();
            repositoryMock.Setup(r => r.ExistsByPlateAsync(It.IsAny<string>())).ReturnsAsync(false);
            repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((MotorcycleEntity)null);
            repositoryMock.Setup(r => r.AddAsync(It.IsAny<MotorcycleEntity>())).ReturnsAsync((MotorcycleEntity m) => m);

            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<CreateMotorcycleHandler>>();

            var handler = new CreateMotorcycleHandler(repositoryMock.Object, mediatorMock.Object, loggerMock.Object);

            var command = new CreateMotorcycleCommand(
                Id: "123",
                Year: 2020,
                Model: "Honda CG 160",
                Plate: "ABC1234"
            );

            MotorcycleDto result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(command.Id, result.Id);
            Assert.Equal(command.Year, result.Year);
            Assert.Equal(command.Model, result.Model);
            Assert.Equal(command.Plate, result.Plate);

            repositoryMock.Verify(r => r.AddAsync(It.IsAny<MotorcycleEntity>()), Times.Once);
            mediatorMock.Verify(m => m.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateMotorcycleHandler_ShouldThrowValidationException_WhenPlateTooLong()
        {
            var repositoryMock = new Mock<IMotorcycleRepository>();
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<CreateMotorcycleHandler>>();

            var handler = new CreateMotorcycleHandler(repositoryMock.Object, mediatorMock.Object, loggerMock.Object);

            var command = new CreateMotorcycleCommand(
                Id: "123",
                Year: 2020,
                Model: "Honda CG 160",
                Plate: "LONGPLATE12345"
            );

            await Assert.ThrowsAsync<ValidationException>(() =>
                handler.Handle(command, CancellationToken.None)
            );
        }

        [Fact]
        public async Task CreateMotorcycleHandler_ShouldThrowValidationException_WhenPlateAlreadyExists()
        {
            var repositoryMock = new Mock<IMotorcycleRepository>();
            repositoryMock.Setup(r => r.ExistsByPlateAsync(It.IsAny<string>())).ReturnsAsync(true);

            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<CreateMotorcycleHandler>>();

            var handler = new CreateMotorcycleHandler(repositoryMock.Object, mediatorMock.Object, loggerMock.Object);

            var command = new CreateMotorcycleCommand(
                Id: "123",
                Year: 2020,
                Model: "Honda CG 160",
                Plate: "ABC1234"
            );

            await Assert.ThrowsAsync<ValidationException>(() =>
                handler.Handle(command, CancellationToken.None)
            );
        }

        [Fact]
        public async Task CreateMotorcycleHandler_ShouldThrowValidationException_WhenIdAlreadyExists()
        {
            var repositoryMock = new Mock<IMotorcycleRepository>();
            repositoryMock.Setup(r => r.ExistsByPlateAsync(It.IsAny<string>())).ReturnsAsync(false);
            repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(new MotorcycleEntity());

            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<CreateMotorcycleHandler>>();

            var handler = new CreateMotorcycleHandler(repositoryMock.Object, mediatorMock.Object, loggerMock.Object);

            var command = new CreateMotorcycleCommand(
                Id: "123",
                Year: 2020,
                Model: "Honda CG 160",
                Plate: "ABC1234"
            );

            await Assert.ThrowsAsync<ValidationException>(() =>
                handler.Handle(command, CancellationToken.None)
            );
        }
    }
}
