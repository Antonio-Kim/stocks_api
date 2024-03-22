using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTO.Stock;
using api.Models;

namespace api.Mappers
{
    public static class StockMappers
    {
        public static StockDTO ToStockDTO(this Stock stockModel)
        {
            return new StockDTO
            {
                Id = stockModel.Id,
                Symbol = stockModel.Symbol,
                CompanyName = stockModel.CompanyName,
                Purchase = stockModel.Purchase,
                LastDiv = stockModel.LastDiv,
                Industry = stockModel.Industry,
                MarketCap = stockModel.MarketCap,
                Comments = stockModel.Comments.Select(c => c.ToCommentDTO()).ToList()
            };
        }

        public static Stock ToStockFromCreateDTO(this CreateStockRequestDTO stockDTO)
        {
            return new Stock
            {
                Symbol = stockDTO.Symbol,
                CompanyName = stockDTO.CompanyName,
                Purchase = stockDTO.Purchase,
                Industry = stockDTO.Industry,
                MarketCap = stockDTO.MarketCap
            };
        }
    }
}