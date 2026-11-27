using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Danom.Validation;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Danom.Operations;

public interface ITransformer<TInput, TDomain> {
    Result<TDomain, ResultErrors> Transform(TInput input);
}

public abstract class ResultCommandHandler<TInput, TDomain, TOutput>(
    ITransformer<TInput, TDomain> transformer,
    IValidator<TDomain> validator,
    Func<TDomain, TOutput> fn) :
    ICommandHandler<TInput, Result<TOutput, ResultErrors>> {
    public Result<TOutput, ResultErrors> Handle(TInput command) =>
        transformer.Transform(command)
            .Bind(validator.Validate)
            .Map(fn);
}

public abstract class ResultCommandHandlerAsync<TInput, TDomain, TOutput>(
    ITransformer<TInput, TDomain> transformer,
    IValidator<TDomain> validator,
    Func<TDomain, Task<TOutput>> fn) :
    ICommandHandlerAsync<TInput, Result<TOutput, ResultErrors>> {
    public Task<Result<TOutput, ResultErrors>> HandleAsync(TInput command) =>
        transformer.Transform(command)
            .Bind(validator.Validate)
            .Match(
                ok: async valid => Result<TOutput, ResultErrors>.Ok(await fn(valid)),
                error: errors => Task.FromResult(Result<TOutput, ResultErrors>.Error(errors)));
}

public abstract class UnitResultCommandHandler<TInput, TDomain>(
    ITransformer<TInput, TDomain> transformer,
    IValidator<TDomain> validator,
    Func<TDomain, Unit> fn) : ResultCommandHandler<TInput, TDomain, Unit>(transformer, validator, fn) {
}

public abstract class UnitResultCommandHandler<TDomain>(
    IValidator<TDomain> validator,
    Func<TDomain, Unit> fn) : UnitResultCommandHandler<TDomain, TDomain>(new IdTransformer<TDomain>(), validator, fn) {
}

public abstract class UnitResultCommandHandlerAsync<TInput, TDomain>(
    ITransformer<TInput, TDomain> transformer,
    IValidator<TDomain> validator,
    Func<TDomain, Task<Unit>> fn) : ResultCommandHandlerAsync<TInput, TDomain, Unit>(transformer, validator, fn) {
}

public abstract class UnitResultCommandHandlerAsync<TDomain>(
    IValidator<TDomain> validator,
    Func<TDomain, Task<Unit>> fn) : UnitResultCommandHandlerAsync<TDomain, TDomain>(new IdTransformer<TDomain>(), validator, fn) {
}

public abstract class ResultCommandHandler<TDomain, TOutput>(
    IValidator<TDomain> validator,
    Func<TDomain, TOutput> fn) : ResultCommandHandler<TDomain, TDomain, TOutput>(new IdTransformer<TDomain>(), validator, fn) {
}

public abstract class ResultCommandHandlerAsync<TDomain, TOutput>(
    IValidator<TDomain> validator,
    Func<TDomain, Task<TOutput>> fn) : ResultCommandHandlerAsync<TDomain, TDomain, TOutput>(new IdTransformer<TDomain>(), validator, fn) {
}

public abstract class CreateEntityCommandHandler<TInput, TDomain>(
    ITransformer<TInput, TDomain> transformer,
    IValidator<TDomain> validator,
    ICreateEntity<TDomain> operation) : UnitResultCommandHandler<TInput, TDomain>(transformer, validator, operation.Create) {
}

public abstract class UpdateEntityCommandHandler<TInput, TDomain>(
    ITransformer<TInput, TDomain> transformer,
    IValidator<TDomain> validator,
    IUpdateEntity<TDomain> operation) : UnitResultCommandHandler<TInput, TDomain>(transformer, validator, operation.Update) {
}

public abstract class SaveEntityCommandHandler<TInput, TDomain>(
    ITransformer<TInput, TDomain> transformer,
    IValidator<TDomain> validator,
    ISaveEntity<TDomain, Unit> operation) : UnitResultCommandHandler<TInput, TDomain>(transformer, validator, operation.Save) {
}

public abstract class DeleteEntityCommandHandler<TInput, TDomain>(
    ITransformer<TInput, TDomain> transformer,
    IValidator<TDomain> validator,
    IDeleteEntity<TDomain> operation) : UnitResultCommandHandler<TInput, TDomain>(transformer, validator, operation.Delete) {
}

public abstract class CreateEntityCommandHandler<TInput, TDomain, TOutput>(
    ITransformer<TInput, TDomain> transformer,
    IValidator<TDomain> validator,
    ICreateEntity<TDomain, TOutput> operation) : ResultCommandHandler<TInput, TDomain, TOutput>(transformer, validator, operation.Create) {
}

public abstract class UpdateEntityCommandHandler<TInput, TDomain, TOutput>(
    ITransformer<TInput, TDomain> transformer,
    IValidator<TDomain> validator,
    IUpdateEntity<TDomain, TOutput> operation) : ResultCommandHandler<TInput, TDomain, TOutput>(transformer, validator, operation.Update) {
}

