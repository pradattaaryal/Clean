using System.Threading.Tasks;
using practices.Model;
using practices.Repositories;
using MediatR;
using practices.Helpers;

namespace practices.CQRS.Commands
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, bool>
    {
        private readonly IProductRepository _productRepository;
        private readonly ImageHelper _imageHelper;

        public CreateProductHandler(IProductRepository productRepository, ImageHelper imageHelper)
        {
            _productRepository = productRepository;
            _imageHelper = imageHelper;
        }

        public async Task<bool> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        { var fileName = Guid.NewGuid() +Path.GetExtension(command.Image.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await command.Image.CopyToAsync(stream);
            }
            var product = new Product
            {
                Name = command.Name,
                Price = command.Price,
                Image = filePath
            };
            await _productRepository.AddProductAsync(product);
            return true;
        }
    }

    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, bool>
    {
        private readonly IProductRepository _productRepository;
        private readonly ImageHelper _imageHelper;

        public UpdateProductHandler(IProductRepository productRepository, ImageHelper imageHelper)
        {
            _productRepository = productRepository;
            _imageHelper = imageHelper;
        }

        public async Task<bool> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            var imagePath = command.Image != null ? await _imageHelper.SaveImageAsync(command.Image) : null;
            var product = new Product
            {
                Id = command.Id,
                Name = command.Name,
                Price = command.Price,
                Image = imagePath
            };
            await _productRepository.UpdateProductAsync(product);
            return true;
        }
    }

    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly IProductRepository _productRepository;

        public DeleteProductHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<bool> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            await _productRepository.DeleteProductAsync(command.Id);
            return true;
        }
    }

    public class SignInHandler : IRequestHandler<SignupUserDto, bool>, IRequestHandler<LoginUserDto, string>
    {
        private readonly IUserRepository _userRepository;

        private readonly IConfiguration _configuration;
        public SignInHandler(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;

        }

        public async Task<bool> Handle(SignupUserDto signupDto, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(signupDto.Email);
            if (existingUser != null)
                throw new Exception("User already exists.");
            var user = new User
            {
                Username = signupDto.Username,
                Email = signupDto.Email,
                Password = signupDto.Password,
                Role = signupDto.Role
            };
            await _userRepository.AddUserAsync(user);
            return true;
        }

        public async Task<string> Handle(LoginUserDto loginDto, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByEmailAsync(loginDto.Email);
            if (user == null)
            {
                throw new Exception("Invalid login credentials.");
            }

            return JwtTokenGenerator.GenerateToken(user, _configuration);
        }
    }

   
}
