CREATE PROCEDURE dbo.Institution_GetAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Code, Name FROM Institutions;
END
GO