//using MediatR;
//using Microsoft.AspNetCore.Hosting;
//using System.IO;
//using System.Threading;
//using System.Threading.Tasks;

//namespace Infrastructure.CQRS.Commands.Request.User
//{
//    public class RemoveImageHandler : IRequestHandler<RemoveImageCommand, string>
//    {
//        readonly private IWebHostEnvironment _environment;

//        public RemoveImageHandler(IWebHostEnvironment environment)
//        {
//            _environment = environment;
//        }

//        public async Task<string> Handle(RemoveImageCommand request, CancellationToken cancellationToken)
//        {
//            string path = "/img/" + request.Title + ".jpg";
//            FileInfo file = new FileInfo(_environment.WebRootPath + path);
//            await Task.Run( () => file.Delete());
//            return path;
//        }
//    }
//}
