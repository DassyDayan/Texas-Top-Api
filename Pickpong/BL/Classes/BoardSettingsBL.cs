using AutoMapper;
using Pickpong.BL.Interfaces;
using Pickpong.DAL.Interfaces;
using Pickpong.Entities;
using Pickpong.Models;

namespace Pickpong.BL.Classes
{
    public class BoardSettingsBL : IBoardSettingsBL
    {
        private const int MinShapeId = 1;
        private const int MaxShapeId = 4;
        private readonly IBoardSettingsDL _boardSettingsDL;
        private readonly IMapper _mapper;

        public BoardSettingsBL(IMapper mapper, IBoardSettingsDL boardSettingsDL)
        {
            _mapper = mapper;
            _boardSettingsDL = boardSettingsDL;
        }

        public async Task<List<SizeOptionsDto>?> GetSizesByShapeIdAsync(int idShape)
        {
            if (idShape is < MinShapeId or > MaxShapeId)
                return null;

            List<TboardSetting>? boardSettings = await _boardSettingsDL.GetSizesByShapeIdAsync(idShape).ConfigureAwait(false);
            return _mapper.Map<List<SizeOptionsDto>>(boardSettings);
        }
    }
}