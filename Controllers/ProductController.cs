using Microsoft.AspNetCore.Mvc;
using practices.CQRS.Commands;
using practices.CQRS.Queries;
using MediatR;
using System.Threading.Tasks;
using practices.Model;

namespace practices.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllProductsQuery();
            var products = await _mediator.Send(query);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetProductByIdQuery { Id = id };
            var product = await _mediator.Send(query);
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateProductCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteProductCommand { Id = id };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
