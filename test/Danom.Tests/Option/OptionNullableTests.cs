namespace Danom.Tests;

using Danom.TestHelpers;
using Xunit;

public sealed class OptionNullableExtensionsTests
{
    [Fact]
    public void ValueCollections()
    {
        char[] charList = ['a', 'b', 'c', 'd'];
        byte[] byteList = [0, 1, 2, 3];
        short[] shortList = [0, 1, 2, 3];
        int[] intList = [0, 1, 2, 3];
        long[] longList = [0L, 1L, 2L, 3L];
        decimal[] decimalList = [0m, 1m, 2m, 3m];
        double[] doubleList = [0.0, 1.0, 2.0, 3.0];
        float[] floatList = [0f, 1f, 2f, 3f];
        Guid[] guidList = [Guid.Empty, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()];
        DateTime[] dateTimeList = [DateTime.MinValue, DateTime.UtcNow, DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(2)];
        DateOnly[] dateOnlyList = [DateOnly.MinValue, DateOnly.FromDateTime(DateTime.UtcNow), DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)), DateOnly.FromDateTime(DateTime.UtcNow.AddDays(2))];

        AssertOption.IsSome('a', charList.FirstOrDefault(x => x == 'a').ToOption());
        AssertOption.IsSome((byte)0, byteList.FirstOrDefault(x => x == 0).ToOption());
        AssertOption.IsSome((short)0, shortList.FirstOrDefault(x => x == 0).ToOption());
        AssertOption.IsSome(0, intList.FirstOrDefault(x => x == 0).ToOption());
        AssertOption.IsSome(0L, longList.FirstOrDefault(x => x == 0L).ToOption());
        AssertOption.IsSome(0m, decimalList.FirstOrDefault(x => x == 0m).ToOption());
        AssertOption.IsSome(0.0, doubleList.FirstOrDefault(x => x == 0.0).ToOption());
        AssertOption.IsSome(0f, floatList.FirstOrDefault(x => x == 0f).ToOption());
        AssertOption.IsSome(Guid.Empty, guidList.FirstOrDefault(x => x == Guid.Empty).ToOption());
        AssertOption.IsSome(DateTime.MinValue, dateTimeList.FirstOrDefault(x => x == DateTime.MinValue).ToOption());
        AssertOption.IsSome(DateOnly.MinValue, dateOnlyList.FirstOrDefault(x => x == DateOnly.MinValue).ToOption());

    }

    [Fact]
    public void EmptyValueCollections()
    {
        char[] charList = [];
        byte[] byteList = [];
        short[] shortList = [];
        int[] intList = [];
        long[] longList = [];
        decimal[] decimalList = [];
        double[] doubleList = [];
        float[] floatList = [];
        Guid[] guidList = [];
        DateTime[] dateTimeList = [];
        DateOnly[] dateOnlyList = [];

        AssertOption.IsSome(charList.FirstOrDefault(x => x == 'a').ToOption());
        AssertOption.IsSome(byteList.FirstOrDefault(x => x == 0).ToOption());
        AssertOption.IsSome(shortList.FirstOrDefault(x => x == 0).ToOption());
        AssertOption.IsSome(intList.FirstOrDefault(x => x == 0).ToOption());
        AssertOption.IsSome(longList.FirstOrDefault(x => x == 0L).ToOption());
        AssertOption.IsSome(decimalList.FirstOrDefault(x => x == 0m).ToOption());
        AssertOption.IsSome(doubleList.FirstOrDefault(x => x == 0.0).ToOption());
        AssertOption.IsSome(floatList.FirstOrDefault(x => x == 0f).ToOption());
        AssertOption.IsSome(guidList.FirstOrDefault(x => x == Guid.Empty).ToOption());
        AssertOption.IsSome(dateTimeList.FirstOrDefault(x => x == DateTime.MinValue).ToOption());
        AssertOption.IsSome(dateOnlyList.FirstOrDefault(x => x == DateOnly.MinValue).ToOption());
    }

    [Fact]
    public void ConversionsFromNull()
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
        // Assert.Null(nullableDateOnly.ToOption().ToNullable());
        Assert.Null(nullableObj.ToOption().ToNullable());
        Assert.Null(objFunc().ToOption().ToNullable());
    }

    [Fact]
    public void ConversionsFromDefault()
    {
        char? nullableChar = default;
        bool? nullableBool = default;
        byte? nullableByte = default;
        short? nullableShort = default;
        int? nullableInt = default;
        long? nullableLong = default;
        decimal? nullableDecimal = default;
        double? nullableDouble = default;
        float? nullableFloat = default;
        Guid? nullableGuid = default;
        DateTime? nullableDateTime = default;
        DateOnly? nullableDateOnly = default;
        object? nullableObj = default;
        Func<object?> objFunc = () => default;

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
        // Assert.Null(nullableDateOnly.ToOption().ToNullable());
        Assert.Null(nullableObj.ToOption().ToNullable());
        Assert.Null(objFunc().ToOption().ToNullable());
    }

    [Fact]
    public void ConversionsFromValue()
    {
        char? nullableChar = char.MinValue;
        bool? nullableBool = true;
        byte? nullableByte = byte.MinValue;
        short? nullableShort = short.MinValue;
        int? nullableInt = int.MinValue;
        long? nullableLong = long.MinValue;
        decimal? nullableDecimal = decimal.MinValue;
        double? nullableDouble = double.MinValue;
        float? nullableFloat = float.MinValue;
        Guid? nullableGuid = Guid.Empty;
        DateTime? nullableDateTime = DateTime.MinValue;
        DateOnly? nullableDateOnly = DateOnly.MinValue;
        object? nullableObj = new object();
        Func<object?> objFunc = () => new object();

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
        // Assert.NotNull(nullableDateOnly.ToOption().ToNullable());
        Assert.NotNull(nullableObj.ToOption().ToNullable());
        Assert.NotNull(objFunc.ToOption().ToNullable());
    }

    [Fact]
    public void ConversionsFromNonNullableValue()
    {
        char nullableChar = char.MinValue;
        bool nullableBool = true;
        byte nullableByte = byte.MinValue;
        short nullableShort = short.MinValue;
        int nullableInt = int.MinValue;
        long nullableLong = long.MinValue;
        decimal nullableDecimal = decimal.MinValue;
        double nullableDouble = double.MinValue;
        float nullableFloat = float.MinValue;
        Guid nullableGuid = Guid.Empty;
        DateTime nullableDateTime = DateTime.MinValue;
        DateOnly nullableDateOnly = DateOnly.MinValue;
        object nullableObj = new object();
        Func<object> objFunc = () => new object();

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
        // Assert.NotNull(nullableDateOnly.ToOption().ToNullable());
        Assert.NotNull(nullableObj.ToOption().ToNullable());
        Assert.NotNull(objFunc.ToOption().ToNullable());
    }
}
