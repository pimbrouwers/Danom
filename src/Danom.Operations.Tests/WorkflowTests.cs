namespace Danom.Operations.Tests;

using Danom.TestHelpers;
using Danom.Validation;
using Xunit;

public sealed class WorkflowTests {
    [Fact]
    public void ResultCommandHandlerUsesValidatedValue() {
        var handler = new TestResultCommandHandler(new DelegateValidator<TestCommand>(_ =>
            Result<TestCommand, ResultErrors>.Ok(new TestCommand(42))),
            command => command.Value);

        AssertResult.IsOk(42, handler.Handle(new TestCommand(1)));
    }

    [Fact]
    public void ResultCommandHandlerReturnsErrorWhenValidationFails() {
        var called = false;
        var handler = new TestResultCommandHandler(new DelegateValidator<TestCommand>(_ =>
            Result<TestCommand, ResultErrors>.Error(new ResultErrors("invalid"))),
            _ => {
                called = true;
                return 1;
            });

        var result = handler.Handle(new TestCommand(1));
        AssertResult.IsError(result);
        Assert.False(called);
    }

    [Fact]
    public async Task ResultCommandHandlerAsyncUsesValidatedValue() {
        var handler = new TestResultCommandHandlerAsync(new DelegateValidator<TestCommand>(_ =>
            Result<TestCommand, ResultErrors>.Ok(new TestCommand(42))),
            command => Task.FromResult(command.Value));

        var result = await handler.HandleAsync(new TestCommand(1));
        AssertResult.IsOk(42, result);
    }

    [Fact]
    public async Task ResultCommandHandlerAsyncReturnsErrorWhenValidationFails() {
        var called = false;
        var handler = new TestResultCommandHandlerAsync(new DelegateValidator<TestCommand>(_ =>
            Result<TestCommand, ResultErrors>.Error(new ResultErrors("invalid"))),
            _ => {
                called = true;
                return Task.FromResult(1);
            });

        var result = await handler.HandleAsync(new TestCommand(1));
        AssertResult.IsError(result);
        Assert.False(called);
    }

    [Fact]
    public void CreateEntityCommandHandlerCallsCreate() {
        var store = new EntityStore();
        var handler = new TestCreateEntityCommandHandler(new PassValidator(), store);

        AssertResult.IsOk(handler.Handle(new TestCommand(7)));
        Assert.Equal(7, store.LastCreatedValue);
    }

    [Fact]
    public void UpdateEntityCommandHandlerCallsUpdate() {
        var store = new EntityStore();
        var handler = new TestUpdateEntityCommandHandler(new PassValidator(), store);

        AssertResult.IsOk(handler.Handle(new TestCommand(7)));
        Assert.Equal(7, store.LastUpdatedValue);
    }

    [Fact]
    public void SaveEntityCommandHandlerCallsSave() {
        var store = new EntityStore();
        var handler = new TestSaveEntityCommandHandler(new PassValidator(), store);

        AssertResult.IsOk(handler.Handle(new TestCommand(7)));
        Assert.Equal(7, store.LastSavedValue);
    }

    [Fact]
    public void DeleteEntityCommandHandlerCallsDelete() {
        var store = new EntityStore();
        var handler = new TestDeleteEntityCommandHandler(new PassValidator(), store);

        AssertResult.IsOk(handler.Handle(new TestCommand(7)));
        Assert.Equal(7, store.LastDeletedValue);
    }

    [Fact]
    public async Task CreateEntityCommandHandlerAsyncCallsCreateAsync() {
        var store = new EntityStore();
        var handler = new TestCreateEntityCommandHandlerAsync(new PassValidator(), store);

        var result = await handler.HandleAsync(new TestCommand(7));
        AssertResult.IsOk(result);
        Assert.Equal(7, store.LastCreatedAsyncValue);
    }

    [Fact]
    public async Task UpdateEntityCommandHandlerAsyncCallsUpdateAsync() {
        var store = new EntityStore();
        var handler = new TestUpdateEntityCommandHandlerAsync(new PassValidator(), store);

        var result = await handler.HandleAsync(new TestCommand(7));
        AssertResult.IsOk(result);
        Assert.Equal(7, store.LastUpdatedAsyncValue);
    }

