using AutoMapper;
using Core.DTO;
using Core.Entity;
using Core.Exceptions;
using Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public class ContactRepository : IContactRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ContactRepository(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Create()
        {
            var get = _mapper.Map<ContactDTO>(await _unitOfWork.ContactRepository.GetByIdAsync(1));

            if (get != null)
                throw new HttpException("Сontact already exists!", System.Net.HttpStatusCode.BadRequest);

            var contact = new ContactDTO()
            {
                Name = "Lukasz Lucinski",
                Phone = "0176/431 417 02",
                Email = "solido1a@gmx.de",
                DateOne = "08:00",
                DateTwo = "16:00",
                AddressOne = "Keltenstr. 16",
                AddressTwo = "76744 Wörth am Rhein"
            };

            await _unitOfWork.ContactRepository.InsertAsync(_mapper.Map<Contact>(contact));
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<ContactDTO> Get(int id)
        {
            if (id < 0)
                throw new HttpException("Invalid Id!", System.Net.HttpStatusCode.BadRequest);

            var contact = await _unitOfWork.ContactRepository
                .GetByIdAsync(id);

            if (contact == null)
            {
                throw new HttpException($"The Genre with id:{id} does not exist!", System.Net.HttpStatusCode.NotFound);
            }

            return _mapper.Map<ContactDTO>(contact);
        }

        public async Task Update(ContactDTO contactDTO)
        {
            await _unitOfWork.ContactRepository.UpdateAsync(_mapper.Map<Contact>(contactDTO));
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
