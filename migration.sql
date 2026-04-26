BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260423130304_UpdateOnApplicantForm'
)
BEGIN
    ALTER TABLE [Applicants] ADD [LeavingReason] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260423130304_UpdateOnApplicantForm'
)
BEGIN
    ALTER TABLE [Applicants] ADD [Relatives] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260423130304_UpdateOnApplicantForm'
)
BEGIN
    ALTER TABLE [Applicants] ADD [TotalExperience] int NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260423130304_UpdateOnApplicantForm'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260423130304_UpdateOnApplicantForm', N'8.0.21');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260426082921_removeSoftDelete'
)
BEGIN
    ALTER TABLE [Applications] DROP CONSTRAINT [FK_Applications_Applicants_ApplicantId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260426082921_removeSoftDelete'
)
BEGIN
    ALTER TABLE [Applications] DROP CONSTRAINT [FK_Applications_AspNetUsers_ReviewedBy];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260426082921_removeSoftDelete'
)
BEGIN
    ALTER TABLE [Applications] DROP CONSTRAINT [FK_Applications_Vacancies_VacancyId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260426082921_removeSoftDelete'
)
BEGIN
    ALTER TABLE [Interviews] DROP CONSTRAINT [FK_Interviews_Applications_ApplicationId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260426082921_removeSoftDelete'
)
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Vacancies]') AND [c].[name] = N'IsDeleted');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Vacancies] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [Vacancies] DROP COLUMN [IsDeleted];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260426082921_removeSoftDelete'
)
BEGIN
    DECLARE @var1 sysname;
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[UserProjects]') AND [c].[name] = N'IsDeleted');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [UserProjects] DROP CONSTRAINT [' + @var1 + '];');
    ALTER TABLE [UserProjects] DROP COLUMN [IsDeleted];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260426082921_removeSoftDelete'
)
BEGIN
    DECLARE @var2 sysname;
    SELECT @var2 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Titles]') AND [c].[name] = N'IsDeleted');
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Titles] DROP CONSTRAINT [' + @var2 + '];');
    ALTER TABLE [Titles] DROP COLUMN [IsDeleted];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260426082921_removeSoftDelete'
)
BEGIN
    DECLARE @var3 sysname;
    SELECT @var3 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RolePermissions]') AND [c].[name] = N'IsDeleted');
    IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [RolePermissions] DROP CONSTRAINT [' + @var3 + '];');
    ALTER TABLE [RolePermissions] DROP COLUMN [IsDeleted];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260426082921_removeSoftDelete'
)
BEGIN
    DECLARE @var4 sysname;
    SELECT @var4 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Reports]') AND [c].[name] = N'IsDeleted');
    IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [Reports] DROP CONSTRAINT [' + @var4 + '];');
    ALTER TABLE [Reports] DROP COLUMN [IsDeleted];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260426082921_removeSoftDelete'
)
BEGIN
    DECLARE @var5 sysname;
    SELECT @var5 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ReportParameters]') AND [c].[name] = N'IsDeleted');
    IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [ReportParameters] DROP CONSTRAINT [' + @var5 + '];');
    ALTER TABLE [ReportParameters] DROP COLUMN [IsDeleted];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260426082921_removeSoftDelete'
)
BEGIN
    DECLARE @var6 sysname;
    SELECT @var6 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RejectionReasons]') AND [c].[name] = N'IsDeleted');
    IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [RejectionReasons] DROP CONSTRAINT [' + @var6 + '];');
    ALTER TABLE [RejectionReasons] DROP COLUMN [IsDeleted];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260426082921_removeSoftDelete'
)
BEGIN
    DECLARE @var7 sysname;
    SELECT @var7 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ProjectVacancies]') AND [c].[name] = N'IsDeleted');
    IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [ProjectVacancies] DROP CONSTRAINT [' + @var7 + '];');
    ALTER TABLE [ProjectVacancies] DROP COLUMN [IsDeleted];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260426082921_removeSoftDelete'
)
BEGIN
    DECLARE @var8 sysname;
    SELECT @var8 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Projects]') AND [c].[name] = N'IsDeleted');
    IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [Projects] DROP CONSTRAINT [' + @var8 + '];');
    ALTER TABLE [Projects] DROP COLUMN [IsDeleted];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260426082921_removeSoftDelete'
)
BEGIN
    DECLARE @var9 sysname;
    SELECT @var9 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Permissions]') AND [c].[name] = N'IsDeleted');
    IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [Permissions] DROP CONSTRAINT [' + @var9 + '];');
    ALTER TABLE [Permissions] DROP COLUMN [IsDeleted];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260426082921_removeSoftDelete'
)
BEGIN
    DECLARE @var10 sysname;
    SELECT @var10 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Locations]') AND [c].[name] = N'IsDeleted');
    IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [Locations] DROP CONSTRAINT [' + @var10 + '];');
    ALTER TABLE [Locations] DROP COLUMN [IsDeleted];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260426082921_removeSoftDelete'
)
BEGIN
    DECLARE @var11 sysname;
    SELECT @var11 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Interviews]') AND [c].[name] = N'IsDeleted');
    IF @var11 IS NOT NULL EXEC(N'ALTER TABLE [Interviews] DROP CONSTRAINT [' + @var11 + '];');
    ALTER TABLE [Interviews] DROP COLUMN [IsDeleted];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260426082921_removeSoftDelete'
)
BEGIN
    DECLARE @var12 sysname;
    SELECT @var12 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[InterviewRejectionReason]') AND [c].[name] = N'IsDeleted');
    IF @var12 IS NOT NULL EXEC(N'ALTER TABLE [InterviewRejectionReason] DROP CONSTRAINT [' + @var12 + '];');
    ALTER TABLE [InterviewRejectionReason] DROP COLUMN [IsDeleted];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260426082921_removeSoftDelete'
)
BEGIN
    DECLARE @var13 sysname;
    SELECT @var13 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Interviewers]') AND [c].[name] = N'IsDeleted');
    IF @var13 IS NOT NULL EXEC(N'ALTER TABLE [Interviewers] DROP CONSTRAINT [' + @var13 + '];');
    ALTER TABLE [Interviewers] DROP COLUMN [IsDeleted];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260426082921_removeSoftDelete'
)
BEGIN
    DECLARE @var14 sysname;
    SELECT @var14 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[DepartmentTitles]') AND [c].[name] = N'IsDeleted');
    IF @var14 IS NOT NULL EXEC(N'ALTER TABLE [DepartmentTitles] DROP CONSTRAINT [' + @var14 + '];');
    ALTER TABLE [DepartmentTitles] DROP COLUMN [IsDeleted];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260426082921_removeSoftDelete'
)
BEGIN
    DECLARE @var15 sysname;
    SELECT @var15 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Departments]') AND [c].[name] = N'IsDeleted');
    IF @var15 IS NOT NULL EXEC(N'ALTER TABLE [Departments] DROP CONSTRAINT [' + @var15 + '];');
    ALTER TABLE [Departments] DROP COLUMN [IsDeleted];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260426082921_removeSoftDelete'
)
BEGIN
    DECLARE @var16 sysname;
    SELECT @var16 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Currencies]') AND [c].[name] = N'IsDeleted');
    IF @var16 IS NOT NULL EXEC(N'ALTER TABLE [Currencies] DROP CONSTRAINT [' + @var16 + '];');
    ALTER TABLE [Currencies] DROP COLUMN [IsDeleted];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260426082921_removeSoftDelete'
)
BEGIN
    DECLARE @var17 sysname;
    SELECT @var17 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Countries]') AND [c].[name] = N'IsDeleted');
    IF @var17 IS NOT NULL EXEC(N'ALTER TABLE [Countries] DROP CONSTRAINT [' + @var17 + '];');
    ALTER TABLE [Countries] DROP COLUMN [IsDeleted];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260426082921_removeSoftDelete'
)
BEGIN
    DECLARE @var18 sysname;
    SELECT @var18 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AuditLogs]') AND [c].[name] = N'IsDeleted');
    IF @var18 IS NOT NULL EXEC(N'ALTER TABLE [AuditLogs] DROP CONSTRAINT [' + @var18 + '];');
    ALTER TABLE [AuditLogs] DROP COLUMN [IsDeleted];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260426082921_removeSoftDelete'
)
BEGIN
    DECLARE @var19 sysname;
    SELECT @var19 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Applications]') AND [c].[name] = N'IsDeleted');
    IF @var19 IS NOT NULL EXEC(N'ALTER TABLE [Applications] DROP CONSTRAINT [' + @var19 + '];');
    ALTER TABLE [Applications] DROP COLUMN [IsDeleted];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260426082921_removeSoftDelete'
)
BEGIN
    DECLARE @var20 sysname;
    SELECT @var20 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ApplicationRejectionReason]') AND [c].[name] = N'IsDeleted');
    IF @var20 IS NOT NULL EXEC(N'ALTER TABLE [ApplicationRejectionReason] DROP CONSTRAINT [' + @var20 + '];');
    ALTER TABLE [ApplicationRejectionReason] DROP COLUMN [IsDeleted];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260426082921_removeSoftDelete'
)
BEGIN
    DECLARE @var21 sysname;
    SELECT @var21 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Applicants]') AND [c].[name] = N'IsDeleted');
    IF @var21 IS NOT NULL EXEC(N'ALTER TABLE [Applicants] DROP CONSTRAINT [' + @var21 + '];');
    ALTER TABLE [Applicants] DROP COLUMN [IsDeleted];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260426082921_removeSoftDelete'
)
BEGIN
    ALTER TABLE [Applications] ADD CONSTRAINT [FK_Applications_Applicants_ApplicantId] FOREIGN KEY ([ApplicantId]) REFERENCES [Applicants] ([Id]) ON DELETE NO ACTION;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260426082921_removeSoftDelete'
)
BEGIN
    ALTER TABLE [Applications] ADD CONSTRAINT [FK_Applications_AspNetUsers_ReviewedBy] FOREIGN KEY ([ReviewedBy]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260426082921_removeSoftDelete'
)
BEGIN
    ALTER TABLE [Applications] ADD CONSTRAINT [FK_Applications_Vacancies_VacancyId] FOREIGN KEY ([VacancyId]) REFERENCES [Vacancies] ([Id]) ON DELETE NO ACTION;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260426082921_removeSoftDelete'
)
BEGIN
    ALTER TABLE [Interviews] ADD CONSTRAINT [FK_Interviews_Applications_ApplicationId] FOREIGN KEY ([ApplicationId]) REFERENCES [Applications] ([Id]) ON DELETE NO ACTION;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260426082921_removeSoftDelete'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260426082921_removeSoftDelete', N'8.0.21');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260426111737_addAssignedToColumn'
)
BEGIN
    ALTER TABLE [Applications] ADD [AssignedAt] datetime2 NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260426111737_addAssignedToColumn'
)
BEGIN
    ALTER TABLE [Applications] ADD [AssignedTo] int NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260426111737_addAssignedToColumn'
)
BEGIN
    ALTER TABLE [Applications] ADD [AssignedToUserId] int NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260426111737_addAssignedToColumn'
)
BEGIN
    CREATE INDEX [IX_Applications_AssignedToUserId] ON [Applications] ([AssignedToUserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260426111737_addAssignedToColumn'
)
BEGIN
    ALTER TABLE [Applications] ADD CONSTRAINT [FK_Applications_AspNetUsers_AssignedToUserId] FOREIGN KEY ([AssignedToUserId]) REFERENCES [AspNetUsers] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260426111737_addAssignedToColumn'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260426111737_addAssignedToColumn', N'8.0.21');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260426121550_addAssignedToColumn2'
)
BEGIN
    ALTER TABLE [Applications] DROP CONSTRAINT [FK_Applications_AspNetUsers_AssignedToUserId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260426121550_addAssignedToColumn2'
)
BEGIN
    DROP INDEX [IX_Applications_AssignedToUserId] ON [Applications];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260426121550_addAssignedToColumn2'
)
BEGIN
    DECLARE @var22 sysname;
    SELECT @var22 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Applications]') AND [c].[name] = N'AssignedToUserId');
    IF @var22 IS NOT NULL EXEC(N'ALTER TABLE [Applications] DROP CONSTRAINT [' + @var22 + '];');
    ALTER TABLE [Applications] DROP COLUMN [AssignedToUserId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260426121550_addAssignedToColumn2'
)
BEGIN
    CREATE INDEX [IX_Applications_AssignedTo] ON [Applications] ([AssignedTo]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260426121550_addAssignedToColumn2'
)
BEGIN
    ALTER TABLE [Applications] ADD CONSTRAINT [FK_Applications_AspNetUsers_AssignedTo] FOREIGN KEY ([AssignedTo]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260426121550_addAssignedToColumn2'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260426121550_addAssignedToColumn2', N'8.0.21');
END;
GO

COMMIT;
GO

