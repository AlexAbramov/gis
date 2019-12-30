CREATE TABLE gisBg
(
	Id int NOT NULL PRIMARY KEY,
	Attr int NOT NULL,
	FilePath varchar(500) NOT NULL,
	SMin int NOT NULL,
	SMax int NOT NULL,
	Opacity int NOT NULL,
	Code varbinary(100) NOT NULL 
)

CREATE TABLE gisColors
(
	Id int NOT NULL PRIMARY KEY,
	Name varchar(50) NOT NULL,
	Val int NOT NULL 
)


CREATE TABLE gisLayers
(
	Id int  NOT NULL PRIMARY KEY,
	Attr int NOT NULL,
	Name varchar(50) NOT NULL ,
	Code varbinary(8000) NOT NULL 
)

CREATE TABLE gisLib(
	Id int NOT NULL PRIMARY KEY,
	Attr int NOT NULL ,
	Name varchar(50) NOT NULL ,
	Style varchar(100) NOT NULL ,
	DefaultStyle varchar(100) NOT NULL ,
	SMin int NOT NULL ,
	SMax int NOT NULL ,
	Code binary(16) NOT NULL ,
	IndexerCode varbinary(100) NOT NULL ,
	Scales varbinary(1000) NOT NULL 
)

CREATE TABLE gisObjects(
	Id int  NOT NULL PRIMARY KEY,
	TypeId int NOT NULL ,
	RangeId int NOT NULL ,
	Attr int NOT NULL ,
	Name varchar(50) NOT NULL ,
	Caption varchar(50) NOT NULL ,
	Style varchar(100) NOT NULL ,
	TextAttr varchar(100) NOT NULL ,
	Code varbinary(8000) NOT NULL 
)



CREATE TABLE gisRanges(
	Id int NOT NULL PRIMARY KEY,
	TypeId int NOT NULL ,
	Code binary(16) NOT NULL 
)




CREATE TABLE gisTypes(
	Id int NOT NULL PRIMARY KEY,
	ParentId int NOT NULL,
	Priority int NOT NULL,
	Attr int NOT NULL,
	Name varchar(50) NOT NULL,
	Style varchar(100) NOT NULL,
	GeomType int NOT NULL,
	SMin int NOT NULL,
	SMax int NOT NULL 
)

CREATE TABLE gisViews
(
	Id int NOT NULL PRIMARY KEY,
	Attr int NOT NULL ,
	Name varchar(50) NOT NULL ,
	Code varbinary(8000) NOT NULL 
)