using System.Collections.Generic;
using System.Threading.Tasks;

namespace Danom.Operations;
/// <summary>
/// List entities without any filter.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IListEntity<out T> {
    /// <summary>
    /// List entities without any filter.
    /// </summary>
    /// <returns></returns>
    IEnumerable<T> List();
}

/// <summary>
/// List entities without any filter.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IListEntityAsync<T> {
    /// <summary>
    /// List entities without any filter.
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<T>> ListAsync();
}

/// <summary>
/// List entities, filtered by some criteria. The filter can be a simple
/// value, or a complex object with multiple properties.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TFilter"></typeparam>
public interface IListEntity<out T, in TFilter> {
    /// <summary>
    /// List entities, filtered by some criteria.
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    IEnumerable<T> List(TFilter filter);
}

/// <summary>
/// List entities, filtered by some criteria. The filter can be a simple
/// value, or a complex object with multiple properties.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TFilter"></typeparam>
public interface IListEntityAsync<T, in TFilter> {
    /// <summary>
    /// List entities, filtered by some criteria.
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    Task<IEnumerable<T>> ListAsync(TFilter filter);
}

/// <summary>
/// Get an entity by its key. The key can be a single value, or a composite
/// of multiple values.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TKey"></typeparam>
public interface IGetEntity<T, in TKey> {
    /// <summary>
    /// Get an entity by its key.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    Option<T> Get(TKey key);
}

/// <summary>
/// Get an entity by its key. The key can be a single value, or a composite
/// of multiple values.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TKey"></typeparam>
public interface IGetEntityAsync<T, in TKey> {
    /// <summary>
    /// Get an entity by its key.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    Task<Option<T>> GetAsync(TKey key);
}

/// <summary>
/// Save an entity, without a return value.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ISaveEntity<in T> : ISaveEntity<T, Unit> { }

/// <summary>
/// Save an entity, without a return value.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ISaveEntityAsync<in T> : ISaveEntityAsync<T, Unit> { }

/// <summary>
/// Save an entity, with a return value of TOutput.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TOutput"></typeparam>
public interface ISaveEntity<in T, out TOutput> {
    /// <summary>
    /// Save an entity, with a return value of TOutput.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    TOutput Save(T entity);
}

/// <summary>
/// Save an entity, with a return value of TOutput.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TOutput"></typeparam>
public interface ISaveEntityAsync<in T, TOutput> {
    /// <summary>
    /// Save an entity, with a return value of TOutput.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<TOutput> SaveAsync(T entity);
}

/// <summary>
/// Create an entity, without a return value.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ICreateEntity<in T> : ICreateEntity<T, Unit> { }

/// <summary>
/// Create an entity, without a return value.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ICreateEntityAsync<in T> : ICreateEntityAsync<T, Unit> { }

/// <summary>
/// Create an entity, with a return value of TOutput.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TOutput"></typeparam>
public interface ICreateEntity<in T, out TOutput> {
    /// <summary>
    /// Create an entity, with a return value of TOutput.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    TOutput Create(T entity);
}

/// <summary>
/// Create an entity, with a return value of TOutput.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TOutput"></typeparam>
public interface ICreateEntityAsync<in T, TOutput> {
    /// <summary>
    /// Create an entity, with a return value of TOutput.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<TOutput> CreateAsync(T entity);
}

/// <summary>
/// Update an entity, without a return value.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IUpdateEntity<in T> : IUpdateEntity<T, Unit> { }

/// <summary>
/// Update an entity, without a return value.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IUpdateEntityAsync<in T> : IUpdateEntityAsync<T, Unit> { }

/// <summary>
/// Update an entity, with a return value of TOutput.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TOutput"></typeparam>
public interface IUpdateEntity<in T, out TOutput> {
    /// <summary>
    /// Update an entity, with a return value of TOutput.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    TOutput Update(T entity);
}

/// <summary>
/// Update an entity, with a return value of TOutput.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TOutput"></typeparam>
public interface IUpdateEntityAsync<in T, TOutput> {
    /// <summary>
    /// Update an entity, with a return value of TOutput.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<TOutput> UpdateAsync(T entity);
}

/// <summary>
/// Delete an entity, without a return value.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IDeleteEntity<in T> : IDeleteEntity<T, Unit> { }

/// <summary>
/// Delete an entity, without a return value.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IDeleteEntityAsync<in T> : IDeleteEntityAsync<T, Unit> { }

/// <summary>
/// Delete an entity, with a return value of TOutput.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TOutput"></typeparam>
public interface IDeleteEntity<in T, out TOutput> {
    /// <summary>
    /// Delete an entity, with a return value of TOutput.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    TOutput Delete(T entity);
}

/// <summary>
/// Delete an entity, with a return value of TOutput.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TOutput"></typeparam>
public interface IDeleteEntityAsync<in T, TOutput> {
    /// <summary>
    /// Delete an entity, with a return value of TOutput.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<TOutput> DeleteAsync(T entity);
}