    [Fact]
    public async Task SaveEntityCommandHandlerAsyncCallsSaveAsync() {
        var store = new EntityStore();
        var handler = new TestSaveEntityCommandHandlerAsync(new PassValidator(), store);

        var result = await handler.HandleAsync(new TestCommand(7));
        AssertResult.IsOk(result);
        Assert.Equal(7, store.LastSavedAsyncValue);
    }

    [Fact]
    public async Task DeleteEntityCommandHandlerAsyncCallsDeleteAsync() {
        var store = new EntityStore();
        var handler = new TestDeleteEntityCommandHandlerAsync(new PassValidator(), store);

        var result = await handler.HandleAsync(new TestCommand(7));
        AssertResult.IsOk(result);
        Assert.Equal(7, store.LastDeletedAsyncValue);
    }

    [Fact]
    public void CreateEntityCommandHandlerWithOutputCallsCreate() {
        var store = new EntityStore();
        var handler = new TestCreateEntityCommandHandlerWithOutput(new PassValidator(), store);

        var result = handler.Handle(new TestCommand(5));
        AssertResult.IsOk(10, result);
    }

    [Fact]
    public async Task CreateEntityCommandHandlerAsyncWithOutputCallsCreateAsync() {
        var store = new EntityStore();
        var handler = new TestCreateEntityCommandHandlerAsyncWithOutput(new PassValidator(), store);

        var result = await handler.HandleAsync(new TestCommand(5));
        AssertResult.IsOk(10, result);
    }

    [Fact]
    public void ListEntityQueryHandlerReturnsList() {
        var store = new QueryStore();
        var handler = new TestListEntityQueryHandler(store);

        Assert.Equal([1, 2, 3], handler.Handle().ToArray());
    }

    [Fact]
    public void ListEntityQueryHandlerWithFilterReturnsFilteredList() {
        var store = new QueryStore();
        var handler = new TestListEntityQueryHandlerWithFilter(store);

        Assert.Equal([4], handler.Handle(4).ToArray());
    }

    [Fact]
    public void GetEntityQueryHandlerReturnsOption() {
        var store = new QueryStore();
        var handler = new TestGetEntityQueryHandler(store);

        var result = handler.Handle(2);
        Assert.True(result.IsSome);
    }

    [Fact]
    public async Task ListEntityQueryHandlerAsyncReturnsList() {
        var store = new QueryStore();
        var handler = new TestListEntityQueryHandlerAsync(store);
        Assert.Equal([1, 2, 3], (await handler.Handle()).ToList());
    }

    [Fact]
    public async Task ListEntityQueryHandlerAsyncWithFilterReturnsFilteredList() {
        var store = new QueryStore();
        var handler = new TestListEntityQueryHandlerAsyncWithFilter(store);
        Assert.Equal([4], (await handler.HandleAsync(4)).ToList());
    }

    [Fact]
    public async Task GetEntityQueryHandlerAsyncReturnsOption() {
        var store = new QueryStore();
        var handler = new TestGetEntityQueryHandlerAsync(store);

        var result = await handler.HandleAsync(2);
        Assert.True(result.IsSome);
    }

    private sealed record TestCommand(int Value);

    private sealed class DelegateValidator<T>(Func<T, Result<T, ResultErrors>> validate) : IValidator<T> {
        public Result<T, ResultErrors> Validate(T value) => validate(value);
    }

    private sealed class PassValidator : IValidator<TestCommand> {
        public Result<TestCommand, ResultErrors> Validate(TestCommand value) => Result<TestCommand, ResultErrors>.Ok(value);
    }

    private sealed class IdentityTransformer : ITransformer<TestCommand, TestCommand> {
        public Result<TestCommand, ResultErrors> Transform(TestCommand input) => Result<TestCommand, ResultErrors>.Ok(input);
    }

    private sealed class TestResultCommandHandler : ResultCommandHandler<TestCommand, int> {
        public TestResultCommandHandler(IValidator<TestCommand> validator, Func<TestCommand, int> fn)
            : base(validator, fn) { }
    }

