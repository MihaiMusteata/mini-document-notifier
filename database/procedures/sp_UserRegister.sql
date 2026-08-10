CREATE PROCEDURE dbo.User_Register
    @Username VARCHAR(32),
    @PasswordHash VARCHAR(256),
    @PasswordSalt VARCHAR(256),
    @InstitutionId INT
AS
BEGIN
    SET NOCOUNT ON;
INSERT INTO Users (Username, PasswordHash, PasswordSalt, InstitutionId, IsEnabled)
VALUES (@Username, @PasswordHash, @PasswordSalt, @InstitutionId, 1);
END
GO