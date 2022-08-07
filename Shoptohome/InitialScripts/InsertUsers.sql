INSERT INTO [dbo].[Users]
           ([UserName]
           ,[Password]
           ,[RoleID]
           ,[StatusID]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
     VALUES
           ('remin'
           ,'remin@123'
           ,1
           ,1
           ,1
           ,GETDATE()
           ,1
           ,GETDATE())