using HotelBooking.Domain.Models;
using HotelBooking.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HotelBooking.Application.Query;
using MediatR;
using HotelBooking.Domain.DTOs;
using HotelBooking.Application.Command;

namespace HotelBooking.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddProduct([FromBody] CreateProductDTO product)
    {
        var result = await mediator.Send(new AddProductCommand
        {
            Name = product.Name,
            IsDeleted = false,
            NumberOfRooms = product.NumberOfRooms,
            NumberOfPeople = product.NumberOfPeople,
            Price = product.Price,
        });

        return StatusCode(200, result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProductsAsync()
    {
        var products = await mediator.Send(new GetAllProductsQuery());
        return StatusCode(200, products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductByIdAsync(int id)
    {
        var productDTO = await mediator.Send(new GetProductByIdQuery { Id = id });
        return StatusCode(200, productDTO);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateProductAsync(int id, [FromBody]CreateProductDTO product)
    {
        var updatedProduct = await mediator.Send(new UpdateProductCommand
        {
            Id = id,
            Name = product.Name,
            NumberOfPeople = product.NumberOfPeople,
            NumberOfRooms = product.NumberOfRooms,
            IsDeleted = product.IsDeleted
        });

        return StatusCode(200, updatedProduct);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProductAsync(int id)
    {
        var productId = mediator.Send(new DeleteProductCommand
        {
            Id = id
        });

        return StatusCode(200, $"Product with id {id} has been deleted");
    }
}
