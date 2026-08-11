CREATE PROCEDURE dbo.ViewConfiguration_GetAllWithInstitutions
    AS
BEGIN
    SET NOCOUNT ON;

SELECT vc.Id,
       vc.InstitutionId,
       vc.VisibleColumns,
       vc.ActiveCategories,
       vc.LastUpdatedDate,

       i.Id   AS Institution_Id,
       i.Code AS Institution_Code,
       i.Name AS Institution_Name

FROM ViewConfigurations vc
INNER JOIN Institutions i ON i.Id = vc.InstitutionId;

END
GO