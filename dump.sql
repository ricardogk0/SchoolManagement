-- Criar o banco de dados 
IF DB_ID(N'SchoolManagementDB') IS NULL
BEGIN
    CREATE DATABASE [SchoolManagementDB];
END;
GO

USE [SchoolManagementDB];
GO

-- Criar tabela students 
IF OBJECT_ID(N'dbo.students', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[students] (
        [Id]            uniqueidentifier   NOT NULL,
        [Name]          nvarchar(100)      NOT NULL,
        [BirthDate]     date               NOT NULL,
        [DocumentNumber] nvarchar(20)      NOT NULL,
        [Email]         nvarchar(100)      NOT NULL,
        [Password]      nvarchar(255)      NOT NULL,
        [CreatedAt]     datetime2          NOT NULL,
        [CreatedBy]     nvarchar(max)      NOT NULL,
        [UpdatedAt]     datetime2          NULL,
        [UpdatedBy]     nvarchar(max)      NULL,
        [IsDeleted]     bit                NOT NULL,
        CONSTRAINT [PK_students] PRIMARY KEY ([Id])
    );
    

    CREATE UNIQUE INDEX [UX_students_DocumentNumber] ON [dbo].[students]([DocumentNumber]);
    CREATE UNIQUE INDEX [UX_students_Email] ON [dbo].[students]([Email]);

END;
GO

-- Criar tabela classes
IF OBJECT_ID(N'dbo.classes', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[classes] (
        [Id]            uniqueidentifier   NOT NULL,
        [ClassName]     nvarchar(100)      NOT NULL,
        [Description]   nvarchar(255)      NOT NULL,
        [CreatedAt]     datetime2          NOT NULL,
        [CreatedBy]     nvarchar(max)      NOT NULL,
        [UpdatedAt]     datetime2          NULL,
        [UpdatedBy]     nvarchar(max)      NULL,
        [IsDeleted]     bit                NOT NULL,
        CONSTRAINT [PK_classes] PRIMARY KEY ([Id])
    );
END;
GO

-- Criar tabela registrations 
IF OBJECT_ID(N'dbo.registrations', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[registrations] (
        [Id]            uniqueidentifier   NOT NULL,
        [StudentId]     uniqueidentifier   NOT NULL,
        [ClassId]       uniqueidentifier   NOT NULL,
        [CreatedAt]     datetime2          NOT NULL,
        [CreatedBy]     nvarchar(max)      NOT NULL,
        [UpdatedAt]     datetime2          NULL,
        [UpdatedBy]     nvarchar(max)      NULL,
        [IsDeleted]     bit                NOT NULL,
        CONSTRAINT [PK_registrations] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_registrations_students_StudentId]
            FOREIGN KEY ([StudentId]) REFERENCES [dbo].[students]([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_registrations_classes_ClassId]
            FOREIGN KEY ([ClassId]) REFERENCES [dbo].[classes]([Id]) ON DELETE NO ACTION
    );

    CREATE INDEX [IX_registrations_StudentId] ON [dbo].[registrations]([StudentId]);
    CREATE INDEX [IX_registrations_ClassId] ON [dbo].[registrations]([ClassId]);


    CREATE UNIQUE INDEX [UX_registrations_Student_Class] ON [dbo].[registrations]([StudentId], [ClassId]);
END;
GO
