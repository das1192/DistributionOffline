using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ValidationConstants
/// </summary>
public static class ValidationConstants
{

    public static class RegularExpression
    {
        public const string Email = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
        public const string Web = @"^((ht|f)tp(s?)\:\/\/|~/|/)?([\w]+:\w+@)?([a-zA-Z]{1}([\w\-]+\.)+([\w]{2,5}))(:[\d]{1,5})?((/?\w+/)+|/?)(\w+\.[\w]{3,4})?((\?\w+=\w+)?(&\w+=\w+)*)?";
        public const string Name = @"^[A-Z a-z \. \-]*$";
        public const string Phone = @"^[+]?([0-9 ( ) \s \-]{6,19})([^-+ a-zA-Z/(/)])$";
        public const string PhoneExtension = @"^[0-9]{2,4}$";
        public const string Mobile = @"^[+]?([0-9 ( ) \s \-]{6,19})([^-+ a-zA-Z/(/)])$";
        public const string ZipCode = @"^[A-Z a-z 0-9]{1,1}([A-Z a-z 0-9 \-]{3,9})";
        public const string Experience = "[0-9]{1,2}((.1[0-1])|(.[1-9]))?";
        public const string FaxNo = @"^[+]?([0-9 ( ) \s \-]{6,19})([^-+ a-zA-Z/(/)])$";
        public const string Photo = @"(.*\.[jJ][pP][gG])|(.*\.[jJ][pP][eE][gG])|(.*\.[gG][iI][fF])|(.*\.[pP][nN][gG])";
        public const string File = @"(.*\.[dD][oO][cC][xX])|(.*\.[dD][oO][cC])|(.*\.[rR][tT][fF])|(.*\.[tT][xX][tT])|(.*\.[zZ][iI][pP])|(.*\.[pP][dD][fF])";
        public const string Salary = @"([0-9]+\.[0-9]+|[0-9]+)";
        public const string GPA = @"^(\d+(\.\d{1,2}))|([0-9]{1,5})$";
        public const string Number = @"^[0-9]{1,2}";
        public const string DecimalNumber = @"[-+]?[0-9]*\.?[0-9]+";
        public const string Date_MM_dd_yyyy = @"^(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d";
        public const string Date_dd_MM_yyyy = @"^(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d";
        public const string Social_Security_Number = @"^\d{3}-\d{2}-\d{4}$";
        public const string Password = @"^(?!^[0-9]*$)(?!^[a-zA-Z]*$)^([a-zA-Z0-9]{8,10})$";
        public const string National_Id_Number = @"^\d{13}$";

        //public const string Social_Security_Number = @"^\d{3}-\d{2}-\d{4}$";
    }

    public static class FieldLength
    {
        
    }

    public static class ErrorMessage
    {
        public const string REQUIRED = "Required";
        public const string NUMERIC = "Numeric value only";
        public const string INVALID_DATE = "Invalid date";
        public const string INVALID_DATE_FORMAT = "Invalid date(MM/dd/yyyy)";
        public const string INVALID_DATE_FORMAT_Local = "Invalid date(dd/MM/yyyy)";
        //public const string Required = "Required";
    }

    public static class DataTypeRange
    {
        public const string Decimal_Max = @"79228162514264337593543950335";

    }
}
