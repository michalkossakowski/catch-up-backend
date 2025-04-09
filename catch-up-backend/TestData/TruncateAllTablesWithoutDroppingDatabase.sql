DECLARE @sql NVARCHAR(MAX) = '';

-- Wy��czanie i usuni�cie kluczy obcych
SELECT @sql += 'ALTER TABLE ' + QUOTENAME(OBJECT_NAME(fk.parent_object_id)) 
               + ' DROP CONSTRAINT ' + QUOTENAME(fk.name) + ';' + CHAR(13)
FROM sys.foreign_keys fk;

PRINT @sql;  -- Sprawdzenie SQL przed wykonaniem
EXEC sp_executesql @sql;

-- Truncate wszystkich tabel
SET @sql = '';
SELECT @sql += 'TRUNCATE TABLE ' + QUOTENAME(TABLE_SCHEMA) + '.' + QUOTENAME(TABLE_NAME) + ';' + CHAR(13)
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE';

PRINT @sql;  -- Sprawdzenie SQL przed wykonaniem
EXEC sp_executesql @sql;

-- Przywr�cenie usuni�tych kluczy obcych
SET @sql = '';
SELECT @sql += 'ALTER TABLE ' + QUOTENAME(OBJECT_NAME(fk.parent_object_id)) 
               + ' ADD CONSTRAINT ' + QUOTENAME(fk.name) 
               + ' FOREIGN KEY (' + STUFF((SELECT ',' + QUOTENAME(c.name) 
                                           FROM sys.foreign_key_columns fkc
                                           JOIN sys.columns c ON fkc.parent_object_id = c.object_id AND fkc.parent_column_id = c.column_id
                                           WHERE fkc.constraint_object_id = fk.object_id
                                           FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '') 
               + ') REFERENCES ' + QUOTENAME(OBJECT_NAME(fk.referenced_object_id)) 
               + ' (' + STUFF((SELECT ',' + QUOTENAME(c.name) 
                               FROM sys.foreign_key_columns fkc
                               JOIN sys.columns c ON fkc.referenced_object_id = c.object_id AND fkc.referenced_column_id = c.column_id
                               WHERE fkc.constraint_object_id = fk.object_id
                               FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '') 
               + ');' + CHAR(13)
FROM sys.foreign_keys fk;
	
PRINT @sql;  -- Sprawdzenie SQL przed wykonaniem
EXEC sp_executesql @sql;
