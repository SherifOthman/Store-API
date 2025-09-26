
namespace OnlineStore.Application.Common;
public class Constants
{
    private static int _imageSize = 2;
    public static int MAX_IMAGE_SIZE = _imageSize * 1024 * 1024;
    public static string IMAGE_VALIDATE_MESSAGE = $"Image must be less than {_imageSize}MB ";
}
