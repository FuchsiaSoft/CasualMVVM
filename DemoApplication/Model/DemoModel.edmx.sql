
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 11/02/2016 17:26:49
-- Generated from EDMX file: C:\Users\Chris\Documents\GitHub\CasualMVVM\DemoApplication\Model\DemoModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [DemoAppDatabase];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_CaseTypeCase]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Cases] DROP CONSTRAINT [FK_CaseTypeCase];
GO
IF OBJECT_ID(N'[dbo].[FK_PersonCase]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Cases] DROP CONSTRAINT [FK_PersonCase];
GO
IF OBJECT_ID(N'[dbo].[FK_CaseTask]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Tasks] DROP CONSTRAINT [FK_CaseTask];
GO
IF OBJECT_ID(N'[dbo].[FK_CaseTelephoneContact]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TelephoneContacts] DROP CONSTRAINT [FK_CaseTelephoneContact];
GO
IF OBJECT_ID(N'[dbo].[FK_PersonTelephoneContact]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TelephoneContacts] DROP CONSTRAINT [FK_PersonTelephoneContact];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Cases]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Cases];
GO
IF OBJECT_ID(N'[dbo].[CaseTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CaseTypes];
GO
IF OBJECT_ID(N'[dbo].[People]', 'U') IS NOT NULL
    DROP TABLE [dbo].[People];
GO
IF OBJECT_ID(N'[dbo].[Tasks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Tasks];
GO
IF OBJECT_ID(N'[dbo].[TelephoneContacts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TelephoneContacts];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Cases'
CREATE TABLE [dbo].[Cases] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [OpenedDate] datetime  NOT NULL,
    [Comments] nvarchar(max)  NULL,
    [CaseType_Id] int  NOT NULL,
    [Person_Id] int  NOT NULL
);
GO

-- Creating table 'CaseTypes'
CREATE TABLE [dbo].[CaseTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'People'
CREATE TABLE [dbo].[People] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FirstName] nvarchar(max)  NOT NULL,
    [LastName] nvarchar(max)  NOT NULL,
    [DOB] datetime  NOT NULL
);
GO

-- Creating table 'Tasks'
CREATE TABLE [dbo].[Tasks] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(max)  NOT NULL,
    [Comments] nvarchar(max)  NULL,
    [DueDate] datetime  NOT NULL,
    [Complete] bit  NOT NULL,
    [Case_Id] int  NOT NULL
);
GO

-- Creating table 'TelephoneContacts'
CREATE TABLE [dbo].[TelephoneContacts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Date] datetime  NOT NULL,
    [Comments] nvarchar(max)  NULL,
    [Case_Id] int  NOT NULL,
    [Person_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Cases'
ALTER TABLE [dbo].[Cases]
ADD CONSTRAINT [PK_Cases]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CaseTypes'
ALTER TABLE [dbo].[CaseTypes]
ADD CONSTRAINT [PK_CaseTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'People'
ALTER TABLE [dbo].[People]
ADD CONSTRAINT [PK_People]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Tasks'
ALTER TABLE [dbo].[Tasks]
ADD CONSTRAINT [PK_Tasks]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TelephoneContacts'
ALTER TABLE [dbo].[TelephoneContacts]
ADD CONSTRAINT [PK_TelephoneContacts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [CaseType_Id] in table 'Cases'
ALTER TABLE [dbo].[Cases]
ADD CONSTRAINT [FK_CaseTypeCase]
    FOREIGN KEY ([CaseType_Id])
    REFERENCES [dbo].[CaseTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CaseTypeCase'
CREATE INDEX [IX_FK_CaseTypeCase]
ON [dbo].[Cases]
    ([CaseType_Id]);
GO

-- Creating foreign key on [Person_Id] in table 'Cases'
ALTER TABLE [dbo].[Cases]
ADD CONSTRAINT [FK_PersonCase]
    FOREIGN KEY ([Person_Id])
    REFERENCES [dbo].[People]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PersonCase'
CREATE INDEX [IX_FK_PersonCase]
ON [dbo].[Cases]
    ([Person_Id]);
GO

-- Creating foreign key on [Case_Id] in table 'Tasks'
ALTER TABLE [dbo].[Tasks]
ADD CONSTRAINT [FK_CaseTask]
    FOREIGN KEY ([Case_Id])
    REFERENCES [dbo].[Cases]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CaseTask'
CREATE INDEX [IX_FK_CaseTask]
ON [dbo].[Tasks]
    ([Case_Id]);
GO

-- Creating foreign key on [Case_Id] in table 'TelephoneContacts'
ALTER TABLE [dbo].[TelephoneContacts]
ADD CONSTRAINT [FK_CaseTelephoneContact]
    FOREIGN KEY ([Case_Id])
    REFERENCES [dbo].[Cases]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CaseTelephoneContact'
CREATE INDEX [IX_FK_CaseTelephoneContact]
ON [dbo].[TelephoneContacts]
    ([Case_Id]);
GO

-- Creating foreign key on [Person_Id] in table 'TelephoneContacts'
ALTER TABLE [dbo].[TelephoneContacts]
ADD CONSTRAINT [FK_PersonTelephoneContact]
    FOREIGN KEY ([Person_Id])
    REFERENCES [dbo].[People]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PersonTelephoneContact'
CREATE INDEX [IX_FK_PersonTelephoneContact]
ON [dbo].[TelephoneContacts]
    ([Person_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------