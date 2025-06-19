using HotelBooking.Application.Command;
using HotelBooking.Application.Query;
using HotelBooking.Application.Services;
using HotelBooking.Domain.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [Authorize(Policy = "EmployeeOnly")]
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
    public async Task<IActionResult> GetProducts()
    {
        var products = await mediator.Send(new GetAllProductsQuery());
        return StatusCode(200, products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        var productDTO = await mediator.Send(new GetProductByIdQuery { Id = id });
        return StatusCode(200, productDTO);
    }

    [HttpPatch("{id}")]
    [Authorize(Policy = "EmployeeOnly")]
    public async Task<IActionResult> PatchProduct(int id, [FromBody]CreateProductDTO product)
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
    [Authorize(Policy = "EmployeeOnly")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var productId = await mediator.Send(new DeleteProductCommand
        {
            Id = id
        });

        return StatusCode(200, $"Product with id {id} has been deleted");
    }
}
