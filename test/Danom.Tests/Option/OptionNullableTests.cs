namespace Danom.Tests;

using Xunit;
using Danom.TestHelpers;

public sealed class OptionNullableExtensionsTests
{
    [Fact]
    public void ConversionsShouldWork()
    {
        char? nullableChar = null;
        bool? nullableBool = null;
        byte? nullableByte = null;
        short? nullableShort = null;
        int? nullableInt = null;
        long? nullableLong = null;
        decimal? nullableDecimal = null;
        double? nullableDouble = null;
        float? nullableFloat = null;
        Guid? nullableGuid = null;
        DateTime? nullableDateTime = null;
        DateOnly? nullableDateOnly = null;
        object? nullableObj = null;
        Func<object?> objFunc = () => null;

        AssertOption.IsNone(nullableChar.ToOption());
        AssertOption.IsNone(nullableBool.ToOption());
        AssertOption.IsNone(nullableByte.ToOption());
        AssertOption.IsNone(nullableShort.ToOption());
        AssertOption.IsNone(nullableInt.ToOption());
        AssertOption.IsNone(nullableLong.ToOption());
        AssertOption.IsNone(nullableDecimal.ToOption());
        AssertOption.IsNone(nullableDouble.ToOption());
        AssertOption.IsNone(nullableFloat.ToOption());
        AssertOption.IsNone(nullableGuid.ToOption());
        AssertOption.IsNone(nullableDateTime.ToOption());
        AssertOption.IsNone(nullableDateOnly.ToOption());
        AssertOption.IsNone(nullableObj.ToOption());
        AssertOption.IsNone(objFunc().ToOption());
        Assert.Null(nullableChar.ToOption().ToNullable());
        Assert.Null(nullableBool.ToOption().ToNullable());
        Assert.Null(nullableByte.ToOption().ToNullable());
        Assert.Null(nullableShort.ToOption().ToNullable());
        Assert.Null(nullableInt.ToOption().ToNullable());
        Assert.Null(nullableLong.ToOption().ToNullable());
        Assert.Null(nullableDecimal.ToOption().ToNullable());
        Assert.Null(nullableDouble.ToOption().ToNullable());
        Assert.Null(nullableFloat.ToOption().ToNullable());
        Assert.Null(nullableGuid.ToOption().ToNullable());
        Assert.Null(nullableDateTime.ToOption().ToNullable());
        Assert.Null(nullableDateOnly.ToOption().ToNullable());
        Assert.Null(nullableObj.ToOption().ToNullable());
        Assert.Null(objFunc().ToOption().ToNullable());

        nullableChar = Char.MinValue;
        nullableBool = true;
        nullableByte = Byte.MinValue;
        nullableShort = short.MinValue;
        nullableInt = int.MinValue;
        nullableLong = long.MinValue;
        nullableDecimal = decimal.MinValue;
        nullableDouble = double.MinValue;
        nullableFloat = float.MinValue;
        nullableGuid = Guid.Empty;
        nullableDateTime = DateTime.MinValue;
        nullableDateOnly = DateOnly.MinValue;
        nullableObj = new object();
        objFunc = () => new object();

        AssertOption.IsSome(nullableChar.ToOption());
        AssertOption.IsSome(nullableBool.ToOption());
        AssertOption.IsSome(nullableByte.ToOption());
        AssertOption.IsSome(nullableShort.ToOption());
        AssertOption.IsSome(nullableInt.ToOption());
        AssertOption.IsSome(nullableLong.ToOption());
        AssertOption.IsSome(nullableDecimal.ToOption());
        AssertOption.IsSome(nullableDouble.ToOption());
        AssertOption.IsSome(nullableFloat.ToOption());
        AssertOption.IsSome(nullableGuid.ToOption());
        AssertOption.IsSome(nullableDateTime.ToOption());
        AssertOption.IsSome(nullableDateOnly.ToOption());
        AssertOption.IsSome(nullableObj.ToOption());
        AssertOption.IsSome(objFunc.ToOption());
        Assert.NotNull(nullableChar.ToOption().ToNullable());
        Assert.NotNull(nullableBool.ToOption().ToNullable());
        Assert.NotNull(nullableByte.ToOption().ToNullable());
        Assert.NotNull(nullableShort.ToOption().ToNullable());
        Assert.NotNull(nullableInt.ToOption().ToNullable());
        Assert.NotNull(nullableLong.ToOption().ToNullable());
        Assert.NotNull(nullableDecimal.ToOption().ToNullable());
        Assert.NotNull(nullableDouble.ToOption().ToNullable());
        Assert.NotNull(nullableFloat.ToOption().ToNullable());
        Assert.NotNull(nullableGuid.ToOption().ToNullable());
        Assert.NotNull(nullableDateTime.ToOption().ToNullable());
        Assert.NotNull(nullableDateOnly.ToOption().ToNullable());
        Assert.NotNull(nullableObj.ToOption().ToNullable());
        Assert.NotNull(objFunc.ToOption().ToNullable());
    }
}
