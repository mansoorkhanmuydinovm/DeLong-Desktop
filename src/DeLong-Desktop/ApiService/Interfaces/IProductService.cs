﻿using DeLong_Desktop.ApiService.Configurations;
using DeLong_Desktop.ApiService.DTOs.Products;

namespace DeLong_Desktop.ApiService.Interfaces;

interface IProductService
{
    ValueTask<ProductResultDto> AddAsync(ProductCreationDto dto);
    ValueTask<ProductResultDto> ModifyAsync(ProductUpdateDto dto);
    ValueTask<bool> RemoveAsync(long id);
    ValueTask<ProductResultDto> RetrieveByIdAsync(long id);
    ValueTask<IEnumerable<ProductResultDto>> RetrieveAllAsync(PaginationParams @params, Filter filter, string search = null);
    ValueTask<IEnumerable<ProductResultDto>> RetrieveAllAsync();
}
