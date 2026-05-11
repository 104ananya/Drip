using System.Data;
using Dapper;

namespace Drip.Data;

// Teaches Dapper how to handle DateOnly (Dapper doesn't support it natively)
public class DateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly>
{
    // When SENDING DateOnly to the database → convert to DateTime
    public override void SetValue(IDbDataParameter parameter, DateOnly value)
    {
        parameter.DbType = DbType.Date;
        parameter.Value = value.ToDateTime(TimeOnly.MinValue);
    }

    // When READING from the database → handle both DateOnly and DateTime
    public override DateOnly Parse(object value)
    {
        if (value is DateOnly dateOnly)
            return dateOnly;

        return DateOnly.FromDateTime((DateTime)value);
    }
}
