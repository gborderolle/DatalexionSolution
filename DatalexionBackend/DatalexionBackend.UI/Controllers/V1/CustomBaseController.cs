﻿using AutoMapper;
using DatalexionBackend.Core.DTO;
using DatalexionBackend.Core.Domain.Entities;
using DatalexionBackend.Core.Domain.RepositoryContracts;
using DatalexionBackend.Core.Helpers;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Net;

namespace DatalexionBackend.UI.Controllers.V1
{
    /// <summary>
    /// Esta versión simplifica los Controllers con esta base, mantengo ambas por si falla.
    /// Clase: https://www.udemy.com/course/construyendo-web-apis-restful-con-aspnet-core/learn/lecture/20659018#notes
    /// </summary>
    public class CustomBaseController<T> : ControllerBase where T : class, IId
    {
        protected readonly IMapper _mapper;
        protected readonly ILogger _logger;
        protected readonly IRepository<T> _repository;
        protected APIResponse _response;

        public CustomBaseController(IMapper mapper, ILogger logger, IRepository<T> repository)
        {
            _response = new APIResponse();
            _mapper = mapper;
            _logger = logger;
            _repository = repository;
        }

        protected async Task<ActionResult<APIResponse>> Get<TEntity, TDTO>
            (
                Expression<Func<T, bool>>? where = null,
                Expression<Func<T, object>>? orderBy = null,
                IEnumerable<IncludePropertyConfiguration<T>> includes = null,
                IEnumerable<ThenIncludePropertyConfiguration<T>> thenIncludes = null,
                PaginationDTO paginationDTO = null,
                bool tracked = false,
                bool ascendingOrder = true
            ) where TEntity : class
        {
            try
            {
                var list = await _repository
                    .GetAll(
                        where: where,
                        orderBy: orderBy,
                        tracked: tracked,
                        ascendingOrder: ascendingOrder,
                        httpContext: this.HttpContext,
                        paginationDTO: paginationDTO,
                        includes: includes,
                        thenIncludes: thenIncludes
                    );

                _response.Result = _mapper.Map<List<TDTO>>(list);
                _response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages = [ex.ToString()];
            }
            return Ok(_response);
        }

        protected async Task<ActionResult<APIResponse>> Get<TEntity, TDTO>
            (
                IQueryable<TEntity> queryable, // extra
                Expression<Func<T, bool>>? where = null,
                Expression<Func<T, object>>? orderBy = null,
                IEnumerable<IncludePropertyConfiguration<T>> includes = null,
                IEnumerable<ThenIncludePropertyConfiguration<T>> thenIncludes = null,
                PaginationDTO paginationDTO = null,
                bool tracked = false,
                bool ascendingOrder = true
            ) where TEntity : class
        {
            try
            {
                var list = await _repository
                    .GetAll(
                        where: where,
                        orderBy: orderBy,
                        tracked: tracked,
                        ascendingOrder: ascendingOrder,
                        httpContext: this.HttpContext,
                        paginationDTO: paginationDTO,
                        includes: includes,
                        thenIncludes: thenIncludes
                    );

                _response.Result = _mapper.Map<List<TDTO>>(list);
                _response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages = [ex.ToString()];
            }
            return Ok(_response);
        }

        protected async Task<ActionResult<APIResponse>> GetById<TEntity, TDTO>
            (
                int id,
                IEnumerable<IncludePropertyConfiguration<T>> includes = null,
                IEnumerable<ThenIncludePropertyConfiguration<T>> thenIncludes = null
            )
            where TEntity : class
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogError($"Error al obtener la entidad ID = {id}");
                    _response.ErrorMessages = new() { $"Error al obtener la entidad ID = {id}." };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var entity = await _repository.Get(
                    v => v.Id == id,
                    includes: includes,
                    thenIncludes: thenIncludes
                );

                if (entity == null)
                {
                    _logger.LogError($"Entidad no encontrada ID = {id}.");
                    _response.ErrorMessages = new() { $"Entidad no encontrada ID = {id}." };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                var dto = _mapper.Map<TDTO>(entity);

                _response.Result = dto;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages = [ex.ToString()];
            }
            return _response;
        }

        protected async Task<ActionResult<APIResponse>> Delete<TEntity>(int id) where TEntity : class
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogError($"El Id {id} es inválido.");
                    _response.ErrorMessages = new() { $"El Id {id} es inválido." };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var entity = await _repository.Get(v => v.Id == id);
                if (entity == null)
                {
                    _logger.LogError($"Entidad no encontrada ID = {id}.");
                    _response.ErrorMessages = new() { $"Entidad no encontrada ID = {id}." };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _repository.Remove(entity);
                _logger.LogInformation($"Se eliminó correctamente la entidad Id:{id}.");
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages = [ex.ToString()];
            }
            return BadRequest(_response);
        }

        protected async Task<ActionResult<APIResponse>> Put<TCreate, TDTO, TEntity>(int id, TCreate createDTO)
            where TEntity : class, IId
        {
            try
            {
                if (id <= 0)
                {
                    // _logger.LogError(_message.NotValid());
                    // _response.ErrorMessages = new() { _message.NotValid() };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var entity = await _repository.Get(v => v.Id == id, tracked: false);
                if (entity == null)
                {
                    _logger.LogError($"Entidad no encontrada ID = {id}.");
                    _response.ErrorMessages = new() { $"Entidad no encontrada ID = {id}" };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }


                entity = _mapper.Map(createDTO, entity);
                entity.Id = id;

                var updatedEntity = await _repository.Update(entity);

                _logger.LogInformation($"Se actualizó correctamente la entidad Id:{id}.");
                _response.Result = _mapper.Map<TDTO>(updatedEntity);
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages = [ex.ToString()];
            }
            return BadRequest(_response);
        }

        protected async Task<ActionResult<APIResponse>> Patch<TEntity, TDTO>(int id, JsonPatchDocument<TDTO> dto)
              where TEntity : class, IId, new()
              where TDTO : class
        {
            try
            {
                if (dto == null || id <= 0)
                {
                    _logger.LogError($"El Id {id} es inválido.");
                    _response.ErrorMessages = new() { $"El Id {id} es inválido." };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var entity = await _repository.Get(v => v.Id == id, tracked: true);
                if (entity == null)
                {
                    _logger.LogError($"Entidad no encontrada ID = {id}.");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                // Map the entity to DTO
                TDTO dtoToPatch = _mapper.Map<TDTO>(entity);

                // Aplicar el parche al DTO
                dto.ApplyTo(dtoToPatch, error =>
                {
                    ModelState.AddModelError(error.ErrorMessage, $"{error.Operation} failed on {error}: {error.ErrorMessage}");
                });

                // Validate the DTO after applying the patch.
                if (!TryValidateModel(dtoToPatch))
                {
                    _logger.LogError($"Validation failed after applying the patch to the entity ID = {id}.");
                    return BadRequest(ModelState);
                }

                // Map the DTO back to the entity and save changes.
                _mapper.Map(dtoToPatch, entity);

                await _repository.Update(entity);

                _response.Result = _mapper.Map<TDTO>(entity);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages = [ex.ToString()];
                return StatusCode((int)HttpStatusCode.InternalServerError, _response);
            }
        }

    }
}