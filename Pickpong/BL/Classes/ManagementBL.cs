using AutoMapper;
using Pickpong.BL.Interfaces;
using Pickpong.DAL.Interfaces;
using Pickpong.Entities;
using Pickpong.Models;

namespace Pickpong.BL.Classes
{
    public class ManagementBL : IManagementBL
    {

        private const int MinShapeId = 1;
        private const int MaxShapeId = 4;
        private readonly IMapper _mapper;
        private readonly IBoardSettingsDL _boardSettingsDL;
        private readonly ICustomizesDL _customizesDL;
        private readonly IOrderDL _orderDL;


        public ManagementBL(IMapper mapper, IBoardSettingsDL boardSettingsDL,
            ICustomizesDL customizesDL, IOrderDL orderDL)
        {
            _mapper = mapper;
            _boardSettingsDL = boardSettingsDL;
            _customizesDL = customizesDL;
            _orderDL = orderDL;
        }

        public async Task<List<List<OrdersDetailModel>>> GetAllOrdersAsync()
        {
            List<OrdersDetailModel> all = await _orderDL.GetAllOrdersAsync();

            if (all == null)
                return new List<List<OrdersDetailModel>>();

            return all
                .GroupBy(x => x.IIdOrder)
                .Select(g => g.ToList())
                .ToList();
        }

        public async Task<List<TboardSetting>?> GetSettingsAsync(int shapeId)
        {
            if (shapeId < MinShapeId || shapeId > MaxShapeId)
                return null;
            return await _boardSettingsDL.GetSizesByShapeIdAsync(shapeId);
        }

        public async Task<List<Tcustomize>?> GetCustomizeSettingsAsync(int shapeId)
        {
            if (shapeId < MinShapeId || shapeId > MaxShapeId)
                return null;
            return await _customizesDL.GetCustomizeSettingsAsync(shapeId);
        }

        public async Task<bool> UpdateSettingAsync(Settings setting)
        {
            TboardSetting? existing = await _boardSettingsDL.GetSettingByIdAsync(setting.IIdSize);
            if (existing == null) return false;
            if (setting.IIdShape < MinShapeId || setting.IIdShape > MaxShapeId)
                return false;
            _mapper.Map(setting, existing);
            await _boardSettingsDL.UpdateAsync(existing);
            return true;
        }

        public async Task<int> AddSettingAsync(Settings setting)
        {
            if (setting.IIdShape < MinShapeId || setting.IIdShape > MaxShapeId)
                return -1;
            TboardSetting newSetting = _mapper.Map<TboardSetting>(setting);
            await _boardSettingsDL.AddAsync(newSetting);
            return newSetting.IIdSize;
        }

        public async Task<bool> RemoveSettingAsync(int id)
        {
            TboardSetting? existing = await _boardSettingsDL.GetSettingByIdAsync(id);
            if (existing == null) return false;
            await _boardSettingsDL.RemoveAsync(existing.IIdSize);
            return true;
        }

        public async Task<bool> UpdateCustomizeSettingAsync(SettingOfCustomize customize)
        {
            if (customize.IIdShape < MinShapeId || customize.IIdShape > MaxShapeId)
                return false;
            Tcustomize? existing = await _customizesDL.GetCustomizeSettingAsync(customize.IIdShape);
            if (existing == null) return false;
            _mapper.Map(customize, existing);
            await _customizesDL.UpdateCustomizeSettingAsync(existing);
            return true;
        }

        public async Task<bool> MarkOrderAsSentAsync(int orderId)
        {
            Torder? order = await _orderDL.GetOrderByIdAsync(orderId);
            if (order == null) return false;
            order.ISysTableRowIdOrderStatus = 2;
            await _orderDL.SaveChangesAsync();
            return true;
        }
    }
}