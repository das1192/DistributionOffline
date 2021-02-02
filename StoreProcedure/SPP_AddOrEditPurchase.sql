-- Author: Yeasin Ahmed        
-- Created Date: 12-May-2019        
-- Purpose: Add Or Edit a Product       
ALTER PROCEDURE [dbo].[SPP_AddOrEditPurchase]  
 @OID  varchar(50),          
 @PROD_WGPG  bigint,          
 @PROD_SUBCATEGORY bigint,          
 @PROD_DES bigint,          
 @Vendor_ID int,           
 @Branch varchar(50),          
 @Barcode varchar(50),          
 --@CashTrans varchar(50),          
 @CostPrice bigint,          
 @SalePrice bigint,          
 @Quantity int,          
 @SaleStatus int,           
 @ActiveStatus int,          
 @IUSER varchar(50),          
 @IDAT date,          
 @EDAT date,          
 @PONUMBER varchar(100),          
 @Flag varchar(50)          
AS          
BEGIN           
 SET NOCOUNT ON;          
 BEGIN TRANSACTION [Tran1];          
   
 BEGIN TRY         
 ---------If the Item is barcode item. Added By Yeasin --------          
 --- Start.Page For T_PRODForm.aspx.cs---          
 IF(@Flag='AddNewPurchase')          
 Begin          
  IF(RTRIM(@Barcode) !='' AND @Barcode IS NOT NULL)          
  Begin 
   
   if((Select COUNT(*) From T_PROD Where Barcode=@Barcode And Branch=@Branch And SaleStatus=0 ) > 0)
   Begin
	Return;
   End
   
   INSERT INTO T_PROD(PROD_WGPG,PROD_SUBCATEGORY,PROD_DES,Branch,Barcode,CostPrice,SalePrice,Quantity,SaleStatus,IUSER,IDAT) VALUES(@PROD_WGPG,@PROD_SUBCATEGORY,@PROD_DES,@Branch,@Barcode,@CostPrice,@SalePrice,@Quantity,@SaleStatus,@IUSER,@IDAT)          
            
   INSERT INTO StockPosting(BranchOID,DescriptionOID,Barcode,InwardQty,OutwardQty,Particulars,IUSER,IDAT) VALUES(@Branch,@PROD_DES,@Barcode,@Quantity,0,'Stock In',@IUSER,@IDAT)           
  END          
 End          
           
 ELSE IF(@Flag='EditPurchase') --if (lblOID.Value != string.Empty)          
 Begin
  Declare @CurrentQty int,@NewQuantity int
  IF((SELECT Count(OID) from StoreMasterStock where PROD_WGPG=@PROD_WGPG and PROD_SUBCATEGORY=@PROD_SUBCATEGORY and PROD_DES=@PROD_DES and Branch=@Branch) > 0)
  Begin
    SELECT @CurrentQty=Quantity from StoreMasterStock where PROD_WGPG=@PROD_WGPG and PROD_SUBCATEGORY=@PROD_SUBCATEGORY and PROD_DES=@PROD_DES and Branch=@Branch
    If(@Quantity > @CurrentQty)
    Begin
		set @Quantity = @Quantity-@CurrentQty
		set @NewQuantity = @CurrentQty + @Quantity
    End
    Else If (@Quantity < @CurrentQty)
    Begin
		set @NewQuantity = @CurrentQty - @Quantity
    End
    Else
    Begin
		set @NewQuantity = @CurrentQty
    End
	update StoreMasterStock set Quantity=@NewQuantity where PROD_WGPG=@PROD_WGPG AND PROD_SUBCATEGORY=@PROD_SUBCATEGORY AND PROD_DES=@PROD_DES          
           
	delete from T_STOCK where OID=@OID            
  End          
 End          
 --- End.Page For T_PRODForm.aspx.cs---          
 ---------------------------------------------------------------          
          
 DECLARE @PurchaseDatetime as datetime          
 SET @PurchaseDatetime = GetDate()          
      Declare @OID1 bigint=0          
 select @OID1=OID from StoreMasterStock where PROD_WGPG=@PROD_WGPG and PROD_SUBCATEGORY=@PROD_SUBCATEGORY and PROD_DES=@PROD_DES  and Branch=@Branch          
    
   insert into T_STOCK(Po_Number,PROD_WGPG,PROD_SUBCATEGORY,PROD_DES,Branch,Barcode,CostPrice,SalePrice,Quantity,SaleStatus,IUSER,IDAT,Vendor_ID) values(@PONUMBER,@PROD_WGPG,@PROD_SUBCATEGORY,@PROD_DES,@Branch,@Barcode,@CostPrice,@SalePrice,@Quantity,0,@IUSER,@IDAT,@Vendor_ID)             

           
           
 Declare @OID2 bigint=0          
 select @OID2=MAX(OID) from T_STOCK where PROD_WGPG=@PROD_WGPG and PROD_SUBCATEGORY=@PROD_SUBCATEGORY and PROD_DES=@PROD_DES  and Branch=@Branch          
           
           
 IF(@OID1 <>0)          
  BEGIN          
             
    update StoreMasterStock set Quantity=(Quantity + @Quantity),IUSER=@IUSER,IDAT=@IDAT where OID=@OID1          
    insert into Vendor_Incoming(PROD_WGPG,PROD_SUBCATEGORY,PROD_DES,Vendor_ID,Branch,CostPrice,Quantity,TOTAL,IUSER,IDAT,StockID) values(@PROD_WGPG,@PROD_SUBCATEGORY,@PROD_DES,@Vendor_ID,@Branch,@CostPrice,@Quantity,@CostPrice * @Quantity,@IUSER,@IDAT,@OID2)             
             
   IF (@OID IS NULL OR @OID = '')          
    BEGIN          
     insert into Purchase_Report(PROD_WGPG,PROD_SUBCATEGORY,PROD_DES,Vendor_ID,Branch,CostPrice,Quantity,TOTAL,Particular,IUSER,IDAT,StockID) values(@PROD_WGPG,@PROD_SUBCATEGORY,@PROD_DES,@Vendor_ID,@Branch,@CostPrice,@Quantity,@CostPrice * @Quantity,'Purchase',@IUSER,@IDAT,@OID2)             
    END          
   ELSE          
    BEGIN          
     insert into Purchase_Report(PROD_WGPG,PROD_SUBCATEGORY,PROD_DES,Vendor_ID,Branch,CostPrice,Quantity,TOTAL,Particular,IUSER,IDAT,StockID) values(@PROD_WGPG,@PROD_SUBCATEGORY,@PROD_DES,@Vendor_ID,@Branch,@CostPrice,@Quantity,@CostPrice * @Quantity,'Purchase Edit',@IUSER,@EDAT,@OID2)             
    END          
  END          
 ELSE          
  BEGIN      
    insert into StoreMasterStock(PROD_WGPG,PROD_SUBCATEGORY,PROD_DES,Branch,Barcode,CostPrice,SalePrice,Quantity,SaleQuantity,IUSER,IDAT) values(@PROD_WGPG,@PROD_SUBCATEGORY,@PROD_DES,@Branch,@Barcode,@CostPrice,@SalePrice,@Quantity,0,@IUSER,@IDAT)       
   
    insert into Vendor_Incoming(PROD_WGPG,PROD_SUBCATEGORY,PROD_DES,Vendor_ID,Branch,CostPrice,Quantity,TOTAL,IUSER,IDAT,StockID) values(@PROD_WGPG,@PROD_SUBCATEGORY,@PROD_DES,@Vendor_ID,@Branch,@CostPrice,@Quantity,@CostPrice * @Quantity,@IUSER,@IDAT,@OID2)
                 
    insert into Purchase_Report(PROD_WGPG,PROD_SUBCATEGORY,PROD_DES,Vendor_ID,Branch,CostPrice,Quantity,TOTAL,Particular,IUSER,IDAT,StockID) values(@PROD_WGPG,@PROD_SUBCATEGORY,@PROD_DES,@Vendor_ID,@Branch,@CostPrice,@Quantity,@CostPrice * @Quantity,'Purchase',@IUSER,@IDAT,@OID2)             
             
  END          
           
 IF (@Barcode IS NULL OR @Barcode = '')          
  BEGIN          
  insert into StockPosting(BranchOID,DescriptionOID,Barcode,InwardQty,OutwardQty,Particulars,Remarks,IUSER,IDAT) values(@Branch,@PROD_DES,'',@Quantity,'0','Stock In','',@IUSER,@IDAT)             
  END          
           
  insert into Acc_Purchase(Po_Number,PROD_WGPG,PROD_SUBCATEGORY,PROD_DES,Branch,CostPrice,PurchaseQuantity,IDAT,IDATTIME,IUSER,StockID)           
  values(@PONUMBER,@PROD_WGPG,@PROD_SUBCATEGORY,@PROD_DES,@Branch,@CostPrice,@Quantity,@IDAT,@PurchaseDatetime,@IUSER,@OID2)             
           
           
 Declare @AccStockOID bigint=0          
 IF(ISNULL(RTRIM(@Barcode),'')<>'')          
 Begin          
 --=====================================================          
 select @AccStockOID=MAX(ACC_STOCKID) from Acc_Stock           
 where PROD_WGPG=@PROD_WGPG and PROD_SUBCATEGORY=@PROD_SUBCATEGORY and PROD_DES=@PROD_DES  and Branch=@Branch and CostPrice=@CostPrice and Quantity>=0 and ISNULL(Flag,'')='' -- >=(Newly Added By Yeasin)         
           
 IF(@AccStockOID <>0)          
  BEGIN          
   update Acc_Stock set  Quantity=(Quantity + @Quantity) ,PQty=(PQty + @Quantity)           
   where ACC_STOCKID=@AccStockOID          
  END          
 ELSE          
  BEGIN          
   insert into Acc_Stock          
   (PROD_WGPG,PROD_SUBCATEGORY,PROD_DES,Branch,CostPrice,Quantity,IDAT,IDATTIME,IUSER,Flag          
   ,VendorID,PQty, PRQty, SQty, SRQty, MQty)           
   values          
   (@PROD_WGPG,@PROD_SUBCATEGORY,@PROD_DES,@Branch,@CostPrice,@Quantity,@IDAT,@PurchaseDatetime,@IUSER,''          
   ,'',@Quantity,'0','0','0','0')              
  END          
 END          
          
 --===========================================================          
 IF(ISNULL(RTRIM(@Barcode),'')='')          
 Begin          
 --=====================================================          
 --Declare @AccStockOID bigint=0          
 --select @AccStockOID=MAX(ACC_STOCKID) from Acc_Stock           
 --where PROD_WGPG=@PROD_WGPG and PROD_SUBCATEGORY=@PROD_SUBCATEGORY and PROD_DES=@PROD_DES  and Branch=@Branch and CostPrice=@CostPrice and Quantity>0           
           
 --IF(@AccStockOID <>0)          
 -- BEGIN          
 --  update Acc_Stock set Quantity=(Quantity + @Quantity) where ACC_STOCKID=@AccStockOID          
 -- END          
 --ELSE          
 -- BEGIN
	-- (Newly Added) By Yeasin
	 Declare @AccStockQty int=0
	 select @AccStockOID=MAX(ACC_STOCKID) from Acc_Stock           
	 where PROD_WGPG=@PROD_WGPG and PROD_SUBCATEGORY=@PROD_SUBCATEGORY and PROD_DES=@PROD_DES  
	 and Branch=@Branch and CostPrice=@CostPrice and VendorID=@Vendor_ID   
         
	if(@AccStockOID<>0)
	Begin 
		Set @AccStockQty =(Select Quantity From Acc_Stock Where ACC_STOCKID=@AccStockOID)
		if(@AccStockQty <0)
		Begin
			Set @AccStockQty=0
		End
		update Acc_Stock set  Quantity=(@AccStockQty + @Quantity) ,PQty=(PQty + @Quantity)           
		Where ACC_STOCKID=@AccStockOID          
	End 
	Else
	Begin         
	  insert into Acc_Stock          
	  (PROD_WGPG,PROD_SUBCATEGORY,PROD_DES,Branch,CostPrice,Quantity,IDAT,IDATTIME,IUSER,Flag,VendorID,PQty, PRQty, SQty, SRQty, MQty)           
	  values          
	  (@PROD_WGPG,@PROD_SUBCATEGORY,@PROD_DES,@Branch,@CostPrice,@Quantity,@IDAT,@PurchaseDatetime,@IUSER,'Quantity',@Vendor_ID,@Quantity,'0','0','0','0') 
    End  
  END          
   
           
  insert into Acc_StockDetail(Po_Number,PROD_WGPG,PROD_SUBCATEGORY,PROD_DES,Branch,Quantity,CostPrice,Total,Flag,Remarks,IDAT,IUSER,IDATTIME)           
  values(@PONUMBER,@PROD_WGPG,@PROD_SUBCATEGORY,@PROD_DES,@Branch,@Quantity,@CostPrice,@CostPrice*@Quantity,'Purchase','Purchase',@IDAT,@IUSER,@PurchaseDatetime)             
           
   
  insert into Acc_Journal(AccountID,Branch,Particular,Remarks,Debit,Credit,RefferenceNumber,IDAT,IDATTIME,Narration)           
  values(@PROD_DES,@Branch,'Purchase','Product',@CostPrice*@Quantity,0,@PONUMBER,@IDAT,@PurchaseDatetime,'Purchase on credit')             
  
  insert into Acc_Journal(AccountID,Branch,Particular,Remarks,Debit,Credit,RefferenceNumber,IDAT,IDATTIME,Narration) values(@Vendor_ID,@Branch,'A/P','Supplier',0,@CostPrice*@Quantity,@PONUMBER,@IDAT,@PurchaseDatetime,'Purchase on credit')         
  
    
 --IF(@CashTrans='1') 
 -- BEGIN          
 --  BEGIN          
 --   insert into Acc_Journal(AccountID,Branch,Particular,Remarks,Debit,Credit,RefferenceNumber,IDAT,IDATTIME,Narration)           
 --   values(@Vendor_ID,@Branch,'A/P','Supplier',@CostPrice*@Quantity,0,@PONUMBER,@IDAT,@PurchaseDatetime,'Payment to supplier on credit purchase')             
 --  END          
 --  BEGIN          
 --   insert into Acc_Journal(AccountID,Branch,Particular,Remarks,Debit,Credit,RefferenceNumber,IDAT,IDATTIME,Narration)           
 --   values('1',@Branch,'Cash','Cash',0,@CostPrice*@Quantity,@PONUMBER,@IDAT,@PurchaseDatetime,'Payment to supplier on credit purchase')             
 --  END   -- END     
 
 COMMIT TRANSACTION [Tran1];  
 END TRY  
   
 BEGIN CATCH  
 ROLLBACK TRANSACTION [Tran1];  
 END CATCH        
END 

GO


