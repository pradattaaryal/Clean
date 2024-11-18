using System.Threading.Tasks;
using practices.Model;
using practices.Repositories;
using MediatR;
using practices.Helpers;
using Application.Mapper;
using practices.Service;
namespace practices.CQRS.Commands
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, bool>
    {
        private readonly IProductService _ProductService;
        private readonly IMapper _mapper;
        public CreateProductHandler(IProductService IProductService, IMapper mapper)
        {
            _ProductService = IProductService;
            _mapper = mapper;
        }
        public async Task<bool> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        { var fileName = Guid.NewGuid() +Path.GetExtension(command.Image.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await command.Image.CopyToAsync(stream);
            }
            var product = _mapper.MapToEntity(command, filePath);
            await _ProductService.AddProductAsync(product);
            return true;
        }
    }

    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, bool>
    {
        private readonly IProductService _IProductService;
        private readonly ImageHelper _imageHelper;
        private readonly IMapper _mapper;

        public UpdateProductHandler(IProductService IProductService, ImageHelper imageHelper, IMapper mapper)
        {
            _IProductService = IProductService;
            _imageHelper = imageHelper;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            var imagePath = command.Image != null ? await _imageHelper.SaveImageAsync(command.Image) : null;
            var product = _mapper.MapToEntity(command, imagePath);
            await _IProductService.UpdateProductAsync(product);
            return true;
        }
    }

    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly IProductService _IProductService;

        public DeleteProductHandler(IProductService IProductService)
        {
            _IProductService = IProductService;
        }

        public async Task<bool> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            await _IProductService.DeleteProductAsync(command.Id);
            return true;
        }
    }

    public class SignInHandler : IRequestHandler<SignupUserDto, bool>, IRequestHandler<LoginUserDto, string>
    {
        private readonly IUserService _UserService;

        private readonly IConfiguration _configuration;
        public SignInHandler(IUserService IUserService, IConfiguration configuration)
        {
            _UserService = IUserService;
            _configuration = configuration;

        }

        public async Task<bool> Handle(SignupUserDto signupDto, CancellationToken cancellationToken)
        {
            var existingUser = await _UserService.GetUserByEmailAsync(signupDto.Email);
            if (existingUser != null)
                throw new Exception("User already exists.");
            var user = new User
            {
                Username = signupDto.Username,
                Email = signupDto.Email,
                Password = signupDto.Password,
                Role = signupDto.Role
            };
            await _UserService.AddUserAsync(user);
            return true;
        }

        public async Task<string> Handle(LoginUserDto loginDto, CancellationToken cancellationToken)
        {
            var user = await _UserService.GetUserByEmailAsync(loginDto.Email);
            if (user == null)
            {
                throw new Exception("Invalid login credentials.");
            }

            return JwtTokenGenerator.GenerateToken(user, _configuration);
        }
    }

   
}
