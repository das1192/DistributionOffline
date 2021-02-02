-- Author: Yeasin Ahmed          
-- Created Date: 12-May-2019          
-- Purpose: Add Or Edit a Product         
        
Alter Procedure SPP_PurchaseReturn_Quantity          
@PROD_WGPG Bigint,            
@PROD_SUBCATEGORY Bigint,            
@PROD_DES Bigint,            
@Quantity int,            
@Branch varchar(100),            
@CostPrice Bigint,            
@IUSER varchar(100),            
@IDAT Date,            
@Vendor_ID int,            
@prnumber varchar(100),            
@Total Bigint,            
@IDATTIME Datetime,            
@ACC_STOCKID Bigint,            
@Debit Bigint,            
@Credit Bigint,            
@StockID int            
As            
Begin      
 BEGIN TRANSACTION [Tran1];    
 BEGIN TRY             
 IF(@Vendor_ID >0)          
 Begin          
  --UpdateStoreMasterStock  
   --if()            
   if((Select COUNT(*) From StoreMasterStock Where PROD_WGPG=@PROD_WGPG AND PROD_SUBCATEGORY=@PROD_SUBCATEGORY AND PROD_DES=@PROD_DES And Branch=@Branch And Quantity > 0) >0)   
   Begin  
               
   update StoreMasterStock set Quantity=Quantity-@Quantity   
   where PROD_WGPG=@PROD_WGPG AND PROD_SUBCATEGORY=@PROD_SUBCATEGORY AND PROD_DES=@PROD_DES   
   And Branch=@Branch            
            
   --UpdatePurchaseReturn            
   INSERT INTO Purchase_Return(PROD_WGPG,PROD_SUBCATEGORY,PROD_DES,Branch,CostPrice,Quantity,IUSER,IDAT,Vendor_ID) VALUES(@PROD_WGPG,@PROD_SUBCATEGORY,@PROD_DES,@Branch,@CostPrice,@Quantity,@IUSER,@IDAT,@Vendor_ID)             
   INSERT INTO Acc_StockDetail(Po_Number,PROD_WGPG,PROD_SUBCATEGORY,PROD_DES,Branch,Quantity,CostPrice,Total,Flag,Remarks,IDAT,IUSER,IDATTIME) VALUES(@prnumber,@PROD_WGPG,@PROD_SUBCATEGORY,@PROD_DES,@Branch,@Quantity,@CostPrice,@Total,'Purchase Return','P
urchase Return',@IDAT,@IUSER,@IDATTIME)            
            
   update Acc_Stock set Quantity=Quantity-@Quantity , PRQty=PRQty+@Quantity    
   where ACC_STOCKID=@ACC_STOCKID And Branch=@Branch   
       
   INSERT INTO Acc_Journal(AccountID,Branch,Particular,Remarks,Debit,Credit,RefferenceNumber,IDAT,IDATTIME,Narration)   
   VALUES(@Vendor_ID,@Branch,'A/P','Supplier',@Debit,0,'',@IDAT,@IDATTIME,'Purchase Return To Supplier')            
            
   INSERT INTO Acc_Journal(AccountID,Branch,Particular,Remarks,Debit,Credit,RefferenceNumber,IDAT,IDATTIME,Narration) VALUES(@PROD_DES,@Branch,'Purchase','Product',0,@Credit,'',@IDAT,@IDATTIME,'Purchase Return To Supplier')            
              
   INSERT INTO Purchase_Report(PROD_WGPG,PROD_SUBCATEGORY,PROD_DES,Vendor_ID,Branch,CostPrice,Quantity,TOTAL,Particular,IUSER,IDAT,StockID) VALUES(@PROD_WGPG,@PROD_SUBCATEGORY,@PROD_DES,@Vendor_ID,@Branch,@CostPrice,@Quantity,@TOTAL,'Purchase Return',@IUSER,@IDAT,@StockID)  
     
      End  
              
 End      
 COMMIT TRANSACTION [Tran1];       
     
 END TRY    
 BEGIN CATCH    
 ROLLBACK TRANSACTION [Tran1];     
 END CATCH    
End 