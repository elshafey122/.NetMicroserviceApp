using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Mongo.ProductApi.Services.Interfaces;
using Mongo.ShoppingCartApi.Data;
using Mongo.ShoppingCartApi.Model;
using Mongo.ShoppingCartApi.Model.Dto;
using Mongo.ShoppingCartApi.Services.Interfaces;
using System.Reflection.PortableExecutable;

namespace Mongo.ProductApi.Services.Implemintations
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ResponseDto _responseDto;
        private readonly IRestRepository _restRepository;
        public ShoppingCartRepository(ApplicationDbContext context, IMapper mapper, IRestRepository restRepository)
        {
            _mapper = mapper;
            _context = context;
            _responseDto = new ResponseDto();
            _restRepository = restRepository;
        }

        public async Task<ResponseDto> ApplyCoupon(CartDto cartDto)
        {
            try
            {
                var cartHeader = await _context.CartHeaders.FirstOrDefaultAsync(x => x.UserId == cartDto.cartHeader.UserId);
                cartHeader.CouponCode = cartDto.cartHeader.CouponCode;
                _context.CartHeaders.Update(cartHeader);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _responseDto.Message = ex.Message;
                _responseDto.IsSucceeded = false;
            }
            return _responseDto;
        }

        public async Task<ResponseDto> RemoveCoupon(CartDto cartDto)
        {
            try
            {
                var cartHeader = await _context.CartHeaders.FirstOrDefaultAsync(x => x.UserId == cartDto.cartHeader.UserId);
                cartHeader.CouponCode = "";
                _context.CartHeaders.Update(cartHeader);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _responseDto.Message = ex.Message;
                _responseDto.IsSucceeded = false;
            }
            return _responseDto;
        }

        public async Task<ResponseDto> GetCart(string userId)
        {
            try
            {
                CartDto cartDto = new();
                cartDto.cartHeader = _mapper.Map<CartHeaderDto>(await _context.CartHeaders.FirstOrDefaultAsync(x => x.UserId == userId));
                cartDto.cartDetails = _mapper.Map<IEnumerable<CartDetailsDto>>(_context.CartDetails.Where(x => x.CartheaderId == cartDto.cartHeader.CartHeaderId));

                IEnumerable<ProductDto> productDtos = await _restRepository.GetProductsAsync();
                foreach (var item in cartDto.cartDetails)
                {
                    item.product = productDtos.FirstOrDefault(x => x.ProductId == item.productId);
                    cartDto.cartHeader.CartTotal += item.Count * item.product.Price;
                }
                if(!string.IsNullOrEmpty(cartDto.cartHeader.CouponCode))
                {
                    CouponDto couponDto = await _restRepository.GetCouponAsync(cartDto.cartHeader.CouponCode);
                    if(couponDto!=null && cartDto.cartHeader.CartTotal> couponDto.MinAmount)
                    {
                        cartDto.cartHeader.CartTotal -= couponDto.DiscountAmount;
                        cartDto.cartHeader.Discount = couponDto.DiscountAmount;
                    }
                }
                _responseDto.Data = cartDto;

            }
            catch (Exception ex)
            {
                _responseDto.IsSucceeded = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        public async Task<ResponseDto> RemoveCart(int cartdetailsId)
        {
            try
            {
                var cartDetails = await _context.CartDetails.FirstOrDefaultAsync(x => x.CartDetailsId == cartdetailsId);
                if (cartDetails == null)
                {
                    _responseDto.IsSucceeded = false;
                    _responseDto.Message = "No CartDetails found";
                    return _responseDto;
                }
                _context.CartDetails.Remove(cartDetails);

                int TotalCountofCartItem = _context.CartDetails.Where(x => x.CartheaderId == cartDetails.CartheaderId).Count();
                if (TotalCountofCartItem == 1)
                {
                    var cardHeaderToRemove = _context.CartHeaders.Where(x => x.CartHeaderId == cartDetails.CartheaderId).FirstOrDefault();
                    _context.CartHeaders.Remove(cardHeaderToRemove);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _responseDto.IsSucceeded = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        public async Task<ResponseDto> Upsert(CartDto cartDto)
        {
            try
            {
                var cardHeaderFromDb = await _context.CartHeaders.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == cartDto.cartHeader.UserId);
                if (cardHeaderFromDb == null)
                {
                    // create new header then details
                    CartHeader cartHeader = _mapper.Map<CartHeader>(cartDto.cartHeader);
                    _context.CartHeaders.Add(cartHeader);
                    await _context.SaveChangesAsync();
                    cartDto.cartDetails.First().CartheaderId = cartHeader.CartHeaderId;
                    await _context.CartDetails.AddAsync(_mapper.Map<CartDetails>(cartDto.cartDetails.First()));
                    await _context.SaveChangesAsync();
                }
                else
                {
                    var cardDetailsFromDb = await _context.CartDetails.AsNoTracking().FirstOrDefaultAsync
                        (x => x.productId == cartDto.cartDetails.First().productId && x.CartheaderId == cardHeaderFromDb.CartHeaderId);
                    if (cardDetailsFromDb == null)
                    {
                        // create new cartdetalis
                        cartDto.cartDetails.First().CartheaderId = cardHeaderFromDb.CartHeaderId;
                        await _context.CartDetails.AddAsync(_mapper.Map<CartDetails>(cartDto.cartDetails.First()));
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        //update only count of cart
                        cartDto.cartDetails.First().Count += cardDetailsFromDb.Count;
                        cartDto.cartDetails.First().CartDetailsId = cardDetailsFromDb.CartDetailsId;
                        cartDto.cartDetails.First().CartheaderId = cardDetailsFromDb.CartheaderId;
                        _context.CartDetails.Update(_mapper.Map<CartDetails>(cartDto.cartDetails.First()));
                        await _context.SaveChangesAsync();
                    }
                }
                _responseDto.Data = cartDto;
            }
            catch (Exception ex)
            {
                _responseDto.IsSucceeded = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }
    }
}