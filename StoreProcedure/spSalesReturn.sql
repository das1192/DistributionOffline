-- Author  : Yeasin Ahmed  
-- Created Date : 20-May-2019  
-- Purpose  : To return a sale  
  
Alter Procedure [dbo].[spSalesReturn]  
@InvNo varchar(100),  
@InvOID bigint,  
@StoreId Varchar(10),  
@UserId Varchar(50)  
As  
Begin  
 SET NOCOUNT ON;    
 Begin Transaction [Trans1]  
  
 Begin Try  
  IF OBJECT_ID('tempdb..#tempJournal') IS NOT NULL DROP TABLE #tempJournal  
  IF OBJECT_ID('tempdb..#tempTbl') IS NOT NULL DROP TABLE #tempTbl  
  if((Select Count(Approved) From SalesReturn Where InvoiceNo=@InvNo And Approved=0 And StoreID=@StoreId) > 0)  
  Begin  
   Update SalesReturn Set Approved=1,EUSER= @UserId,EDAT=CAST(GETDATE() as Date)  
   Where OID=@InvOID  
     
   update T_SALES_MST set DropStatus=1,EUSER=@UserId,EDAT=CAST(GETDATE() as Date)   
   where InvoiceNo=@InvNo  
     
   Delete from CASHINOUT where INVOICEID=@InvNo  
     
   -- Special Variable--    
   Declare @RowCount int,@i int;  
   ---- start. Work With Journal -----  
   --AccountID,Branch,Particular,Remarks,Debit,Credit,RefferenceNumber,IDAT,IDATTIME,Narration  
   --,Particular ASC,Narration ASC,IDATTIME ASC  
   Create Table #tempJournal  
   (  
    RowNum int,  
    AccountID Nvarchar(50) Null,  
    Branch varchar(50) Null,  
    Particular varchar(100) Null,  
    Remarks varchar(200) Null,  
    Debit Bigint,  
    Credit Bigint,  
    RefferenceNumber varchar(100) Null,  
    Narration varchar(200) Null  
   )  
     
   Insert Into #tempJournal  
   Select ROW_NUMBER() Over(Order By Journal_OID ASC) RowNum,AccountID,Branch,Particular,Remarks,Debit,Credit,RefferenceNumber,Narration From Acc_Journal Where RefferenceNumber=@InvNo And Branch=@StoreId  
   
   Declare @AccountID Nvarchar(50),@Particular varchar(100),@Remarks varchar(200),@Debit Bigint,@Credit Bigint,@RefNum varchar(100),@Narration varchar(200);  
     
   Set @RowCount= (Select COUNT(*) From #tempJournal)  
   Set @i = 1;  
    
   While(@i <= @RowCount)  
   Begin  
    Select @AccountID=AccountID,@Particular=Particular,@Remarks=Remarks,@Debit=Credit,@Credit=Debit,@RefNum=RefferenceNumber,@Narration=Narration+' (Return From Journal)' From #tempJournal Where RowNum = @i  
       
     Insert Into Acc_Journal(AccountID,Branch,Particular,Remarks,Debit,Credit,RefferenceNumber,Narration,IDAT,IDATTIME)  
       
     Values(@AccountID,@StoreId,@Particular,@Remarks,@Debit,@Credit,@RefNum,@Narration,Cast(GETDATE() As Date),GETDATE())  
    Set @i =@i+1;  
   End  
   ---- End. Work With Journal -----  
   Set @RowCount=0;  
   Set @i=1;  
   ---- Start.Working Area ----  
   Create Table #tempTbl  
   (  
    RowNum int,  
    NetAmount decimal null,  
    ShopName Varchar(200) Null,  
    PCategoryID Bigint null,  
    PCategoryName Varchar(100) null,  
    SubCategoryID Bigint null,  
    SubCategory Varchar(100) null,  
    DescriptionID Bigint null,  
    [Description]Varchar(100) null,  
    QtyPcs bigint null,  
    CostPrice Bigint null,  
    SalePrice Bigint null,  
    GiftAmount Varchar(100) null,  
    Discount Bigint null,  
    TotalSalePrice decimal null,  
    Barcode Varchar(100) null,  
    SalesReturnDate Date Null,  
    InvoiceNo Varchar(100) Null,  
    CashBalance Bigint null,  
    CustomerName varchar(100) null,  
    CustomerID Bigint Null,  
    IUSER Varchar(100) Null,  
    PaymentModeID int null,  
    PaymentMode varchar(50) null,  
    IDAT Date  
   )  
     
   Insert Into #tempTbl  
   Select  ROW_NUMBER() Over(Order By sd.PROD_WGPG,sd.PROD_SUBCATEGORY,sd.PROD_DES,sd.Branch) RowNum,NetAmount=sm.NetAmount,ShopName=shp.ShopName  
   ,PCategoryID=sd.PROD_WGPG,PCategoryName=c.WGPG_NAME  
   ,SubCategoryID=sd.PROD_SUBCATEGORY,SubCategory=sc.SubCategoryName  
   ,DescriptionID=sd.PROD_DES,[Description]=d.[Description]  
   ,QtyPcs=sd.Quantity  
   ,CostPrice=sd.CostPrice  
   ,SalePrice=sd.SalePrice  
   ,GiftAmount= case when sd.Remarks='Gift' then sd.SalePrice else '0' end  
   ,Discount=sd.Discount   
   ,TotalSalePrice=(sd.Quantity*sd.SalePrice)  
   ,Barcode=sd.Barcode  
   ,SalesReturnDate=sm.IDAT  
   ,InvoiceNo=sd.Po_Number  
   ,CashBalance=(select isnull(Balance,0) from vw_Shopwise_Cash_Balance where Branch=@StoreId)  
   ,CustomerName=(select top 1 c.CustomerName from CustomerInformation c where c.InvoiceNo=sm.InvoiceNo)  
   ,CustomerID=(select top 1 c.OID from CustomerInformation c where c.InvoiceNo=sm.InvoiceNo)  
   ,IUSER=sm.IUSER  
   ,PaymentModeID=sm.PaymentModeID,PaymentMode=pay.PaymentMode,IDAT=sm.IDAT  
     
   from Acc_StockDetail sd  
   inner join T_SALES_MST sm on sm.InvoiceNo=sd.Po_Number  
   inner join PaymentMode pay on pay.OID=sm.PaymentModeID  
   inner join ShopInfo shp on shp.OID=sm.StoreID  
   inner join T_WGPG c on c.OID=sd.PROD_WGPG  
   inner join Description d on d.OID=sd.PROD_DES   
   inner join SubCategory sc on sc.OID=sd.PROD_SUBCATEGORY   
   where sm.SlNo=(select top 1 SlNo from T_SALES_MST where  T_SALES_MST.StoreID=@StoreId and T_SALES_MST.InvoiceNo =@InvNo order by IDAT desc)  
     
   Declare @CategoryID Bigint,@SubCategoryID Bigint,@DescriptionID Bigint,@SaleQty int,@PURCHASECOST int,@SalePrice int,@TOTAL int,@TotalSalePrice int,@Barcode nvarchar(100),@IUSER Varchar(50),@IDAT Date,@GiftAmt Bigint,@DiscountAmt Bigint  
     
   Set @RowCount= (Select COUNT(*) From #tempTbl)  
   While(@i <= @RowCount)  
   Begin  
    Declare @stockID int;  
     
    Select @CategoryID=PCategoryID,@SubCategoryID=SubCategoryID,@DescriptionID=DescriptionID,@SaleQty=QtyPcs,@PURCHASECOST=CostPrice,@SalePrice=SalePrice,@TOTAL=(CostPrice*QtyPcs),@TotalSalePrice=TotalSalePrice,@Barcode=Barcode,@IUSER=IUSER,@IDAT =IDAT,@GiftAmt=Cast(ISNULL(GiftAmount,0) as bigint),@DiscountAmt=Cast(ISNULL(Discount,0) as Bigint)From #tempTbl Where RowNum = @i  
      
    Insert into Acc_StockDetail(Po_Number,PROD_WGPG,PROD_SUBCATEGORY,PROD_DES,Branch,Quantity,CostPrice,SalePrice,Total,Flag,Remarks,IDAT,IUSER,IDATTIME) values(@InvNo,@CategoryID,@SubCategoryID,@DescriptionID,@StoreId,@SaleQty,@PURCHASECOST,@SalePrice,@TOTAL,'Sale Return','Sale Return',@IDAT,@IUSER,GETDATE())  
      
    If(ISNULL(RTRIM(@Barcode),'') !='')  
    Begin  
     update Acc_Stock set Quantity = (Quantity + @SaleQty),SQty=(SQty-@SaleQty)  
     ,SRQty=(SRQty+@SaleQty )  
     where ISNULL(Flag,'')='' and PROD_DES = @DescriptionID AND Branch=@StoreId   
     AND CostPrice=@PURCHASECOST  
    End  
    Else  
    Begin  
     Select top 1 @stockID = ISNULL(ACC_STOCKID,0) from Acc_Stock   
     Where Flag ='Quantity' AND Quantity >0   
     and Branch=@StoreId and PROD_WGPG=@CategoryID and PROD_SUBCATEGORY=@SubCategoryID   
     and PROD_DES=@DescriptionID AND CostPrice =@PURCHASECOST   
     Order by ACC_STOCKID asc  
       
  
     IF(ISNULL(@stockID,0)=0)  
     BEGIN  
      Select top 1 @stockID=ACC_STOCKID from Acc_Stock   
      Where Flag ='Quantity' AND Quantity =0 and Branch=@StoreId   
      and PROD_WGPG=@CategoryID and PROD_SUBCATEGORY=@SubCategoryID   
      and PROD_DES=@DescriptionID AND CostPrice =@PURCHASECOST    
      Order by ACC_STOCKID desc  
     END  
     update Acc_Stock   
     set Quantity=(Quantity+@SaleQty),SRQty=(SRQty+@SaleQty),  
     SQty=(SQty-@SaleQty)   
     where ACC_STOCKID =@stockID  
    End  
      
    declare @CostingHeadID bigint  
    If(@DiscountAmt > 0)  
    Begin  
     select @CostingHeadID =(select OID from CostingHead   
     where Shop_id =@StoreId and CostingHead ='Discount On Sales')  
         
     insert into DailyCost(Shop_id,CostingHeadID,AMOUNT,IUSER,IDAT,Remarks,ReferenceNo)   
     values (@StoreId,@CostingHeadID,(0-@DiscountAmt),@IUSER,Cast(GETDATE() as Date),'Discount Returned','Discount Returned')  
    End  
    If(@GiftAmt > 0)  
    Begin  
     select @CostingHeadID =(select OID from CostingHead   
     where Shop_id =@StoreId and CostingHead ='Expense For Gift')  
       
     Insert into DailyCost(Shop_id,CostingHeadID,AMOUNT,IUSER,IDAT,Remarks,ReferenceNo) values (@StoreId,@CostingHeadID,(0-@TotalSalePrice),@IUSER,Cast(GETDATE() as Date),'Gift Returned','Gift Returned')  
    End  
      
    IF((SELECT Count(OID) from StoreMasterStock where PROD_WGPG=@CategoryID and PROD_SUBCATEGORY=@SubCategoryID and PROD_DES=@DescriptionID and Branch=@StoreId) > 0)  
    BEGIN  
     Declare @StoreMasterOID Bigint =0  
       
     SELECT Top 1 @StoreMasterOID=OID from StoreMasterStock where PROD_WGPG=@CategoryID   
     and PROD_SUBCATEGORY=@SubCategoryID and PROD_DES=@DescriptionID and Branch=@StoreId  
       
     update StoreMasterStock set SaleQuantity=SaleQuantity-@SaleQty  
     where OID=@StoreMasterOID  
        
     INSERT INTO StockPosting(BranchOID,DescriptionOID,Barcode,InwardQty,OutwardQty,Particulars,IUSER,IDAT) VALUES(@StoreId,@DescriptionID,@Barcode,@SaleQty,0,'Sale Return',@UserId,Cast(GETDATE() as Date))  
       
     -- Barcode --  
     IF(RTRIM(@Barcode)!='' AND @Barcode IS NOT NULL)  
     BEGIN  
      update T_STOCK set SaleStatus=0   
      where IDAT>'26 Sep 2017' and SaleStatus='1' and PROD_WGPG=@CategoryID   
      and PROD_SUBCATEGORY=@SubCategoryID and PROD_DES=@DescriptionID and Branch=@StoreId   
      and Barcode=@Barcode  
       
      update T_PROD set SaleStatus=0   
      where IDAT>'26 Sep 2017' and SaleStatus='1' and PROD_WGPG=@CategoryID  
      and PROD_SUBCATEGORY=@SubCategoryID and PROD_DES=@DescriptionID   
      and Branch=@StoreId and Barcode=@Barcode  
     END  
          
    END     
      
    Set @i = @i+1;  
   End  
   ---- End.Working Area ----  
  End   
    
  DROP TABLE #tempJournal;  
  DROP TABLE #tempTbl;  
  Commit Transaction [Trans1];  
 End Try  
 Begin Catch  
  RollBack Transaction [Trans1]  
 End Catch  
End  