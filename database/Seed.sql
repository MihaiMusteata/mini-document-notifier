USE MiniDocumentNotifierDb;
GO

INSERT INTO Institutions (Code, Name)
VALUES 
    ('MAIB', 'Moldova Agroindbank'),
    ('MICB', 'Moldindconbank'),
    ('VICB', 'Victoriabank'),
    ('EXIM', 'Eximbank Moldova'),
    ('OTP', 'OTP Bank Moldova');
GO

INSERT INTO ViewConfigurations (InstitutionId, VisibleColumns, ActiveCategories, LastUpdatedDate)
VALUES
    (1, N'["Name","Type","UploadDate","Status"]', N'[0,1,2,3]', GETDATE()),
    (2, N'["Name","Type","UploadDate"]', N'[0,2]', GETDATE()),
    (3, N'["Name","UploadDate","Status"]', N'[0,1,3]', GETDATE()),
    (4, N'["Name","Type","Status"]', N'[1,2]', GETDATE()),
    (5, N'["Name","Type","UploadDate","Status"]', N'[0,1,2,3]', GETDATE());
GO