    private sealed class TestResultCommandHandlerAsync : ResultCommandHandlerAsync<TestCommand, int> {
        public TestResultCommandHandlerAsync(IValidator<TestCommand> validator, Func<TestCommand, Task<int>> fn)
            : base(validator, fn) { }
    }

    private sealed class TestCreateEntityCommandHandler : CreateEntityCommandHandler<TestCommand, TestCommand> {
        public TestCreateEntityCommandHandler(IValidator<TestCommand> validator, ICreateEntity<TestCommand> store)
            : base(new IdentityTransformer(), validator, store) { }
    }

    private sealed class TestUpdateEntityCommandHandler : UpdateEntityCommandHandler<TestCommand, TestCommand> {
        public TestUpdateEntityCommandHandler(IValidator<TestCommand> validator, IUpdateEntity<TestCommand> store)
            : base(new IdentityTransformer(), validator, store) { }
    }

    private sealed class TestSaveEntityCommandHandler : SaveEntityCommandHandler<TestCommand, TestCommand> {
        public TestSaveEntityCommandHandler(IValidator<TestCommand> validator, ISaveEntity<TestCommand, Unit> store)
            : base(new IdentityTransformer(), validator, store) { }
    }

    private sealed class TestDeleteEntityCommandHandler : DeleteEntityCommandHandler<TestCommand, TestCommand> {
        public TestDeleteEntityCommandHandler(IValidator<TestCommand> validator, IDeleteEntity<TestCommand> store)
            : base(new IdentityTransformer(), validator, store) { }
    }

    private sealed class TestCreateEntityCommandHandlerAsync : CreateEntityCommandHandlerAsync<TestCommand, TestCommand> {
        public TestCreateEntityCommandHandlerAsync(IValidator<TestCommand> validator, ICreateEntityAsync<TestCommand> store)
            : base(new IdentityTransformer(), validator, store) { }
    }

    private sealed class TestUpdateEntityCommandHandlerAsync : UpdateEntityCommandHandlerAsync<TestCommand, TestCommand> {
        public TestUpdateEntityCommandHandlerAsync(IValidator<TestCommand> validator, IUpdateEntityAsync<TestCommand> store)
            : base(new IdentityTransformer(), validator, store) { }
    }

    private sealed class TestSaveEntityCommandHandlerAsync : SaveEntityCommandHandlerAsync<TestCommand, TestCommand> {
        public TestSaveEntityCommandHandlerAsync(IValidator<TestCommand> validator, ISaveEntityAsync<TestCommand, Unit> store)
            : base(new IdentityTransformer(), validator, store) { }
    }

    private sealed class TestDeleteEntityCommandHandlerAsync : DeleteEntityCommandHandlerAsync<TestCommand, TestCommand> {
        public TestDeleteEntityCommandHandlerAsync(IValidator<TestCommand> validator, IDeleteEntityAsync<TestCommand> store)
            : base(new IdentityTransformer(), validator, store) { }
    }

    private sealed class TestCreateEntityCommandHandlerWithOutput : CreateEntityCommandHandler<TestCommand, TestCommand, int> {
        public TestCreateEntityCommandHandlerWithOutput(IValidator<TestCommand> validator, ICreateEntity<TestCommand, int> store)
            : base(new IdentityTransformer(), validator, store) { }
    }

    private sealed class TestCreateEntityCommandHandlerAsyncWithOutput : CreateEntityCommandHandlerAsync<TestCommand, TestCommand, int> {
        public TestCreateEntityCommandHandlerAsyncWithOutput(IValidator<TestCommand> validator, ICreateEntityAsync<TestCommand, int> store)
            : base(new IdentityTransformer(), validator, store) { }
    }

