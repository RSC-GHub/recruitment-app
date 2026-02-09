namespace Recruitment.Domain.Enums.Reports
{
    public enum ReportParameterType
    {
        String = 1,       // nvarchar, varchar, char, nchar, text
        NString = 2,      // nvarchar specifically for Unicode
        Char = 3,
        NChar = 4,
        Text = 5,
        NText = 6,

        Int = 7,          // int
        BigInt = 8,       // bigint
        SmallInt = 9,     // smallint
        TinyInt = 10,     // tinyint
        Decimal = 11,     // decimal, numeric
        Money = 12,       // money
        SmallMoney = 13,  // smallmoney
        Float = 14,       // float
        Real = 15,        // real

        Date = 16,        // date
        Time = 17,        // time
        DateTime = 18,    // datetime
        DateTime2 = 19,   // datetime2
        SmallDateTime = 20, // smalldatetime
        DateTimeOffset = 21, // datetimeoffset

        Boolean = 22,     // bit

        Binary = 23,      // binary
        VarBinary = 24,   // varbinary
        Image = 25        // image
    }

}
