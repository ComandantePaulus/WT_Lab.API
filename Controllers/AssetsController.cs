﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WT_Lab.API.Data;
using WT_Lab.Domain;
using WT_Lab.Models;

namespace WT_Lab.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public AssetsController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [HttpPost("{id}")]
        public async Task<IActionResult> SaveImage(int id, IFormFile image)
        {
            // Найти объект по Id
            var asset = await _context.Asset.FindAsync(id);
            if (asset == null)
            {
                return NotFound();
            }
            // Путь к папке wwwroot/Images
            var imagesPath = Path.Combine(_env.WebRootPath, "Images");
            // получить случайное имя файла
            var randomName = Path.GetRandomFileName();
            // получить расширение в исходном файле
            var extension = Path.GetExtension(image.FileName);
            // задать в новом имени расширение как в исходном файле
            var fileName = Path.ChangeExtension(randomName, extension);
            // полный путь к файлу
            var filePath = Path.Combine(imagesPath, fileName);
            // создать файл и открыть поток для записи
            using var stream = System.IO.File.OpenWrite(filePath);
            // скопировать файл в поток
            await image.CopyToAsync(stream);
            // получить Url хоста
            var host = "https://" + Request.Host;
            // Url файла изображения
            var url = $"{host}/Images/{fileName}";
            // Сохранить url файла в объекте
            asset.Photo = url;
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpGet]
        public async Task<ActionResult<ResponseData<AssetListModel<Asset>>>> GetAssets(string? category,int pageNo = 1,int pageSize = 3)
        {
            // Создать объект результата
            var result = new ResponseData<AssetListModel<Asset>>();
            // Фильтрация по категории загрузка данных категории
            var data = _context.Asset
            .Include(d => d.Category)
            .Where(d => String.IsNullOrEmpty(category)
            || d.Category.NormalizedName.Equals(category));
            // Подсчет общего количества страниц
            int totalPages = (int)Math.Ceiling(data.Count() / (double)pageSize);
            if (pageNo > totalPages)
                pageNo = totalPages;
            // Создание объекта ProductListModel с нужной страницей данных
            var listData = new AssetListModel<Asset>()
            {
                Items = await data
            .Skip((pageNo - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(),
                CurrentPage = pageNo,
                TotalPages = totalPages
            };
            // поместить данные в объект результата
            result.Data = listData;
            // Если список пустой
            if (data.Count() == 0)
            {
                result.Success = false;
                result.ErrorMessage = "Нет объектов в выбранной категории";
            }
            return result;
        }

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Asset>>> GetAssets()
        //{
        //   return await _context.Asset.ToListAsync();
        //}
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
