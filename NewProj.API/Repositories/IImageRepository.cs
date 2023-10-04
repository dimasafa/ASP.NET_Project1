using NewProj.API.Models.Domain;

namespace NewProj.API.Repositories
{
    public interface IImageRepository
    {
        Task<Image> Upload(Image image);
    }
}
