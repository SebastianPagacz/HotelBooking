using HotelBooking.Domain.Models;
using HotelBooking.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HotelBooking.Application.Queries;
using MediatR;

namespace HotelBooking.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController(IRepository repository, IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddProduct([FromBody] Product product)
    {
        await repository.AddProductAsync(product);
        return StatusCode(200, product);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProductsAsync()
    {
        // var products = await repository.GetAllProductsAsync();
        var products = await mediator.Send(new GetAllProductsQuery());
        return StatusCode(200, products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductByIdAsync(int id)
    {
        var product = await repository.GetProductByIdAsync(id);
        return StatusCode(200, product);
    }

    [HttpPatch]
    public async Task<IActionResult> UpdateProductAsync(Product product)
    {
        var updatedProduct = await repository.UpdateProductAsync(product);
        return StatusCode(200, updatedProduct);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProductAsync(int id)
    {
        var exisitngProduct = await repository.GetProductByIdAsync(id);
        exisitngProduct.IsDeleted = true;
        await repository.UpdateProductAsync(exisitngProduct);
        return StatusCode(200, exisitngProduct);
    }
}
