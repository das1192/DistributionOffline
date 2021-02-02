using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Enums
/// </summary>
/// 

public static class Enums
{
    public enum ProcurementMethod : int
    {
        Tender = 1,       // Procurement By Tender
        Enquiry = 2,      // Procurement By Enquiry
        Proprietary = 3   // Procurement By Proprietary
    }

    public enum ProcurementSource : int
    {
        Local = 7,    // Local Purchase 
        Foreign = 8    // Foreign Purchase 
    }
    public enum LeaveApplicationStatus : int
    {
        Pending = 1,
        Approved = 2,
        Cancelled = 3
    }
}
