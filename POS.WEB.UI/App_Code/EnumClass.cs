using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for EnumClass
/// </summary>
public static class EnumClass
{

    public enum UserRole
    {
        User=1,
        PurchaseManager=2,
        Authority=3,
        AccountManager = 4
    }

    public enum LocationID 
    {
        RequisitionSender=1,
        PurchaseManager=2,
        Authority=3, 
        WorkOrder=4,
        AccountManager=5
    }

    public enum AproveStatusForPurchase
    {
        Approved=1,
        Rejected=2,
        OnHold=3,
        UnApproved=4,
        PreApproved=5,
        ApproveByAccountManager=6
    }

    public enum POStatus
    {     
        Normal=1,
        Cancel = 2,
        OnHold = 3,
        PurchaseOrder = 4,
        InAccounts=5
    }

    public enum ReceiveStatus
    {
        Normal=1,
        Cancel = 2,
        OnHold = 3,
        Received=4
    }

    public enum IssueStatus
    {
         Issued=1,
        Cancel = 2,
        OnHold = 3       
    }
    public enum RequisitionStatus
    {
        AccountManager=4,
        Authority=3,
        PurchaseManager=2,
        Original=1
    }
}