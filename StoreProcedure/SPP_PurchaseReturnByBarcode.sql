
-- Author: Yeasin Ahmed          
-- Created Date: 12-May-2019          
-- Purpose: Update and add data when product is retured by barcode          
          
Alter PROCEDURE [dbo].[SPP_PurchaseReturnByBarcode]            
@StoreMasterStockOID bigint,          
@T_STOCKOID bigint,          
@T_PRODOID bigint,          
@PROD_DES bigint,           
@Branch nvarchar(100),          
@Barcode varchar(100),          
@PROD_WGPG bigint,          
@PROD_SUBCATEGORY bigint,          
@Vendor_ID int,          
@CostPrice bigint,          
@IUSER as varchar(50),          
@IDAT datetime            
AS            
            
BEGIN            
 BEGIN TRANSACTION [Tran1];     
     
 BEGIN TRY      
   
 if((Select COUNT(*) From [T_PROD] Where Barcode=@Barcode And Branch=@Branch And SaleStatus=0)> 0)  
 Begin  
 Update [T_STOCK] Set SaleStatus = 2              
 Where OID=@T_STOCKOID and PROD_DES=@PROD_DES and Branch=@Branch and Barcode=@Barcode ;            
             
 Update [T_PROD] Set SaleStatus = 2              
 Where OID=@T_PRODOID and PROD_DES=@PROD_DES and Branch=@Branch and Barcode=@Barcode ;            
            
 Update [StoreMasterStock] Set Quantity = (Quantity-1)              
 Where OID=@StoreMasterStockOID and PROD_DES=@PROD_DES and Branch=@Branch;            
             
  update Acc_Stock set Quantity=(Quantity-1), PRQty=(PRQty+1)            
  where ACC_STOCKID=            
  (            
   select top 1 ACC_STOCKID from Acc_Stock             
   where ISNULL(Flag,'')='' and Quantity >=0             
   and Branch=@Branch and CostPrice=@CostPrice            
   and PROD_WGPG=@PROD_WGPG and PROD_SUBCATEGORY=@PROD_SUBCATEGORY and PROD_DES= @PROD_DES  
   Order By Quantity DESC               
  )  
             
  declare @reff nvarchar(50);            
  select @reff=CONVERT(nvarchar(20) ,GETDATE())            
            
  INSERT INTO  Acc_StockDetail            
  (Po_Number, PROD_WGPG, PROD_SUBCATEGORY, PROD_DES, Branch            
  , Quantity, CostPrice, SalePrice, Total, Flag            
  , Remarks, IDAT, IUSER, IDATTIME)              
  VALUES(@reff,@PROD_WGPG,@PROD_SUBCATEGORY,@PROD_DES,@Branch            
  ,1,@CostPrice,0,@CostPrice,'Purchase Return'            
  ,'Purchase Return',@IDAT,@IUSER,@IDAT);            
 --------------------------------------------------------------------            
            
  INSERT INTO             
  Purchase_Return(PROD_WGPG,PROD_SUBCATEGORY,PROD_DES,Branch,CostPrice,Quantity,IUSER,IDAT,Vendor_ID)             
  VALUES(@PROD_WGPG,@PROD_SUBCATEGORY,@PROD_DES,@Branch,@CostPrice,1,@IUSER,@IDAT,@Vendor_ID);            
             
  INSERT INTO             
  Purchase_Report            
  (PROD_WGPG,PROD_SUBCATEGORY,PROD_DES,Vendor_ID,Branch,CostPrice,Quantity,TOTAL,Particular,IUSER,IDAT,StockID)             
  VALUES            
  (@PROD_WGPG,@PROD_SUBCATEGORY,@PROD_DES,@Vendor_ID,@Branch,@CostPrice,1,@CostPrice,'Purchase Return',@IUSER,@IDAT,@T_STOCKOID);            
            
  INSERT INTO Acc_Journal(AccountID,Branch,Particular,Remarks,Debit,Credit,RefferenceNumber,IDAT,IDATTIME,Narration)           
 VALUES(@Vendor_ID,@Branch,'A/P', 'Supplier',@CostPrice,0,'Ref-'+Convert(varchar(50),@Barcode),@IDAT,GETDATE(),'Purchase Return(barcode) From Supplier')          
            
   INSERT INTO Acc_Journal(AccountID,Branch,Particular,Remarks,Debit,Credit,RefferenceNumber,IDAT,IDATTIME,Narration)           
 VALUES(@PROD_DES,@Branch,'Purchase','Product',0,@CostPrice,'Ref-'+CONVERT(Varchar(50),@Barcode),@IDAT,GETDATE(),'Purchase Return(barcode) From Supplier')  
       
 End  
   
 COMMIT TRANSACTION [Tran1];    
     
 END TRY    
     
 BEGIN CATCH    
 ROLLBACK TRANSACTION [Tran1];    
 END CATCH      
END   
  