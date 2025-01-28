using System.Data;
using System.Globalization;
using Dapper;

namespace JobManager.Framework.Infrastructure.Abstractions;

internal sealed class EnumTypeHandler<T> : SqlMapper.TypeHandler<T> where T : struct, Enum
{
    public override void SetValue(IDbDataParameter parameter, T value) =>
        parameter.Value = Convert.ToString(value, CultureInfo.InvariantCulture);

    public override T Parse(object value)
    {
        string? stringValue = Convert.ToString(value, CultureInfo.InvariantCulture);
        return (stringValue is null)? default:Enum.Parse<T>(stringValue);
    }
}
