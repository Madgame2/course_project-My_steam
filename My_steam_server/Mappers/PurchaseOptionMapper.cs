using Game_Net_DTOLib;
using My_steam_server.Models;

namespace My_steam_server.Mappers
{
    public static class PurchaseOptionMapper
    {
        public static Game_Net_DTOLib.PurchaseOption ToDto(this Models.PurchaseOption model)
        {
            return new Game_Net_DTOLib.PurchaseOption
            {
                PurchaseId = model.OptionId,
                GameName = model.GoodsReceived.FirstOrDefault()?.GoodId.ToString() ?? string.Empty,
                Price = model.Price
            };
        }

        public static Models.PurchaseOption ToModel(this Game_Net_DTOLib.PurchaseOption dto)
        {
            return new Models.PurchaseOption
            {
                OptionId = dto.PurchaseId,
                Price = dto.Price,
                GoodsReceived = new List<GoodReceived>()
            };
        }

        public static List<Game_Net_DTOLib.PurchaseOption> ToDtoList(this List<GoodReceived> goodsReceived)
        {
            return goodsReceived
                .GroupBy(gr => gr.PurchaseOptionId)
                .Select(g => g.First().PurchaseOption.ToDto())
                .ToList();
        }
    }
} 