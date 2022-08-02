using Grpc.Core;
using Discount.Grpc;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories.Interfaces;
using AutoMapper;

namespace Discount.Grpc.Services;

public class  DiscountService : DiscountProtoService.DiscountProtoServiceBase
{
    private readonly IDiscountRepository _repository;
    private readonly ILogger<DiscountService> _logger;
    private readonly IMapper _mapper;

   
 public DiscountService(IDiscountRepository repository, IMapper mapper, ILogger<DiscountService> logger)
//   public DiscountService(IDiscountRepository repository,ILogger<DiscountService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
             _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
//    public override Task<GetTestDiscountRequest> GetTestDiscount(GetTestDiscountRequest request, ServerCallContext context)
//   {
//     _logger.LogInformation("Received request to: GetDiscount");
//     var response = new GetTestDiscountResponse();
//     response.TestDiscount.Add(new TestDiscount
//     {
//         Amount= request.ProductName
//     });
    
//     return Task.FromResult(response);
//   }
//  public override Task<GetTestDiscountResponse> GetTestDiscount(GetTestDiscountRequest request, ServerCallContext context)
//   {
//     _logger.LogInformation("Received request to: GetDiscount");
//     var response = new GetTestDiscountResponse();
//     response.TestDiscount.Add(new TestDiscount
//     {
//         Amount= request.ProductName
//     });
    
//     return Task.FromResult(response);
//   }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await _repository.GetDiscount(request.ProductName);
            if (coupon == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={request.ProductName} is not found."));
            }
            _logger.LogInformation("Discount is retrieved for ProductName : {productName}, Amount : {amount}", coupon.ProductName, coupon.Amount);

            var couponModel = _mapper.Map<CouponModel>(coupon);
            return couponModel;
        }
        
        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);

            await _repository.CreateDiscount(coupon);
            _logger.LogInformation("Discount is successfully created. ProductName : {ProductName}", coupon.ProductName);

            var couponModel = _mapper.Map<CouponModel>(coupon);
            return couponModel;
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);

            await _repository.UpdateDiscount(coupon);
            _logger.LogInformation("Discount is successfully updated. ProductName : {ProductName}", coupon.ProductName);

            var couponModel = _mapper.Map<CouponModel>(coupon);
            return couponModel;
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var deleted = await _repository.DeleteDiscount(request.ProductName);
            var response = new DeleteDiscountResponse
            {
                Success = deleted
            };

            return response;
        }
}
