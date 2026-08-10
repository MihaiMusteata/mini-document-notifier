CREATE PROCEDURE dbo.Institution_GetById
    @Id VARCHAR(5)
AS
BEGIN
    SET NOCOUNT ON;
SELECT Id, Code, Name FROM Institutions WHERE Id = @Id;
END
GO