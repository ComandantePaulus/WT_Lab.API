﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WT_Lab.API.Data;
using WT_Lab.Domain;

namespace WT_Lab.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public AssetsController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Asset>>> GetAssets()
        {
           return await _context.Asset.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Asset>> GetAsset(int id)
        {
            var asset= await _context.Asset.FindAsync(id);
            if (asset == null)
            {
                return NotFound();
            }
            return asset;
        }
        [HttpPut]
        public async Task<IActionResult> PutAsset(int id, Asset asset)
        {
            if (id != asset.ID)
            {
                return BadRequest();
            }
            _context.Entry(asset).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DBConcurrencyException)
            {
                if (!AssetExists(id))
                {
                    return NotFound();
                }
                else { throw; }
            }
            return NoContent();
        }
        [HttpPost]
        public async Task<ActionResult<Asset>> PostAsset(Asset asset)
        {
            _context.Asset.Add(asset);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetAsset", new {id =asset.ID},asset);
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteAsset(int id)
        {
            var asset = await _context.Asset.FindAsync(id);
            if (asset == null)
            {
                return NotFound();
            }
            _context.Asset.Remove(asset);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        private bool AssetExists(int id)
        {
            return _context.Asset.Any(a => a.ID == id);
        }

    }
}