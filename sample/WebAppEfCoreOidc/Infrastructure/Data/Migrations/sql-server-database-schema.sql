IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [Applications] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(200) NOT NULL,
    [Description] nvarchar(500) NULL,
    CONSTRAINT [PK_Applications] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Mappings] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(200) NOT NULL,
    CONSTRAINT [PK_Mappings] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Subjects] (
    [Id] int NOT NULL IDENTITY,
    [Sub] nvarchar(200) NOT NULL,
    [Name] nvarchar(200) NOT NULL,
    CONSTRAINT [PK_Subjects] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Permissions] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(200) NOT NULL,
    [Description] nvarchar(500) NULL,
    [ApplicationId] int NOT NULL,
    CONSTRAINT [PK_Permissions] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Permissions_Applications_ApplicationId] FOREIGN KEY ([ApplicationId]) REFERENCES [Applications] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [Roles] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(200) NOT NULL,
    [Description] nvarchar(500) NULL,
    [Enabled] bit NOT NULL DEFAULT CAST(1 AS bit),
    [ApplicationId] int NOT NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Roles_Applications_ApplicationId] FOREIGN KEY ([ApplicationId]) REFERENCES [Applications] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [Delegations] (
    [Id] int NOT NULL IDENTITY,
    [WhoId] int NOT NULL,
    [WhomId] int NOT NULL,
    [From] datetime2 NOT NULL,
    [To] datetime2 NOT NULL,
    [Selected] bit NOT NULL,
    [ApplicationEntityId] int NULL,
    CONSTRAINT [PK_Delegations] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Delegations_Applications_ApplicationEntityId] FOREIGN KEY ([ApplicationEntityId]) REFERENCES [Applications] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Delegations_Subjects_WhoId] FOREIGN KEY ([WhoId]) REFERENCES [Subjects] ([Id]),
    CONSTRAINT [FK_Delegations_Subjects_WhomId] FOREIGN KEY ([WhomId]) REFERENCES [Subjects] ([Id])
);

GO

CREATE TABLE [RolesMappings] (
    [RoleId] int NOT NULL,
    [MappingId] int NOT NULL,
    CONSTRAINT [PK_RolesMappings] PRIMARY KEY ([RoleId], [MappingId]),
    CONSTRAINT [FK_RolesMappings_Mappings_MappingId] FOREIGN KEY ([MappingId]) REFERENCES [Mappings] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_RolesMappings_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [RolesPermissions] (
    [RoleId] int NOT NULL,
    [PermissionId] int NOT NULL,
    CONSTRAINT [PK_RolesPermissions] PRIMARY KEY ([RoleId], [PermissionId]),
    CONSTRAINT [FK_RolesPermissions_Permissions_PermissionId] FOREIGN KEY ([PermissionId]) REFERENCES [Permissions] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_RolesPermissions_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [RolesSubjects] (
    [RoleId] int NOT NULL,
    [SubjectId] int NOT NULL,
    CONSTRAINT [PK_RolesSubjects] PRIMARY KEY ([RoleId], [SubjectId]),
    CONSTRAINT [FK_RolesSubjects_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_RolesSubjects_Subjects_SubjectId] FOREIGN KEY ([SubjectId]) REFERENCES [Subjects] ([Id]) ON DELETE NO ACTION
);

GO

CREATE UNIQUE INDEX [IX_Applications_Name] ON [Applications] ([Name]);

GO

CREATE INDEX [IX_Delegations_ApplicationEntityId] ON [Delegations] ([ApplicationEntityId]);

GO

CREATE INDEX [IX_Delegations_WhoId] ON [Delegations] ([WhoId]);

GO

CREATE INDEX [IX_Delegations_WhomId] ON [Delegations] ([WhomId]);

GO

CREATE UNIQUE INDEX [IX_Mappings_Name] ON [Mappings] ([Name]);

GO

CREATE INDEX [IX_Permissions_ApplicationId] ON [Permissions] ([ApplicationId]);

GO

CREATE INDEX [IX_Roles_ApplicationId] ON [Roles] ([ApplicationId]);

GO

CREATE UNIQUE INDEX [IX_Roles_Name] ON [Roles] ([Name]);

GO

CREATE INDEX [IX_RolesMappings_MappingId] ON [RolesMappings] ([MappingId]);

GO

CREATE INDEX [IX_RolesPermissions_PermissionId] ON [RolesPermissions] ([PermissionId]);

GO

CREATE INDEX [IX_RolesSubjects_SubjectId] ON [RolesSubjects] ([SubjectId]);

GO

CREATE UNIQUE INDEX [IX_Subjects_Sub] ON [Subjects] ([Sub]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200216081447_Initial', N'3.1.2');

GO