Use Archive;
DECLARE @sql AS NVARCHAR(MAX) =''
DECLARE @role_id_from AS NVARCHAR(MAX) ='14'
DECLARE @role_id_to AS NVARCHAR(MAX) ='18'

SET @sql = 'INSERT INTO [dbo].[_role_access]([id_access],[id_role],[del]) SELECT [id_access],' + @role_id_to + ' as [id_role],[del] FROM [_role_access] WHERE id_role=' + @role_id_from + ' AND del=0' 
EXEC sp_executesql 	@sql
