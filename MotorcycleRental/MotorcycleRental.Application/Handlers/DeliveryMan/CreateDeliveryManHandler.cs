using MediatR;
using MotorcycleRental.Application.Commands.DeliveryMan;
using MotorcycleRental.Application.DTOs.DeliveryMan;
using MotorcycleRental.Domain.Entities;
using MotorcycleRental.Infrastructure.Interfaces;
using MotorcycleRental.Infrastructure.Utils;
using System.ComponentModel.DataAnnotations;

namespace MotorcycleRental.Application.Handlers.DeliveryMan
{
    public class CreateDeliveryManHandler : IRequestHandler<CreateDeliveryManCommand, CreateDeliveryManDto>
    {
        private readonly IDeliveryManRepository _repository;

        public CreateDeliveryManHandler(IDeliveryManRepository repository)
        {
            _repository = repository;
        }

        public async Task<CreateDeliveryManDto> Handle(CreateDeliveryManCommand request, CancellationToken cancellationToken)
        {
            if (await _repository.ExistsByCnpjAsync(request.Cnpj))
                throw new InvalidOperationException($"Já existe um entregador cadastrado com o CNPJ {request.Cnpj}.");

            if (await _repository.ExistsByCnhNumberAsync(request.CnhNumber))
                throw new InvalidOperationException($"Já existe um entregador cadastrado com a CNH {request.CnhNumber}.");

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

            var filePath = await Base64FileHelper.SaveFile(request.Id, imageBytes, cancellationToken);

            var deliveryMan = new DeliveryManEntity
            {
                Id = request.Id,
                Name = request.Name,
                Cnpj = request.Cnpj,
                Birthday = request.Birthday,
                CnhNumber = request.CnhNumber,
                CnhType = request.CnhType,
                CnhImagePath = filePath
            };

            var deliveryManSaved = await _repository.AddAsync(deliveryMan);

            return new CreateDeliveryManDto(
                deliveryManSaved.InternalId,
                deliveryMan.Id,
                deliveryMan.Name,
                deliveryMan.Cnpj,
                deliveryMan.Birthday,
                deliveryMan.CnhNumber,
                deliveryMan.CnhType,
                filePath // ou request.CnhImage, dependendo da sua decisão
            );
        }


    }
}
