using AutoMapper;
using Pickpong.BL.Interfaces;
using Pickpong.DAL.Interfaces;
using Pickpong.Entities;
using Pickpong.Models;

namespace Pickpong.BL.Classes
{
    public class CustomizesBL : ICustomizesBL
    {
        private const int MinShapeId = 1;
        private const int MaxShapeId = 4;
        private readonly ICustomizesDL _customizesDL;
        IMapper _mapper;

        public CustomizesBL(ICustomizesDL customizesDL, IMapper mapper)
        {
            _customizesDL = customizesDL;
            _mapper = mapper;
        }

        public async Task<List<CustomizeSettingDTO>?> GetCustomizeSizesByShapeIdAsync(int idShape)
        {
            if (idShape < MinShapeId || idShape > MaxShapeId)
                return null;

            List<Tcustomize> customizeEntities = await _customizesDL.GetCustomizeSizesByShapeIdAsync(idShape);
            return _mapper.Map<List<CustomizeSettingDTO>>(customizeEntities);
        }


        public async Task<Result<decimal>> CalculateCustomPriceAsync(CartDetails carpet)
        {
            Result<bool> validationResult = ValidateCartDetailsForPriceCalculation(carpet);
            if (!validationResult.Success)
                return Result<decimal>.Failure(validationResult.ErrorCode!,
                    validationResult.Message);

            decimal sizeA = Convert.ToDecimal(carpet.FSizeParameterA ?? 0.0);
            decimal sizeB = Convert.ToDecimal(carpet.FSizeParameterB ?? 0.0);

            if (sizeA == 0 || sizeB == 0)
            {
                if (sizeA == 0) sizeA = sizeB;
                else sizeB = sizeA;
            }

            decimal price = await _customizesDL.CalculateCustomPriceAsync(
                carpet.IIdShape!.Value, sizeA, sizeB);

            return Result<decimal>.SuccessResult(price);
        }


        private Result<bool> ValidateCartDetailsForPriceCalculation(CartDetails carpet)
        {
            if (!carpet.IIdShape.HasValue || carpet.IIdShape < 1 || carpet.IIdShape > 4)
                return Result<bool>.Failure("IIdShape must have a value between 1 and 4.");

            if ((carpet.IIdShape == 1 || carpet.IIdShape == 3) &&
                !carpet.FSizeParameterA.HasValue && !carpet.FSizeParameterB.HasValue)
                return Result<bool>
                    .Failure("At least one size parameter must have a value for shapes 1 and 3.");

            if ((carpet.IIdShape == 2 || carpet.IIdShape == 4) &&
                (!carpet.FSizeParameterA.HasValue || !carpet.FSizeParameterB.HasValue))
                return Result<bool>.Failure("Size parameters must have values.");

            return Result<bool>.SuccessResult(true);
        }

    }
}