if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gisBg]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
 BEGIN
CREATE TABLE [dbo].[gisBg] (
	[Id] [int] NOT NULL ,
	[Attr] [int] NOT NULL ,
	[FilePath] [varchar] (500) COLLATE Cyrillic_General_CI_AS NOT NULL ,
	[SMin] [int] NOT NULL ,
	[SMax] [int] NOT NULL ,
	[Opacity] [int] NOT NULL ,
	[Code] [varbinary] (100) NOT NULL 
) ON [PRIMARY]
END

GO

if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gisColors]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
 BEGIN
CREATE TABLE [dbo].[gisColors] (
	[Id] [int] NOT NULL ,
	[Name] [varchar] (50) COLLATE Cyrillic_General_CI_AS NOT NULL ,
	[Val] [int] NOT NULL 
) ON [PRIMARY]
END

GO

if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gisLayers]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
 BEGIN
CREATE TABLE [dbo].[gisLayers] (
	[Id] [int]  NOT NULL ,
	[Attr] [int] NOT NULL ,
	[Name] [varchar] (50) COLLATE Cyrillic_General_CI_AS NOT NULL ,
	[Code] [varbinary] (8000) NOT NULL 
) ON [PRIMARY]
END

GO

if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gisLib]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
 BEGIN
CREATE TABLE [dbo].[gisLib] (
	[Id] [int] NOT NULL ,
	[Attr] [int] NOT NULL ,
	[Name] [varchar] (50) COLLATE Cyrillic_General_CI_AS NOT NULL ,
	[Style] [varchar] (100) COLLATE Cyrillic_General_CI_AS NOT NULL ,
	[DefaultStyle] [varchar] (100) COLLATE Cyrillic_General_CI_AS NOT NULL ,
	[SMin] [int] NOT NULL ,
	[SMax] [int] NOT NULL ,
	[Code] [binary] (16) NOT NULL ,
	[IndexerCode] [varbinary] (100) NOT NULL ,
	[Scales] [varbinary] (1000) NOT NULL 
) ON [PRIMARY]
END

GO

if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gisObjects]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
 BEGIN
CREATE TABLE [dbo].[gisObjects] (
	[Id] [int]  NOT NULL ,
	[TypeId] [int] NOT NULL ,
	[RangeId] [int] NOT NULL ,
	[Attr] [int] NOT NULL ,
	[Name] [varchar] (50) COLLATE Cyrillic_General_CI_AS NOT NULL ,
	[Caption] [varchar] (50) COLLATE Cyrillic_General_CI_AS NOT NULL ,
	[Style] [varchar] (100) COLLATE Cyrillic_General_CI_AS NOT NULL ,
	[TextAttr] [varchar] (100) COLLATE Cyrillic_General_CI_AS NOT NULL ,
	[Code] [varbinary] (8000) NOT NULL 
) ON [PRIMARY]
END

GO

if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gisRanges]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
 BEGIN
CREATE TABLE [dbo].[gisRanges] (
	[Id] [int]  NOT NULL ,
	[TypeId] [int] NOT NULL ,
	[Code] [binary] (16) NOT NULL 
) ON [PRIMARY]
END

GO

if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gisTypes]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
 BEGIN
CREATE TABLE [dbo].[gisTypes] (
	[Id] [int]  NOT NULL ,
	[ParentId] [int] NOT NULL ,
	[Priority] [int] NOT NULL ,
	[Attr] [int] NOT NULL ,
	[Name] [varchar] (50) COLLATE Cyrillic_General_CI_AS NOT NULL ,
	[Style] [varchar] (100) COLLATE Cyrillic_General_CI_AS NOT NULL ,
	[GeomType] [int] NOT NULL ,
	[SMin] [int] NOT NULL ,
	[SMax] [int] NOT NULL 
) ON [PRIMARY]
END

GO

if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gisViews]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
 BEGIN
CREATE TABLE [dbo].[gisViews] (
	[Id] [int] NOT NULL ,
	[Attr] [int] NOT NULL ,
	[Name] [varchar] (50) COLLATE Cyrillic_General_CI_AS NOT NULL ,
	[Code] [varbinary] (8000) NOT NULL 
) ON [PRIMARY]
END

GO

ALTER TABLE [dbo].[gisObjects] WITH NOCHECK ADD 
	CONSTRAINT [PK_Objects] PRIMARY KEY  CLUSTERED 
	(
		[Id]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[gisRanges] WITH NOCHECK ADD 
	CONSTRAINT [PK_Ranges] PRIMARY KEY  CLUSTERED 
	(
		[Id]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[gisTypes] WITH NOCHECK ADD 
	CONSTRAINT [PK_GTypes] PRIMARY KEY  CLUSTERED 
	(
		[Id]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[gisLayers] WITH NOCHECK ADD 
	CONSTRAINT [DF_Layers_Attr] DEFAULT (0) FOR [Attr]
GO

ALTER TABLE [dbo].[gisLib] WITH NOCHECK ADD 
	CONSTRAINT [DF_Lib_Attr] DEFAULT (0) FOR [Attr],
	CONSTRAINT [DF_gisLib_Scales] DEFAULT (0) FOR [Scales]
GO

ALTER TABLE [dbo].[gisViews] WITH NOCHECK ADD 
	CONSTRAINT [DF_Views_Attr] DEFAULT (0) FOR [Attr]
GO

 CREATE  INDEX [IX_Objects] ON [dbo].[gisObjects]([TypeId]) ON [PRIMARY]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE dbo.AddType
	@Id int,
	@ParentId int,
	@Name varchar(50),
	@Style varchar(200)
AS
begin
  insert into GTypes (Id,ParentId,Name,Style) values (@Id,@ParentId,@Name,@Style)
	RETURN 
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE dbo.[Stored Procedure1]
/*
	(
		@parameter1 datatype = default value,
		@parameter2 datatype OUTPUT
	)
*/
AS
	/* SET NOCOUNT ON */
	RETURN 

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE dbo.[_Stored Procedure1]
/*
	(
		@parameter1 datatype = default value,
		@parameter2 datatype OUTPUT
	)
*/
AS
	/* SET NOCOUNT ON */
	RETURN 

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

