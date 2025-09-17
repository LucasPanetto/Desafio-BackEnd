namespace MotorcycleRental.Infrastructure.Utils
{
    public static class Base64FileHelper
    {
        public static bool Base64FileExtensionIsPngOrBmp(string base64)
        {
            var bytes = Convert.FromBase64String(base64);

            // PNG
            if (bytes.Take(8).SequenceEqual(new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 }))
                return true;

            // BMP
            if (bytes.Take(2).SequenceEqual(new byte[] { 66, 77 })) // "BM"
                return true;

            // Se não for PNG ou BMP, retorna null
            return false;
        }

        public async static Task<string> SaveFile(string id, byte[] imageBytes, CancellationToken cancellationToken)
        {
            var fileName = $"cnh_{id}_{Guid.NewGuid()}.png";

            // Caminho temporário
            var tempDir = Path.Combine(Path.GetTempPath(), "DeliveryManCNH");
            if (!Directory.Exists(tempDir))
                Directory.CreateDirectory(tempDir);

            var filePath = Path.Combine(tempDir, fileName);

            await File.WriteAllBytesAsync(filePath, imageBytes, cancellationToken);

            return filePath;
        }
    }
}
