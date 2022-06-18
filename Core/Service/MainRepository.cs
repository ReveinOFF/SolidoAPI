using AutoMapper;
using Core.DTO;
using Core.Entity;
using Core.Exceptions;
using Core.Interface;
using Microsoft.AspNetCore.Hosting;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public class MainRepository : IMainRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public MainRepository(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment hostingEnvironment)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task Create()
        {
            var get = _mapper.Map<IEnumerable<MainDTO>>(await _unitOfWork.MainsRepository.GetAsync());

            if (get.Count() != 0)
                throw new HttpException("Main already exists!", System.Net.HttpStatusCode.BadRequest);

            var main = new MainDTO()
            {
                Company = "Lorem ipsum dolor sit amet consectetur, " +
                "adipisicing elit. Earum, a cum. Autem provident ipsum voluptatibus, " +
                "culpa et, sapiente repellat dolorum id architecto quasi molestiae non dolore dignissimos animi earum quam.",
                OrderOne = "Lorem ipsum dolor, sit amet consectetur adipisicing elit. " +
                "Sint, accusantium possimus veniam laboriosam in laborum, " +
                "fuga et quia voluptatum dolorem tempore iusto alias nobis harum sed ea nemo ducimus necessitatibus!",
                OrderTwo = "Nam maxime ratione eum nostrum, corrupti doloremque facere debitis. " +
                "Deleniti iure eos harum, velit autem, in ipsum, voluptates totam ipsam illo omnis! " +
                "Odit quas dicta itaque dolores corporis, beatae nesciunt.",
                OrderThree = "Quos consectetur magnam doloremque ad accusamus delectus pariatur deserunt " +
                "aspernatur excepturi sunt corrupti sequi dolorem commodi nulla officiis facilis, nisi repudiandae, " +
                "neque distinctio quaerat dolore ullam atque! Corporis, a molestiae."
            };

            await _unitOfWork.MainsRepository.InsertAsync(_mapper.Map<Main>(main));
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<MainGetDTO> Get(int id)
        {
            if (id < 0)
                throw new HttpException("Invalid Id!", System.Net.HttpStatusCode.BadRequest);

            var contact = await _unitOfWork.MainsRepository
                .GetByIdAsync(id);

            if (contact == null)
            {
                throw new HttpException($"The Genre with id:{id} does not exist!", System.Net.HttpStatusCode.NotFound);
            }

            return _mapper.Map<MainGetDTO>(contact);
        }

        public async Task Update(MainDTO mainDTO)
        {
            using (SftpClient client = new SftpClient("91.238.103.47", 22, "root", "809FK7s191TRD"))
            {
                client.Connect();

                client.ChangeDirectory("/var/www/solidoapi/Image");

                foreach (var image in mainDTO.Images)
                {
                    client.UploadFile(image.OpenReadStream(), image.FileName);
                }
            }

            mainDTO.PathImage = "/var/www/solidoapi/Image";

            await _unitOfWork.MainsRepository.UpdateAsync(_mapper.Map<Main>(mainDTO));
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task Delete(string name)
        {
            var main = await _unitOfWork.MainsRepository
                .GetByIdAsync(1);

            List<string> list = new List<string>();

            foreach (string image in main.Images)
            {
                list.Add(image);
            }

            using (SftpClient client = new SftpClient("91.238.103.47", 22, "root", "809FK7s191TRD"))
            {
                client.Connect();

                client.ChangeDirectory("/var/www/solidoapi/Image");

                foreach (string image in main.Images)
                {
                    if (image == name)
                    {
                        list.Remove(name);
                        main.Images = list.ToArray();

                        await _unitOfWork.MainsRepository.UpdateAsync(main);
                        await _unitOfWork.SaveChangesAsync();

                        client.DeleteFile("/var/www/solidoapi/Image" + image);
                    }
                }
            }
        }
    }
}