public abstract class SaveEntityCommandHandler<TInput, TDomain, TOutput>(
    ITransformer<TInput, TDomain> transformer,
    IValidator<TDomain> validator,
    ISaveEntity<TDomain, TOutput> operation) : ResultCommandHandler<TInput, TDomain, TOutput>(transformer, validator, operation.Save) {
}

public abstract class DeleteEntityCommandHandler<TInput, TDomain, TOutput>(
    ITransformer<TInput, TDomain> transformer,
    IValidator<TDomain> validator,
    IDeleteEntity<TDomain, TOutput> operation) : ResultCommandHandler<TInput, TDomain, TOutput>(transformer, validator, operation.Delete) {
}

public abstract class CreateEntityCommandHandlerAsync<TInput, TDomain>(
    ITransformer<TInput, TDomain> transformer,
    IValidator<TDomain> validator,
    ICreateEntityAsync<TDomain> operation) : UnitResultCommandHandlerAsync<TInput, TDomain>(transformer, validator, operation.CreateAsync) {
}

public abstract class UpdateEntityCommandHandlerAsync<TInput, TDomain>(
    ITransformer<TInput, TDomain> transformer,
    IValidator<TDomain> validator,
    IUpdateEntityAsync<TDomain> operation) : UnitResultCommandHandlerAsync<TInput, TDomain>(transformer, validator, operation.UpdateAsync) {
}

public abstract class SaveEntityCommandHandlerAsync<TInput, TDomain>(
    ITransformer<TInput, TDomain> transformer,
    IValidator<TDomain> validator,
    ISaveEntityAsync<TDomain, Unit> operation) : UnitResultCommandHandlerAsync<TInput, TDomain>(transformer, validator, operation.SaveAsync) {
}

public abstract class DeleteEntityCommandHandlerAsync<TInput, TDomain>(
    ITransformer<TInput, TDomain> transformer,
    IValidator<TDomain> validator,
    IDeleteEntityAsync<TDomain> operation) : UnitResultCommandHandlerAsync<TInput, TDomain>(transformer, validator, operation.DeleteAsync) {
}

public abstract class CreateEntityCommandHandlerAsync<TInput, TDomain, TOutput>(
    ITransformer<TInput, TDomain> transformer,
    IValidator<TDomain> validator,
    ICreateEntityAsync<TDomain, TOutput> operation) : ResultCommandHandlerAsync<TInput, TDomain, TOutput>(transformer, validator, operation.CreateAsync) {
}

public abstract class UpdateEntityCommandHandlerAsync<TInput, TDomain, TOutput>(
    ITransformer<TInput, TDomain> transformer,
    IValidator<TDomain> validator,
    IUpdateEntityAsync<TDomain, TOutput> operation) : ResultCommandHandlerAsync<TInput, TDomain, TOutput>(transformer, validator, operation.UpdateAsync) {
}

public abstract class SaveEntityCommandHandlerAsync<TInput, TDomain, TOutput>(
    ITransformer<TInput, TDomain> transformer,
    IValidator<TDomain> validator,
    ISaveEntityAsync<TDomain, TOutput> operation) : ResultCommandHandlerAsync<TInput, TDomain, TOutput>(transformer, validator, operation.SaveAsync) {
}

public abstract class DeleteEntityCommandHandlerAsync<TInput, TDomain, TOutput>(
    ITransformer<TInput, TDomain> transformer,
    IValidator<TDomain> validator,
    IDeleteEntityAsync<TDomain, TOutput> operation) : ResultCommandHandlerAsync<TInput, TDomain, TOutput>(transformer, validator, operation.DeleteAsync) {
}

public abstract class GetEntityQueryHandler<T, TKey>(
    IGetEntity<T, TKey> operation) : IQueryHandler<TKey, Option<T>> {
    public Option<T> Handle(TKey query) => operation.Get(query);
}

public abstract class GetEntityQueryHandlerAsync<T, TKey>(
    IGetEntityAsync<T, TKey> operation) : IQueryHandlerAsync<TKey, Option<T>> {
    public Task<Option<T>> HandleAsync(TKey query) => operation.GetAsync(query);
}

public abstract class ListEntityQueryHandler<T>(
    IListEntity<T> operation) : IQueryHandler<IEnumerable<T>> {
    public IEnumerable<T> Handle() => operation.List();
}

public abstract class ListEntityQueryHandler<T, TFilter>(
    IListEntity<T, TFilter> operation) : IQueryHandler<TFilter, IEnumerable<T>> {
    public IEnumerable<T> Handle(TFilter query) => operation.List(query);
}

public abstract class ListEntityQueryHandlerAsync<T>(
    IListEntityAsync<T> operation) : IQueryHandlerAsync<IEnumerable<T>> {
    public Task<IEnumerable<T>> Handle() => operation.ListAsync();
}

public abstract class ListEntityQueryHandlerAsync<T, TFilter>(
    IListEntityAsync<T, TFilter> operation) : IQueryHandlerAsync<TFilter, IEnumerable<T>> {
    public Task<IEnumerable<T>> HandleAsync(TFilter query) => operation.ListAsync(query);
}

internal sealed class IdTransformer<T> : ITransformer<T, T> {
    public Result<T, ResultErrors> Transform(T input) =>
        Result<T, ResultErrors>.Ok(input);
}