    private sealed class EntityStore :
        ICreateEntity<TestCommand>,
        IUpdateEntity<TestCommand>,
        ISaveEntity<TestCommand, Unit>,
        IDeleteEntity<TestCommand>,
        ICreateEntityAsync<TestCommand>,
        IUpdateEntityAsync<TestCommand>,
        ISaveEntityAsync<TestCommand, Unit>,
        IDeleteEntityAsync<TestCommand>,
        ICreateEntity<TestCommand, int>,
        ICreateEntityAsync<TestCommand, int> {

        public int LastCreatedValue { get; private set; }
        public int LastUpdatedValue { get; private set; }
        public int LastSavedValue { get; private set; }
        public int LastDeletedValue { get; private set; }
        public int LastCreatedAsyncValue { get; private set; }
        public int LastUpdatedAsyncValue { get; private set; }
        public int LastSavedAsyncValue { get; private set; }
        public int LastDeletedAsyncValue { get; private set; }

        public Unit Create(TestCommand entity) {
            LastCreatedValue = entity.Value;
            return Unit.Value;
        }

        public Unit Update(TestCommand entity) {
            LastUpdatedValue = entity.Value;
            return Unit.Value;
        }

        public Unit Save(TestCommand entity) {
            LastSavedValue = entity.Value;
            return Unit.Value;
        }

        public Unit Delete(TestCommand entity) {
            LastDeletedValue = entity.Value;
            return Unit.Value;
        }

        public Task<Unit> CreateAsync(TestCommand entity) {
            LastCreatedAsyncValue = entity.Value;
            return Unit.ValueAsync;
        }

        public Task<Unit> UpdateAsync(TestCommand entity) {
            LastUpdatedAsyncValue = entity.Value;
            return Unit.ValueAsync;
        }

        public Task<Unit> SaveAsync(TestCommand entity) {
            LastSavedAsyncValue = entity.Value;
            return Unit.ValueAsync;
        }

        public Task<Unit> DeleteAsync(TestCommand entity) {
            LastDeletedAsyncValue = entity.Value;
            return Unit.ValueAsync;
        }

        int ICreateEntity<TestCommand, int>.Create(TestCommand entity) => entity.Value * 2;

        Task<int> ICreateEntityAsync<TestCommand, int>.CreateAsync(TestCommand entity) => Task.FromResult(entity.Value * 2);
    }

    private sealed class TestListEntityQueryHandler : ListEntityQueryHandler<int> {
        public TestListEntityQueryHandler(IListEntity<int> store)
            : base(store) { }
    }

    private sealed class TestListEntityQueryHandlerWithFilter : ListEntityQueryHandler<int, int> {
        public TestListEntityQueryHandlerWithFilter(IListEntity<int, int> store)
            : base(store) { }
    }

    private sealed class TestGetEntityQueryHandler : GetEntityQueryHandler<int, int> {
        public TestGetEntityQueryHandler(IGetEntity<int, int> store)
            : base(store) { }
    }

    private sealed class TestListEntityQueryHandlerAsync : ListEntityQueryHandlerAsync<int> {
        public TestListEntityQueryHandlerAsync(IListEntityAsync<int> store)
            : base(store) { }
    }

    private sealed class TestListEntityQueryHandlerAsyncWithFilter : ListEntityQueryHandlerAsync<int, int> {
        public TestListEntityQueryHandlerAsyncWithFilter(IListEntityAsync<int, int> store)
            : base(store) { }
    }

    private sealed class TestGetEntityQueryHandlerAsync : GetEntityQueryHandlerAsync<int, int> {
        public TestGetEntityQueryHandlerAsync(IGetEntityAsync<int, int> store)
            : base(store) { }
    }

    private sealed class QueryStore :
        IListEntity<int>,
        IListEntity<int, int>,
        IGetEntity<int, int>,
        IListEntityAsync<int>,
        IListEntityAsync<int, int>,
        IGetEntityAsync<int, int> {

        public IEnumerable<int> List() => [1, 2, 3];

        public IEnumerable<int> List(int filter) => [filter];

        public Option<int> Get(int key) => Option.Some(key);

        public Task<IEnumerable<int>> ListAsync() => Task.FromResult<IEnumerable<int>>([1, 2, 3]);

        public Task<IEnumerable<int>> ListAsync(int filter) => Task.FromResult<IEnumerable<int>>([filter]);

        public Task<Option<int>> GetAsync(int key) => Task.FromResult(Option.Some(key));
    }
}
