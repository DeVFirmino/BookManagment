using AutoMapper;
using BooksApi.Dto;
using BooksApi.Models;

namespace BooksApi.Profiles;

public class ProfileAutoMapper : Profile 
{
     
    public ProfileAutoMapper()
    {
        CreateMap<BookCreationDto, BooksModel>();
    }
    
}