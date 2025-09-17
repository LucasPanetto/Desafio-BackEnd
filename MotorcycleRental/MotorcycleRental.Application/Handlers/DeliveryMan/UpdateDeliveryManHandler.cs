using MediatR;
using MotorcycleRental.Application.Commands.DeliveryMan;
using MotorcycleRental.Application.DTOs.DeliveryMan;
using MotorcycleRental.Infrastructure.Interfaces;
using MotorcycleRental.Infrastructure.Utils;
using System.ComponentModel.DataAnnotations;

namespace MotorcycleRental.Application.Handlers.DeliveryMan
{
    public class UpdateDeliveryManHandler : IRequestHandler<UpdateDeliveryManCommand, UpdateDeliveryManDto>
    {
        private readonly IDeliveryManRepository _repository;

        public UpdateDeliveryManHandler(IDeliveryManRepository repository)
        {
            _repository = repository;
        }

        public async Task<UpdateDeliveryManDto> Handle(UpdateDeliveryManCommand request, CancellationToken cancellationToken)
        {
            var deliveryMan = await _repository.GetByIdAsync(request.Id);
            if (deliveryMan == null)
                throw new KeyNotFoundException($"Entregador com Id {request.Id} não encontrado.");

            if (!Base64FileHelper.Base64FileExtensionIsPngOrBmp(request.CnhImage))
                throw new ValidationException("Formato da imagem (CNH) inválido.");

            byte[] imageBytes;
            try
            {
                imageBytes = Convert.FromBase64String(request.CnhImage);
            }
            catch
            {
                throw new ValidationException("Imagem da CNH inválida (base64 incorreto).");
            }

            var filePath = await Base64FileHelper.SaveFile(deliveryMan.Id, imageBytes, cancellationToken);
            deliveryMan.CnhImagePath = filePath;

            await _repository.UpdateAsync(deliveryMan);

            return new UpdateDeliveryManDto(
                deliveryMan.InternalId,
                deliveryMan.Id,
                deliveryMan.Name,
                deliveryMan.Cnpj,
                deliveryMan.Birthday,
                deliveryMan.CnhNumber,
                deliveryMan.CnhType,
                request.CnhImage
            );
        }
    }
}
