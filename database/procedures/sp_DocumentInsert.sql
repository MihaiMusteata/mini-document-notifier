CREATE PROCEDURE dbo.Document_Insert
    @InstitutionId INT,
    @Name VARCHAR(260),
    @Type INT,
    @UploadDate DATETIME2,
    @Status INT,
    @DocumentId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Documents (InstitutionId, Name, Type, UploadDate, Status)
    VALUES (@InstitutionId, @Name, @Type, @UploadDate, @Status);

    SET @DocumentId = SCOPE_IDENTITY();
END
GO