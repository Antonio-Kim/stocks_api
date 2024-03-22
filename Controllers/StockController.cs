using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DTO.Stock;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IStockRepository _stockRepo;
        public StockController(ApplicationDBContext context, IStockRepository stockRepo)
        {
            _stockRepo = stockRepo;
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var stocks = await _stockRepo.GetAllAsync();
            var stockDTO = stocks.Select(s => s.ToStockDTO());

            return Ok(stocks);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var stock = await _context.Stocks.FindAsync(id);
            if (stock == null)
            {
                return NotFound();
            }

            return Ok(stock.ToStockDTO());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDTO stockDTO)
        {
            var stockModel = stockDTO.ToStockFromCreateDTO();
            await _context.Stocks.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDTO()); 
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDTO updateDTO)
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            if (stockModel == null)
            {
                return NotFound();
            }

            stockModel.Symbol = updateDTO.Symbol;
            stockModel.CompanyName = updateDTO.CompanyName;
            stockModel.Purchase = updateDTO.Purchase;
            stockModel.LastDiv = updateDTO.LastDiv;
            stockModel.Industry = updateDTO.Industry;
            stockModel.MarketCap = updateDTO.MarketCap;

            await _context.SaveChangesAsync();

            return Ok(stockModel.ToStockDTO());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            if (stockModel == null)
            {
                return NotFound();
            }

            _context.Stocks.Remove(stockModel);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}