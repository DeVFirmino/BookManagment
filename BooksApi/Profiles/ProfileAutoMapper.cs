// This AutoMapper profile defines how objects are mapped between DTOs and Models.
// It reduces boilerplate by automatically copying matching properties.
// 
// Mappings defined:
// - BookCreationDto → BooksModel: used when creating a new book (DTO input to database model).
// - BooksModel → BookEditDto: used when editing an existing book (database model to editable DTO).
//
// Purpose: Ensures consistency and simplifies conversion between layers (DTO ↔ Model).

using AutoMapper;
using BooksApi.Dto;
using BooksApi.Dto.Address;
using BooksApi.Dto.Report;
using BooksApi.Models;

namespace BooksApi.Profiles;

public class ProfileAutoMapper : Profile 
{
     
    public ProfileAutoMapper()
    {
        CreateMap<BookCreationDto, BooksModel>();
        CreateMap<BooksModel, BookEditDto>();
        CreateMap<BookEditDto, BooksModel>();
        CreateMap<AddressModel, AddressEditDto>();
        CreateMap<AddressEditDto, AddressModel>();
        CreateMap<BooksModel, BookReportDto>();
        CreateMap<UserModel, UserReportDto>();
        CreateMap<BorrowModel, BorrowReportDto>();
        

    }
    }
    
