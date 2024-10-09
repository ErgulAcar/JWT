using AuthServer.Core.Repositories;
using AuthServer.Core.Services;
using AuthServer.Core.UnitOfWork;
using AuthServer.Service.Mapper;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service.Services
{
    public class GenericService<TEntity, TDto> : IGenericService<TEntity, TDto> where TEntity : class where TDto : class
    {
        private readonly IGenericRepository<TEntity> _genericRepository;

        private readonly IUnitOfWork _unitOfWork;

        public GenericService(IGenericRepository<TEntity> genericRepository, IUnitOfWork unitOfWork)
        {
            _genericRepository = genericRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<TDto>> AddAsync(TDto dto)
        {
            var newEntity = ObjectMapper.Mapper.Map<TEntity>(dto);

            await _genericRepository.AddAsync(newEntity);

            await _unitOfWork.CommitAsync();

            var newDto = ObjectMapper.Mapper.Map<TDto>(newEntity);

            return Response<TDto>.Success(newDto, 200);

        }

        public async Task<Response<IEnumerable<TDto>>> GetAllAsync()
        {
            var getList = await _genericRepository.GetAllAsync();

            var newDto = ObjectMapper.Mapper.Map<List<TDto>>(getList);

            return Response<IEnumerable<TDto>>.Success(newDto, 200);
        }

        public async Task<Response<TDto>> GetByIdAsync(int id)
        {
            var product = await _genericRepository.GetByIdAsync(id);

            if (product == null) 
            {
                return Response<TDto>.Fail("Id not found", 404, true);
            }

            var newDto = ObjectMapper.Mapper.Map<TDto>(product);

            return Response<TDto>.Success(newDto, 200);
        }

        public async Task<Response<NoDataDto>> RemoveAsync(int id)
        {
            var product = await _genericRepository.GetByIdAsync(id);

            if (product == null)
            {
                return Response<NoDataDto>.Fail("Id not found", 404, true);
            }

            _genericRepository.Remove(product);

            await _unitOfWork.CommitAsync();

            return Response<NoDataDto>.Success(204);
        }

        public async Task<Response<NoDataDto>> UpdateAsync(TDto dto, int id)
        {
            var product = await _genericRepository.GetByIdAsync(id);

            if (product == null)
            {
                return Response<NoDataDto>.Fail("Id not found", 404, true);
            }

            var newEntity = ObjectMapper.Mapper.Map<TEntity>(dto);

            _genericRepository.Update(newEntity);

            await _unitOfWork.CommitAsync();

            return Response<NoDataDto>.Success(204);
        }

        public async Task<Response<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> predicate)
        {
            var list = _genericRepository.Where(predicate);

            var listDto = ObjectMapper.Mapper.Map<IEnumerable<TDto>>(await list.ToListAsync());

            return Response<IEnumerable<TDto>>.Success(listDto, 200);
        }
    }
}
