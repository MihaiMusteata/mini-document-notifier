CREATE PROCEDURE dbo.Document_GetPaged @InstitutionId INT,
    @PageNumber INT,
    @PageSize INT,
    @AllowedTypes NVARCHAR(200) = NULL,
    @TypeFilter INT = NULL,
    @StatusFilter INT = NULL,
    @SortColumn VARCHAR(50) = 'UploadDate',
    @SortDirection BIT = 1,
    @TotalCount INT OUTPUT
AS
BEGIN
    SET
NOCOUNT ON;

    IF
@SortColumn NOT IN ('Id', 'Name', 'Type', 'Status', 'UploadDate')
        SET @SortColumn = 'UploadDate';

SELECT @TotalCount = COUNT(*)
FROM Documents
WHERE InstitutionId = @InstitutionId
  AND (@AllowedTypes IS NULL OR Type IN (SELECT CAST(value AS INT) FROM STRING_SPLIT(@AllowedTypes, ',')))
  AND (@TypeFilter IS NULL OR Type = @TypeFilter)
  AND (@StatusFilter IS NULL OR Status = @StatusFilter);

DECLARE
@sql NVARCHAR(MAX) = N'
    SELECT Id, InstitutionId, Name, Type, UploadDate, Status
    FROM Documents
    WHERE InstitutionId = @InstitutionId
      AND (@AllowedTypes IS NULL OR Type IN (SELECT CAST(value AS INT) FROM STRING_SPLIT(@AllowedTypes, '','')))
      AND (@TypeFilter IS NULL OR Type = @TypeFilter)
      AND (@StatusFilter IS NULL OR Status = @StatusFilter)
    ORDER BY ' + QUOTENAME(@SortColumn) + CASE WHEN @SortDirection = 1 THEN N' DESC' ELSE N' ASC'
END + N'
    OFFSET @PageNumber * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY;';

EXEC sp_executesql @sql,
        N'@InstitutionId INT, @AllowedTypes NVARCHAR(200), @TypeFilter INT, @StatusFilter INT, @PageNumber INT, @PageSize INT',
        @InstitutionId, @AllowedTypes, @TypeFilter, @StatusFilter, @PageNumber, @PageSize;
END
GO