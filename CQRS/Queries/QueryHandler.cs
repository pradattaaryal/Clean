using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using practices.Model;
using practices.Repositories;
using MediatR;
using practices.Service;

namespace practices.CQRS.Queries
{
    public class GetAllProductsQuery : IRequest<IEnumerable<Product>> { }

    public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<Product>>
    {
        private readonly IProductService _ProductService;

        public GetAllProductsHandler(IProductService IProductService)
        {
            _ProductService = IProductService;
        }

        public async Task<IEnumerable<Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            return await _ProductService.GetAllProductsAsync();
        }
    }

    public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, Product>
    {
        private readonly IProductService _ProductService;

        public GetProductByIdHandler(IProductService IProductService)
        {
            _ProductService = IProductService;
        }

        public async Task<Product> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
        {
            return await _ProductService.GetProductByIdAsync(query.Id);
        }
    }


 
}
