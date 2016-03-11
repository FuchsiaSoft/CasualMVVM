
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 03/06/2016 12:13:19
-- Generated from EDMX file: C:\Users\Chris\Documents\GitHub\CasualMVVM\DemoApp\DemoModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [DemoApp];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------


-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'People'
CREATE TABLE [dbo].[People] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FirstName] nvarchar(max)  NOT NULL,
    [LastName] nvarchar(max)  NOT NULL,
    [Age] int  NOT NULL,
    [CheckedPapers] bit  NOT NULL,
    [Comments] nvarchar(max)  NULL,
    [HairColour_Id] int  NOT NULL
);
GO

-- Creating table 'HairColours'
CREATE TABLE [dbo].[HairColours] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Colour] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'People'
ALTER TABLE [dbo].[People]
ADD CONSTRAINT [PK_People]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'HairColours'
ALTER TABLE [dbo].[HairColours]
ADD CONSTRAINT [PK_HairColours]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [HairColour_Id] in table 'People'
ALTER TABLE [dbo].[People]
ADD CONSTRAINT [FK_HairColourPerson]
    FOREIGN KEY ([HairColour_Id])
    REFERENCES [dbo].[HairColours]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HairColourPerson'
CREATE INDEX [IX_FK_HairColourPerson]
ON [dbo].[People]
    ([HairColour_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------