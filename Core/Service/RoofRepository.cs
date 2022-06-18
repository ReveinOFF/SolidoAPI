using AutoMapper;
using Core.DTO;
using Core.Entity;
using Core.Exceptions;
using Core.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public class RoofRepository : IRoofRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public RoofRepository(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment hostingEnvironment)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
        }
        
        public async Task Create(RoofDTO roofDTO)
        {
            using (SftpClient client = new SftpClient("91.238.103.47", 22, "root", "809FK7s191TRD"))
            {
                client.Connect();

                client.ChangeDirectory("/var/www/solidoapi/Image");

                client.UploadFile(roofDTO.Image.OpenReadStream(), roofDTO.Image.FileName);
            }

            roofDTO.PathImage = "/var/www/solidoapi/Image";

            await _unitOfWork.RoofRepository.InsertAsync(_mapper.Map<Roof>(roofDTO));
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            if (id < 0)
                throw new HttpException("Id is Not Valid!", System.Net.HttpStatusCode.BadRequest);

            var result = await _unitOfWork.RoofRepository
                .GetByIdAsync(id);

            if (result == null)
                throw new HttpException("Administrator data is null!", System.Net.HttpStatusCode.BadRequest);

            using (SftpClient client = new SftpClient("91.238.103.47", 22, "root", "809FK7s191TRD"))
            {
                client.Connect();

                client.ChangeDirectory("/var/www/solidoapi/Image");

                client.DeleteFile("/var/www/solidoapi/Image" + result.Image);
            }

            await _unitOfWork.RoofRepository.DeleteAsync(result);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<RoofGetDTO> Get(int id)
        {
            if (id < 0)
                throw new HttpException("Id is Not Valid!", System.Net.HttpStatusCode.BadRequest);

            var result = await _unitOfWork.RoofRepository
                .GetByIdAsync(id);

            if (result == null)
                throw new HttpException("Administrator data is null!", System.Net.HttpStatusCode.BadRequest);

            return _mapper.Map<RoofGetDTO>(result);
        }

        public async Task<IEnumerable<RoofGetDTO>> GetAll()
        {
            return _mapper.Map<IEnumerable<RoofGetDTO>>(await _unitOfWork.RoofRepository.GetAsync());
        }
    }
}
