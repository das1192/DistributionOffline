-- Author  : Yeasin Ahmed          
-- Created Date : 12-May-2019          
-- Purpose  : To Update Product Price          
          
Alter Procedure [dbo].[spUpdateProductPrice]          
@StockId Bigint,          
@DesId Bigint,          
@TProdId Bigint,          
@SalePrice Bigint,          
@Flag Varchar(20)=null          
As          
Begin       
 BEGIN TRANSACTION [Tran1];    
  BEGIN TRY            
   If(@Flag is null OR @Flag='')          
   Begin          
    update StoreMasterStock set SalePrice=@SalePrice where OID=@StockId          
    update [Description] set MRP=@SalePrice where OID=@DesId          
   End          
   ELSE IF(@Flag!='' AND @Flag is not null) -- PROD_WGPG==111          
   Begin          
    update T_PROD set SalePrice=@SalePrice where OID=@TProdId          
   End    
   COMMIT TRANSACTION [Tran1];      
 END TRY    
 BEGIN CATCH    
  ROLLBACK TRANSACTION [Tran1];    
 END CATCH    
End  