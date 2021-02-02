-- Author  : Yeasin Ahmed  
-- Created Date : 14-May-2019  
-- Purpose  : To Execute SalesDetails   
  
Alter Procedure [dbo].[spInsertSalesDetails]  
@ShopId int,  
@InvoiceNo VarChar(50),  
@CategoryId Bigint,  
@DescriptionID Bigint,  
@SubCategoryID Bigint,  
@Barcode VarChar(50)=NULL,  
@SalePrice Decimal,  
@SaleQty int,  
@DiscountAmount Bigint,  
@ReturnQty int,  
@GiftAmount Decimal,  
@Narration varchar(150),  
--@DiscountReferenceOID Bigint=0,  
@IUser varchar(50),  
@OID Bigint,  
@Customer_Name varchar(100)=null  
As  
Begin  
 SET NOCOUNT ON;    
 Begin Transaction [Trans1]  
 Begin Try  
  IF OBJECT_ID('tempdb..#tempAccStock') IS NOT NULL DROP TABLE #tempAccStock  
  --BLOCK-1  
  -- Sales Detail Entry (DAL METHOD NAME: T_SALES_DTL_Add)  
  insert into T_SALES_DTL(InvoiceNo,DescriptionID,Barcode,SalePrice,SaleQty,DiscountReferenceOID,DiscountAmount,ReturnQty,IDAT,GiftAmount) values(@InvoiceNo,@DescriptionID,@Barcode,@SalePrice,@SaleQty,0,@DiscountAmount,@ReturnQty,CAST(GETDATE() AS DATE),@GiftAmount)  
   
  declare @IDDiscountOnSales NVARCHAR(10),@CopyOfSaleQty int -- Variable  
  Set @CopyOfSaleQty = @SaleQty  
    
