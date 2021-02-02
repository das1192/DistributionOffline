using System;

public static class ContextConstant
{
    #region Common

    public const string PROJECT_NAME = " HRMIS Solution";
    public const string APPLICATION_NAME = ". : :HRMIS : : .";
    public const string MINIMUM_DATE = "01/01/1901";
    public const string MAXIMUM_DATE = "12/31/9999";
    public const string LOGIN_PAGE = "Login.aspx";

    #endregion

    #region DateFormat

    public const string DATE_FORMAT = "MM/dd/yyyy";
    public const string DATE_FORMAT_WITH_DAY = "dddd, MM/dd/yyyy";
    public const string DATE_FORMAT_LOCAL = "dd/MM/yyyy";
    public const string DATE_FORMAT_LOCAL_WITH_DAY = "dddd, dd/MM/yyyy";
    public const string TIME_FORMAT_12_HOUR = "h:mm tt";
    public const string TIME_FORMAT_24_HOUR = "H:mm tt";

    #endregion

    #region Session

    public const string BRANCH_ID_OF_LOGGED_IN_USER = "BRANCH_ID";
    public const string MAIN_DIVISION_ID_OF_LOGGED_IN_USER = "MAIN_DIVISION_ID";
    public const string DIVISION_ID_OF_LOGGED_IN_USER = "DIVISION_ID";
    public const string DEPARTMENT_ID_OF_LOGGED_IN_USER = "DEPARTMENT_ID";
    public const string SECTION_ID_OF_LOGGED_IN_USER = "SECTION_ID";
    public const string USER_ID_OF_LOGGED_IN_USER = "USER_ID";
    public const string EMPLOYEE_ID_OF_LOGGED_IN_USER = "EMPLOYEE_ID";
    public const string SELECTED_EMPLOYEE_ID = "SELECTED_EMPLOYEE_ID";
    public const string SELECTED_ITEM_ID = "SELECTED_ITEM_ID";
    public const string PRESCRIPTION_ID = "PRESCRIPTION_ID";
    public const string CLAIMED_BILL_ID = "CLAIMED_BILL_ID";
    public const string TENDER_ID = "TENDER_ID";
    public const string EMPLOYEE_EDIT_FLAG = "EMPLOYEE_EDIT_FLAG";
    #endregion

    #region Roles

    public const string ROLE_ADMIN = "Admin";
    public const string ROLE_SUPER_ADMIN = "SuperAdmin";
    public const string ROLE_ACCOUNTS_ADMIN = "AccountsAdmin";
    public const string ROLE_INVENTORY_ADMIN = "InventoryAdmin";
    public const string ROLE_HR_ADMIN = "HrAdmin";
    public const string ROLE_PROCUREMENT_ADMIN = "ProcuremntAdmin";
    public const string ROLE_EMPLOYEE = "Employee";
    public const string ROLE_CONSULTANT = "Consultant";
    public const string ROLE_CANDIDATE = "Candidate";
    public const string ROLE_VENDOR = "Vendor";
    public const string ROLE_CLIENT = "Client";
    public const string ROLE_PARTNER = "Partner";

    #endregion

    #region Message

    public const string SAVED_SUCCESS = "Record has been saved successfully";
    public const string UPDATE_SUCCESS = "Record has been updated successfully";
    public const string SAVE_UNSUCCESSFUL = "Record wasn't saved";
    public const string EXISTS = "Record is exists";
    public const string UPDATE_UNSUCCESSFUL = "Record wasn't updated";
    public const string GENERATE_SUCCESS = "Generation completed successfully";
    public const string DOT_HAVE_SAVE_PERMISSION = "You dont have save permission";
    public const string DOT_HAVE_UPDATE_PERMISSION = "You dont have update permission";
    public const string DOT_HAVE_DELETE_PERMISSION = "You dont have delete permission";
    public const string DEBIT_CREDIT_NOT_EQUAL = "Debit credit was't equal";
    public const string PRESS_NULL_VALUE = "Try To Pass Null Value";
    public const string TRANSECTION_REQUIRED = "Transection required";
    #endregion

    public static class Session
    {
        public const string EmployeeWiseLeaveSetup_SearchCondition = "EmployeeWiseLeaveSetup_SearchCondition";
        public const string EmployeeWiseLeaveSetup_OrderClause = "EmployeeWiseLeaveSetup_OrderClause";
        public const string EmployeeList = "EmployeeList";
        public const string EmployeeLeaveApplicationList = "EmployeeLeaveApplicationList";
        public const string VoucherTable = "Voucher";
        public const string VatTaxSecurity = "VatTaxSecurity";
        public const string NetAmount = "NetAmount";
        public const string VoucherCredit = "VoucherCredit";
        public const string VoucherDebit = "VoucherDebit";
        public const string VatTaxSecurityDebit = "VatTaxSecurityDebit";
        public const string VatTaxSecurityCredit = "VatTaxSecurityCredit";
        public const string NetAmountDebit = "NetAmountDebit";
        public const string NetAmountCredit = "NetAmountCredit";
    }

    public static class Messages
    {
        public const string SAVED_SUCCESS = "Record has been saved successfully";
        public const string UPDATE_SUCCESS = "Record has been updated successfully";
        public const string SAVE_UNSUCCESSFUL = "Record wasn't saved";
        public const string UPDATE_UNSUCCESSFUL = "Record wasn't updated";
        public const string GENERATE_SUCCESS = "Generation completed successfully";

        public const string DOT_HAVE_VIEW_PERMISSION = "You don't have view permission";
        public const string DOT_HAVE_SAVE_PERMISSION = "You don't have save permission";
        public const string DOT_HAVE_UPDATE_PERMISSION = "You don't have update permission";
        public const string DOT_HAVE_DELETE_PERMISSION = "You don't have delete permission";

        public const string EXISTS = "Record is exists";
        public const string DEBIT_CREDIT_NOT_EQUAL = "Debit credit was't equal";
        public const string PRESS_NULL_VALUE = "Try To Pass Null Value";
        public const string TRANSECTION_REQUIRED = "Transection required";
    }

    public static class PageTitles
    {

    }
}
public static class CustomQueries
{
    public const string VW_EMPLOYEELIST_GETDDISTINCT_DEPARTMENTS = "Select distinct(DepartmentName),DepartmentId from vw_EmployeeList ";
    public const string VW_EMPLOYEELIST_GETDDISTINCT_DIVISION = "Select distinct(DivisionName),DivisionId from vw_EmployeeList ";
    public const string MinUnitName = "Select distinct(UnitId) , MinUnitName  from InvUnit where MinUnitName is not null ";
    public const string EmpName = "Select EmployeeId , FirstName + '  ' + MiddleName+ ' ' + LastName As EmpName  from HrEmployee ";
    public const string EmpNameByDiv = "Select EmployeeId , FirstName + '  ' + MiddleName+ ' ' + LastName As EmpName  from HrEmployee ";
    public const string PageName = " Select PageId , PageName from Sec_PageList ";
    public const string GetSkillName = "select  SkillId , SkillName from HrSkill  ";

}
public static class Modules
{
    public const string MODULE_ACCOUNTS = "Accounts";
    public const string MODULE_HR = "Human Resource";
    public const string MODULE_INVENTORY = "Inventory";
    public const string MODULE_PROCUREMENT = "Procurement";
    public const string MODULE_ADMIN = "Admin";
    public const string MODULE_REPORTS = "Reports";
    public const string MODULE_MEDICAL = "Medical";
}

