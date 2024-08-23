# Danom

Danom is a C# library that provides monadic structures to simplify functional programming patterns in C#.

## Key Features
- Implementation of common monads like `Option`, `Result`, and `ResultOption`.
- Fluent API for chaining operations.
- Error handling with monads.
- Integration with async/await for asynchronous operations.

## Design Goals
- **Simplicity**: Easy to use API for common monadic operations.
- **Performance**: Efficient implementation to minimize overhead.
- **Interoperability**: Seamless integration with existing C# code and libraries.
- **Extensibility**: Allow users to create custom monads.

## Quick Start

## Options

Represents when an actual value might not exist for a value or named variable. An option has an underlying type and can hold a value of that type, or it might not have a value.

### Creating Options

```csharp
var option = Option<int>.Some(5);
// or, with no value
var optionNone = Option<int>.None();
```

### Using Options

Options are commonly used when a operation does not return a value.

```csharp
public IOption<int> TryFind(IEnumerable<int> numbers, Func<int, bool> predicate) =>
    numbers.FirstOrDefault(predicate).ToOption();
```

With this method defined we can begin performing operations against the Option result:

```csharp
IEnumerable<int> nums = [1,2,3];

// Exhasutive matching
TryFind(nums, x => x == 1)
    .Match(
        some: x => Console.WriteLine("Found: {0}", x),
        none: () => Console.WriteLine("Did not find number"));

// Mapping the value
var optionSum =
    TryFind(nums, x => x == 1)
        .Map(x => x + 1);

// Binding the option
var optionBindSum =
    TryFind(nums, x => x == 1)
        .Bind(num1 =>
            TryFind(nums, x => x == 2)
                .Map(num2 => num1 + num2));

// Handling "None"
var optionDefault =
    TryFind(nums, x => x == 4)
        .DefaultValue(99);

var optionDefaultWith =
    TryFind(nums, x => x == 4)
        .DefaultWith(() => 99); // useful if creating the value is expensive

var optionOrElse =
    TryFind(nums, x => x == 4)
        .OrElse(Option.Some(99));

var optionOrElseWith =
    TryFind(nums, x => x == 4)
        .OrElseWith(() => 99);
```

## Results

## ResultOptions

## Contribute

Thank you for considering contributing to Danom, and to those who have already contributed! We appreciate (and actively resolve) PRs of all shapes and sizes.

We kindly ask that before submitting a pull request, you first submit an [issue](https://github.com/pimbrouwers/Danom/issues) or open a [discussion](https://github.com/pimbrouwers/Danom/discussions).

If functionality is added to the API, or changed, please kindly update the relevant [document](https://github.com/pimbrouwers/Danom/tree/master/docs). Unit tests must also be added and/or updated before a pull request can be successfully merged.

Only pull requests which pass all build checks and comply with the general coding guidelines can be approved.

If you have any further questions, submit an [issue](https://github.com/pimbrouwers/Danom/issues) or open a [discussion](https://github.com/pimbrouwers/Danom/discussions).


## Find a bug?

There's an [issue](https://github.com/pimbrouwers/Danom/issues) for that.

## License

Built with ♥ by [Pim Brouwers](https://github.com/pimbrouwers) in Toronto, ON. Licensed under [Apache License 2.0](https://github.com/pimbrouwers/Danom/blob/master/LICENSE).
