-- Author  : Yeasin Ahmed  
-- Created Date : 14-May-2019  
-- Purpose  : To Execute Sales Master Information  
  
Alter Procedure [dbo].[spInsertSalesMaster]  
@StoreID VarChar(100),  
@InvoiceNo VarChar(100),  
@PaymentModeID int,  
@BankInfoOID int,  
@SubTotal decimal(18,2),  
@Discount decimal(18,2),  
@DiscountReferencedBy VarChar(100),  
@NetAmount decimal(18,2),  
@ReceiveAmount decimal(18,2),  
@CashPaid decimal(18,2),  
@CashChange decimal(18,2),  
@StuffID VarChar(50),   
@IUSER VarChar(50),   
@Remarks VarChar(100),  
@CustomerName VarChar(100),  
@Address VarChar(100),  
@MobileNo VarChar(50),  
@AlternativeMobileNo VarChar(50),  
@DateOfBirth VarChar(50),  
@EmailAddress VarChar(100),  
@BankID int,  
@CardAmt int  
As  
Begin  
 SET NOCOUNT ON;    
 --SET STATISTICS IO ON    
 --SET STATISTICS TIME ON    
 --SET STATISTICS PROFILE ON    
 Begin Transaction [Trans1]  
 BEGIN TRY  
  --BLOCK-1  
  --Cash  
  --Card  
  --Cash & Card  
  --- Check If there is nothing with the InvoiceNo in Sales Details Table ---  
  IF((Select COUNT(SlNo) From T_SALES_DTL Where InvoiceNo=@InvoiceNo)=0)  
  Begin  
   Return;  
  End    
  ---------------------------------------------------------------------------  
  ELSE  
  Begin  
   IF(@PaymentModeID=11) --Cash  
   BEGIN  
    insert into Acc_Journal(AccountID,Branch,Customer_Name,Particular,Remarks,Debit,Credit,RefferenceNumber,IDAT,IDATTIME,Narration) values (1,@StoreID,@CustomerName,'Cash','Cash',CAST(@ReceiveAmount as bigint),0,@InvoiceNo,GETDATE(),GETDATE(),'Cash Received from Customer')   
   END  
     
   ELSE IF(@PaymentModeID=12) -- Card  
   BEGIN  
    insert into Acc_Journal(AccountID,Branch,Customer_Name,Particular,Remarks,Debit,Credit,RefferenceNumber,IDAT,IDATTIME,Narration) values (@BankID,@StoreID,@CustomerName,'Bank','Bank(Card)',CAST(@ReceiveAmount as bigint),0,@InvoiceNo,CAST(GETDATE() AS DATE),GETDATE(),'Received through Card from Customer')   
   END  
     
   ELSE IF(@PaymentModeID=14)-- Cash & Card   
   BEGIN  
    Declare @debitAmt bigint;  
    Set @debitAmt = (CAST(@NetAmount as bigint)-CAST(@ReceiveAmount as bigint))  
    insert into Acc_Journal(AccountID,Branch,Customer_Name,Particular,Remarks,Debit,Credit,RefferenceNumber,IDAT,IDATTIME,Narration) values (1,@StoreID,@CustomerName,'Cash','Cash',CAST(@ReceiveAmount as bigint),0,@InvoiceNo,CAST(GETDATE() AS DATE),GETDATE
(),'Cash Received from Customer')   
      
    insert into Acc_Journal(AccountID,Branch,Particular,Remarks,Debit,Credit,RefferenceNumber,IDAT,IDATTIME,Narration) values (@BankID,@StoreID,'Bank','Bank(Card)',@CardAmt,0,@InvoiceNo,CAST(GETDATE() AS DATE),GETDATE(),'Received through Card from Customer')    
   END  
   PRINT('BLOCK-1 Executed Successfully')   
     
   --BLOCK-2  
   IF(ISNUMERIC(@Discount) = 1 AND (CAST(@Discount as int)) > 0)  
   BEGIN  
    Declare @AccountID int=0;  
    IF((select ch.OID from CostingHead ch where ch.CostingHead='Discount On Sales' and ch.Shop_id=@StoreID)!='' AND (select ch.OID from CostingHead ch where ch.CostingHead='Discount On Sales' and ch.Shop_id=@StoreID) is not null)  
    BEGIN  
     Set @AccountID= CAST((select ch.OID from CostingHead ch   
     where ch.CostingHead='Discount On Sales' and ch.Shop_id=@StoreID) as int)  
    END   
      
    insert into Acc_Journal(AccountID,Branch,Customer_Name,Particular,Remarks,Debit,Credit,RefferenceNumber,IDAT,IDATTIME,Narration) values (@AccountID,@StoreID,@CustomerName,'Expense','Expense',@Discount,0,@InvoiceNo,CAST(GETDATE() AS DATE),GETDATE(),'Expense for Discount')   
   PRINT('BLOCK-2 Executed Successfully')  
   END  
     
     
   --BLOCK-3  
   insert into T_SALES_MST(StoreID,InvoiceNo,PaymentModeID,BankInfoOID,SubTotal,Discount,DiscountReferencedBy,NetAmount,ReceiveAmount,CashPaid,CashChange,DropStatus,StuffID,IUSER,EUSER,IDAT,EDAT,Remarks)    
   values(@StoreID,@InvoiceNo,@PaymentModeID,@BankInfoOID,@SubTotal,@Discount,@DiscountReferencedBy,@NetAmount,@ReceiveAmount,@CashPaid,@CashChange,0,@StuffID,@IUSER,@IUSER,(CAST(GETDATE() AS DATE)),(CAST(GETDATE() AS DATE)),@Remarks)    
     
   insert into CASHINOUT(Branch,CashIN,IUSER,IDAT,INVOICEID,PAYMENTMODE)    
   values(@StoreID,@NetAmount,@IUSER,(CAST(GETDATE() AS DATE)),@InvoiceNo,@PaymentModeID)    
      
   insert into CustomerInformation(InvoiceNo,CustomerName,[Address],MobileNo,AlternativeMobileNo,DateOfBirth,EmailAddress)    
   values(@InvoiceNo,@CustomerName,@Address,@MobileNo,@AlternativeMobileNo,@DateOfBirth,@EmailAddress)  
   PRINT('BLOCK-3 Executed Successfully')  
  End  
    
    
  Commit Transaction [Trans1];  
 END TRY  
 BEGIN CATCH  
  RollBack Transaction [Trans1];  
 END CATCH   
End  