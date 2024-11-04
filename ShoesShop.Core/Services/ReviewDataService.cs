using ShoesShop.Core.Contracts.Services;
using ShoesShop.Core.Models;

namespace ShoesShop.Core.Services;
public class ReviewDataService : IReviewDataService
{
    //private readonly IReviewRepository _reviewRepository;
    private (IEnumerable<Review>, int, string, int) _reviewDataTuple;

    public bool IsInitialized => _reviewDataTuple.Item1 is not null;

    public string ShoesId
    {
        get; set;
    }

    public ReviewDataService(/*IReviewRepository reviewRepository*/)
    {
        //_reviewRepository = reviewRepository;
    }

    public (IEnumerable<Review>, int, string, int) GetData() => _reviewDataTuple;
    public async Task<(IEnumerable<Review>, int, string, int)> LoadDataAsync()
    {
        //_reviewDataTuple = await _reviewRepository.GetAllReviewsAsync(ShoesId);

        //return _reviewDataTuple;
        return (null, 0, string.Empty, 0);
    }
}
