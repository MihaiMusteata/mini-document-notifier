CREATE PROCEDURE dbo.Document_GetByInstitution
    @InstitutionId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, InstitutionId, Name, Type, UploadDate, Status
    FROM Documents
    WHERE InstitutionId = @InstitutionId;
END
GO