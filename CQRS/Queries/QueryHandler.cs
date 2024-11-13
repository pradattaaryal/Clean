using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using practices.Model;
using practices.Repositories;
using MediatR;

namespace practices.CQRS.Queries
{
    public class GetAllProductsQuery : IRequest<IEnumerable<Product>> { }

    public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<Product>>
    {
        private readonly IProductRepository _productRepository;

        public GetAllProductsHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            return await _productRepository.GetAllProductsAsync();
        }
    }

    public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, Product>
    {
        private readonly IProductRepository _productRepository;

        public GetProductByIdHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Product> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
        {
            return await _productRepository.GetProductByIdAsync(query.Id);
        }
    }


 
}
