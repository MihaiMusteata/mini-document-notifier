CREATE PROCEDURE dbo.User_GetByUsernameAndInstitution
    @Username VARCHAR(32),
    @InstitutionId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Username, PasswordHash, PasswordSalt, InstitutionId, IsEnabled
    FROM Users
    WHERE Username = @Username AND InstitutionId = @InstitutionId;
END
GO