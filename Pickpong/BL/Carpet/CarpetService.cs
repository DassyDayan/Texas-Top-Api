using AutoMapper;
using Pickpong.DAL.Interfaces;
using Pickpong.Dto.Classes;
using Pickpong.Entities;
using Pickpong.Models;
using Pickpong.Services.Interfaces;

namespace Pickpong.BL.Carpet
{
    public class CarpetService : ICarpetService
    {

        private readonly IPlayerServiceBL _playerService;
        private readonly ICartServiceBL _cartService;
        private readonly ICarpetDL _carpetDL;
        private readonly IMapper _mapper;

        public CarpetService(
            IPlayerServiceBL playerService,
            ICartServiceBL cartService,
            ICarpetDL carpetDL,
            IMapper mapper)
        {
            _playerService = playerService;
            _cartService = cartService;
            _carpetDL = carpetDL;
            _mapper = mapper;
        }

        public async Task<Result<List<CartDetailsDto>>> CreateCarpetAsync(carpetDetailModel carpetModel, int userId)
        {
            try
            {
                TcarpetDetail carpet = _mapper.Map<TcarpetDetail>(carpetModel);
                int carpetId = await _carpetDL.CreateCarpetEntryAsync(carpet);
                await SetDefaultFileNamesAsync(carpetId);
                await _playerService.AddPlayersToCarpetAsync(carpetModel.Players, carpetId);
                await _cartService.AddCarpetToUserCartAsync(userId, carpetId);
                List<CartDetailsDto> cartDetails = await _cartService.GetCartDetailsDtoAsync(userId);

                return Result<List<CartDetailsDto>>.SuccessResult(cartDetails, "Carpet created and added to cart");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CreateCarpetAsync Error] {ex}");
                return Result<List<CartDetailsDto>>.Failure("Failed to create carpet", "CARPET_CREATE_ERROR", ex);

            }
        }

        public async Task<CarpetDetailsResponse> GetCarpetDetailsAsync(int carpetId)
        {
            return await _carpetDL.GetCarpetDetailsAsync(carpetId);
        }

        public async Task<List<CartDetails>> GetCarpetsWithPricesAsync(int orderId)
        {
            List<TcartDetail> carpets = await _carpetDL.GetCarpetsWithRegularPricesAsync(orderId);
            return _mapper.Map<List<CartDetails>>(carpets);
        }

        public async Task<List<CartDetails>> GetCarpetsInOrderAsync(int orderId)
        {
            List<TcartDetail> carpets = await _carpetDL.GetCarpetsInOrderAsync(orderId);
            return _mapper.Map<List<CartDetails>>(carpets);
        }

        public async Task SetDefaultFileNamesAsync(int carpetId)
        {
            TcarpetDetail? carpet = await _carpetDL.GetByIdAsync(carpetId);
            if (carpet == null) return;

            carpet.NvCarpetFileName = $"CarpetNo_{carpetId}/example.pdf";
            carpet.NvSmallCarpetFileName = $"CarpetNo_{carpetId}/example.bmp";

            await _carpetDL.UpdateAsync(carpet);
        }

        public async Task<List<TcarpetDetail>> GetCarpetsByIdsAsync(List<int> carpetIds)
        {
            return await _carpetDL.GetCarpetsByIdsAsync(carpetIds);
        }
    }
}