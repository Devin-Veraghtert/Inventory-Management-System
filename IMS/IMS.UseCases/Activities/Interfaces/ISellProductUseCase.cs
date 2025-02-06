using IMS.CoreBusiness;

namespace IMS.UseCases.Activities.Interfaces
{
    public interface ISellProductUseCase
    {
        Task ExecteAsync(string salesOrderNumber, Product product, int quantity, string doneBy);
    }
}