select @IDDiscountOnSales = (select ch.OID from CostingHead ch where ch.CostingHead='Discount On Sales' and ch.Shop_id=@ShopId)  
    
  If(@DiscountAmount > 0) -- Discount  
  Begin  
   insert into DailyCost(Shop_id,CostingHeadID,AMOUNT,IUSER,IDAT,Remarks)   
   values (@ShopId,@IDDiscountOnSales,@DiscountAmount,@IUser,CAST(GETDATE() AS DATE) ,'Discount')  
  End   
    
  INSERT INTO StockPosting(BranchOID,DescriptionOID,Barcode,InwardQty,OutwardQty,Particulars,IUSER,IDAT) VALUES(@ShopId,@DescriptionID,@Barcode,0,@SaleQty,'Sale',@IUSER,CAST(GETDATE() AS DATE))   
   
  update StoreMasterStock set SaleQuantity = (SaleQuantity + @SaleQty) where OID = @OID  
  PRINT('BLOCK-1 Executed Successfully (Sales Detail Entry)')  
  --End of Sales Detail Entry (DAL METHOD NAME: T_SALES_DTL_Add)  
      
  declare @costpricefirst NVARCHAR(100),@Credit Bigint,@Debit Bigint,@giftAmt int -- Variable  
    
  --BLOCK-2  
  ------------ Working With Temp Table----------------  
  Create Table #tempAccStock  
  (  
   OID Bigint,  
   ACC_STOCKID Bigint,  
   CostPrice Bigint,  
   Quantity Bigint  
  )  
    
  If(RTRIM(@Barcode) !='' AND @Barcode IS NOT NULL)  
  Begin  
   Insert Into #tempAccStock  
   select bc.OID,s.ACC_STOCKID,bc.CostPrice,s.Quantity from T_PROD bc  
   inner join Acc_Stock s on s.CostPrice=bc.CostPrice And bc.Branch=s.Branch  
   where ISNULL(s.Flag,'')='' and s.Quantity>0 and bc.SaleStatus='0'   
   and bc.Branch=@ShopId and bc.Barcode=@Barcode  
   order by s.ACC_STOCKID  
  End  
    
  ELSE IF(RTRIM(@Barcode)='' OR @Barcode IS NULL)  
  Begin  
   Insert Into #tempAccStock  
   select 0 'OID',s.ACC_STOCKID,s.CostPrice, s.Quantity  
   from Acc_Stock s  
   where s.Flag='Quantity' and s.Quantity>0 and s.Branch=@ShopId and s.PROD_WGPG=@CategoryId   
   and s.PROD_SUBCATEGORY=@SubCategoryID and s.PROD_DES=@DescriptionID  
   order by s.ACC_STOCKID  
  End  
  PRINT('BLOCK-2 Executed Successfully (#tempAccStock)')   
  ------------ End of Temp Table Execuation----------------  
  SET @costpricefirst = Cast((Select Top 1 CostPrice From #tempAccStock) as NVARCHAR(100))  
    
  --BLOCK-3  
  --------- Update Account Stock ---------  
  ------------- Acc_Journal (T_SALES_DTL_Journal)--------------  
  --BLOCK-3.1  
  IF(@GiftAmount > 0) -- Gift  
  BEGIN  
   Set @Credit= (Cast(@costpricefirst as int)* @SaleQty)  
   Set @giftAmt = (Cast(@costpricefirst as int)* @SaleQty)  
     
   -- Insert Daily Cost --  
   declare @IDExpenseForGift Bigint  
   select @IDExpenseForGift=(select ch.OID from CostingHead ch   
   where ch.CostingHead='Expense For Gift' and ch.Shop_id=@ShopId)  
     
   insert into DailyCost(Shop_id,CostingHeadID,AMOUNT,IUSER,IDAT,Remarks)   
   values (@ShopId,@IDExpenseForGift,@giftAmt,@IUser,CAST(GETDATE() AS DATE),'Gift')  
   PRINT('BLOCK-3.1 Executed Successfully (Insert Daily Cost)')  
  END  
    
  ELSE  
  BEGIN  
   Set @Credit= (Cast(@SalePrice as int)* @SaleQty)  
  END  
    
  --BLOCK-3.2  
  insert into Acc_Journal(AccountID,Branch,Particular,Remarks,Debit,Credit,RefferenceNumber,IDAT,IDATTIME,Narration) values (@DescriptionID,@ShopId,'Sale','Product',0,@Credit,@InvoiceNo,CAST(GETDATE() AS DATE) ,GETDATE(),@Narration)  
  PRINT('BLOCK-3.2 Executed Successfully (Acc_Journal)')  
  ------- End of Acc_Journal (T_SALES_DTL_Journal)-------  
    
  --BLOCK-3.3  
  Declare @differenceQty int, @total int,@updateWithQty int,@PURCHASECOST int,@PURCHASECOSTOID Bigint,@Discount Bigint=0,@Remarks VarChar(50)='Sale',@SaleAmt bigint -- Variable  
    
   Set @total = @SaleQty;  
   Set @differenceQty = ((Select Top 1 Quantity From #tempAccStock)-@total);  
     
   If(@differenceQty >= 0 )  
   Begin  
    Set @updateWithQty = @differenceQty  
   End  
   Else  
   Begin  
    Set @updateWithQty=0  
   End  
     
   Set @total = ABS(@differenceQty)  
   Set @PURCHASECOST = Cast((Select Top 1 CostPrice From #tempAccStock) as int)  
   Set @PURCHASECOSTOID = (Select Top 1 ACC_STOCKID From #tempAccStock)  
     
   IF(@DiscountAmount > 0)  
   BEGIN  
    Set @Discount= @DiscountAmount --Default value is 0  
   END  
     
   IF(@GiftAmount >0)  
   Begin  
    Set @SaleAmt = @PURCHASECOST  
    Set @Remarks ='Gift' --Default value is Sale  
   End  
   ELSE  
   BEGIN  
    Set @SaleAmt= @SalePrice  
   END  
     
   ---Start: DAL METHOD NAME: T_SALES_DTL_StockNew  
   if(@updateWithQty >= 0 AND @total >0)  
   BEGIN  
    -- UPDATE Acc_Stock   
    UPDATE Acc_Stock SET Quantity = @updateWithQty,SQty=SQty+@SaleQty    
    WHERE ACC_STOCKID=(Select TOP 1 ACC_STOCKID From #tempAccStock)  
    
    If( (Select Quantity From Acc_Stock Where ACC_STOCKID =(Select Top 1 ACC_STOCKID From #tempAccStock))> 0)  
    BEGIN  
     insert into Acc_StockDetail(Po_Number,PROD_WGPG,PROD_SUBCATEGORY,PROD_DES,Branch,Quantity,CostPrice,SalePrice,Total,Discount,Flag,Remarks,IDAT,IUSER,IDATTIME,Barcode) values(@InvoiceNo,@CategoryID,@SubCategoryID,@DescriptionID,@ShopId,@SaleQty,@PURCHASECOST,@SaleAmt,(@PURCHASECOST*@SaleQty),@Discount,'Sale',@Remarks,CAST(GETDATE() AS DATE),@IUSER,GETDATE(),@Barcode)  
          
    END  
    ELSE  
    BEGIN   
     Set @SaleQty=Cast((Select Top 1 Quantity From #tempAccStock) as int)  
     insert into Acc_StockDetail(Po_Number,PROD_WGPG,PROD_SUBCATEGORY,PROD_DES,Branch,Quantity,CostPrice,SalePrice,Total,Discount,Flag,Remarks,IDAT,IUSER,IDATTIME,Barcode) values(@InvoiceNo,@CategoryID,@SubCategoryID,@DescriptionID,@ShopId,@SaleQty,@PURCHASECOST,@SaleAmt,(@PURCHASECOST*@SaleQty),@Discount,'Sale',@Remarks,CAST(GETDATE() AS DATE) ,@IUSER,GETDATE(),@Barcode)  
       
    END  
    PRINT('BLOCK-3.3 Executed Successfully')  
   END  
     
   ElSE  
   BEGIN  
    -- UPDATE Acc_Stock   
    UPDATE Acc_Stock SET Quantity = @updateWithQty,SQty=SQty+@SaleQty    
    WHERE ACC_STOCKID=(Select TOP 1 ACC_STOCKID From #tempAccStock)  
       
    Set @SaleQty=Cast((Select Top 1 Quantity From #tempAccStock) as int)  
      
    insert into Acc_StockDetail(Po_Number,PROD_WGPG,PROD_SUBCATEGORY,PROD_DES,Branch,Quantity,CostPrice,SalePrice,Total,Discount,Flag,Remarks,IDAT,IUSER,IDATTIME,Barcode) values(@InvoiceNo,@CategoryID,@SubCategoryID,@DescriptionID,@ShopId,@SaleQty,@PURCHASECOST,@SaleAmt,(@PURCHASECOST*@SaleQty),@Discount,'Sale',@Remarks,CAST(GETDATE() AS DATE) ,@IUSER,GETDATE(),@Barcode)  
    PRINT('BLOCK-3.3 Executed Successfully')  
   END  
   ---End: DAL METHOD NAME: T_SALES_DTL_StockNew  
   PRINT('BLOCK-3 Has been done successfully..')  
    
  ------ End Of Update Account Stock -----  
  Declare @LedgerAccID Bigint=0,@PassedAmount NVARCHAR(100);  
    
  ----------- Check For Gift (Journal For Gift Discount) -------------  
  --BLOCK-04  
  IF(@Narration ='Gift Product to Customer')  
  BEGIN  
   IF((select Count(ch.OID) from CostingHead ch where ch.CostingHead='Expense For Gift' and ch.Shop_id=@ShopId)> 0)  
   BEGIN  
    Set @LedgerAccID=(select ch.OID from CostingHead ch where ch.CostingHead='Expense For Gift' and ch.Shop_id=@ShopId)  
   END  
     
   Set @PassedAmount=@costpricefirst  
     
   insert into Acc_Journal(AccountID,Branch,Customer_Name,Particular,Remarks,Debit,Credit,RefferenceNumber,IDAT,IDATTIME,Narration) values (@LedgerAccID,@ShopId,@Customer_Name,'Expense','Expense',((CAST(@PassedAmount as bigint))*@SaleQty),0,@InvoiceNo,CAST(GETDATE() AS DATE) ,GETDATE(),'Expense for Gift')  
  PRINT('BLOCK-4 Executed Successfully')    
     
  END  
  ----------- End Of Check For Gift (Journal For Gift Discount) ----------  
  ----------- Check If the sale item is barcode item -----------------  
  --BLOCK-05  
  If(RTRIM(@Barcode) !='' AND @Barcode IS NOT NULL)  
  BEGIN  
   update T_PROD   
   set SaleStatus=1,SalesDate=CAST(GETDATE() AS DATE)     
   where  SaleStatus=0 and Barcode=@Barcode and Branch=@ShopId  
     
   update T_STOCK     
   set SaleStatus=1,SalesDate=CAST(GETDATE() AS DATE)     
   where  SaleStatus=0 and Barcode=@Barcode and Branch=@ShopId  
     
   PRINT('BLOCK-5 Executed Successfully')  
  END  
    
  ----------- End Of Check If the sale item is barcode item -----------  
      
    
  DROP TABLE #tempAccStock;  
  Commit Transaction [Trans1];  
 End Try  
   
 Begin Catch  
  RollBack Transaction [Trans1]  
 End Catch  
End  