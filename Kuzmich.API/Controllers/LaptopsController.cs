﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Kuzmich.API.Data;
using Kuzmich.Domain.Entities;
using Kuzmich.Domain.Models;

namespace Kuzmich.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LaptopsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LaptopsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Laptops
        [HttpGet]
        //public async Task<ActionResult<IEnumerable<Laptop>>> GetLaptops()
        //{
        //    return await _context.Laptops.ToListAsync();
        //}
        [HttpGet]
        public async Task<ActionResult<ResponseData<ListModel<Laptop>>>> GetLaptops(
                    string? category,
                    int pageNo = 1,
                    int pageSize = 3)
        {
            var result = new ResponseData<ListModel<Laptop>>();

            var data = _context.Laptops
                .Include(d => d.Category)
                .Where(d => string.IsNullOrEmpty(category) || d.Category.NormalizedName == category);

            int totalCount = await data.CountAsync();
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            if (totalPages == 0) totalPages = 1;
            if (pageNo < 1) pageNo = 1;
            if (pageNo > totalPages) pageNo = totalPages;

            var listData = new ListModel<Laptop>
            {
                Items = await data.Skip((pageNo - 1) * pageSize).Take(pageSize).ToListAsync(),
                CurrentPage = pageNo,
                TotalPages = totalPages
            };

            result.Data = listData;

            if (totalCount == 0)
            {
                result.Success = false;
                result.ErrorMessage = "Нет объектов в выбранной категории";
            }

            return result;
        }

        // GET: api/Laptops/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Laptop>> GetLaptop(int id)
        {
            var laptop = await _context.Laptops.FindAsync(id);

            if (laptop == null)
            {
                return NotFound();
            }

            return laptop;
        }

        // PUT: api/Laptops/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLaptop(int id, Laptop laptop)
        {
            if (id != laptop.Id)
            {
                return BadRequest();
            }

            _context.Entry(laptop).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LaptopExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Laptops
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Laptop>> PostLaptop(Laptop laptop)
        {
            _context.Laptops.Add(laptop);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLaptop", new { id = laptop.Id }, laptop);
        }

        // DELETE: api/Laptops/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLaptop(int id)
        {
            var laptop = await _context.Laptops.FindAsync(id);
            if (laptop == null)
            {
                return NotFound();
            }

            _context.Laptops.Remove(laptop);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LaptopExists(int id)
        {
            return _context.Laptops.Any(e => e.Id == id);
        }
    }
}
