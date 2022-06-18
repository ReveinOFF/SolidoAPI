using AutoMapper;
using Core.DTO;
using Core.Entity;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Core.Helpers
{
    public class MapperProfile : Profile
    {
        private string[] CreateImage(IFormFile[] file)
        {
            if (file != null)
            {
                List<string> imageNames = new List<string>();
                foreach (var image in file)
                {
                    imageNames.Add(image.FileName);
                }
                return imageNames.ToArray();
            }
            return null;
        }
        private string[] GetImage(string[] files)
        {
            if (files != null)
            {
                List<string> fileNames = new List<string>();
                foreach (var file in files)
                {
                    fileNames.Add(file);
                }
                return fileNames.ToArray();
            }
            return null;
        }
        public MapperProfile()
        {
            CreateMap<Roof, RoofDTO>().ReverseMap().ForMember(roof => roof.Image, dto => dto.MapFrom(x => x.Image.FileName.ToString()));
            CreateMap<Roof, RoofGetDTO>().ReverseMap();
            CreateMap<Contact, ContactDTO>().ReverseMap();
            CreateMap<Main, MainDTO>().ReverseMap().ForMember(main => main.Images, dto => dto.MapFrom(x => CreateImage(x.Images)));
            CreateMap<Main, MainGetDTO>().ReverseMap();
        }
    }
}
