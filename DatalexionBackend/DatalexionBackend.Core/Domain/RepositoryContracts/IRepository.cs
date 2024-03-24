﻿using System.Linq.Expressions;
using DatalexionBackend.Core.Domain.Entities;
using DatalexionBackend.Core.DTO;
using Microsoft.AspNetCore.Http;

namespace DatalexionBackend.Core.Domain.RepositoryContracts
{
    public interface IRepository<T> where T : class
    {
        Task Create(T entity);

        Task<List<T>> GetAll
            (
                Expression<Func<T, bool>>? where = null,
                Expression<Func<T, object>>? orderBy = null,
                IEnumerable<IncludePropertyConfiguration<T>> includes = null,
                IEnumerable<ThenIncludePropertyConfiguration<T>> thenIncludes = null,
                PaginationDTO paginationDTO = null, HttpContext httpContext = null,
                bool tracked = false,
                bool ascendingOrder = true
            );

        Task<T> Get
            (
                Expression<Func<T, bool>>? filter = null,
                IEnumerable<IncludePropertyConfiguration<T>> includes = null,
                IEnumerable<ThenIncludePropertyConfiguration<T>> thenIncludes = null,
                bool tracked = true
            );

        Task<T> Update(T entity);

        Task Remove(T entity);

        Task Save();
    }
}

public class IncludePropertyConfiguration<T>
{
    public Expression<Func<T, object>> IncludeExpression { get; set; }
    public Expression<Func<object, object>> ThenIncludeExpression { get; set; }
}

public class ThenIncludePropertyConfiguration<T>
{
    public Expression<Func<T, object>> IncludeExpression { get; set; }
    public Expression<Func<object, object>> ThenIncludeExpression { get; set; }
}