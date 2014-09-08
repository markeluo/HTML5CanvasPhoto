CREATE TABLE [Album] (
  [ID] integer NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, 
  [AlbumNo] NVARCHAR(50), 
  [AlbumName] NVARCHAR(50), 
  [AlbumRootPath] NVARCHAR(100), 
  [AlbumRemark] NVARCHAR(100), 
  [AlbumTitleImg] NVARCHAR(200), 
  [CreateDate] NVARCHAR(50), 
  [UpdateDate] NVARCHAR(50));


CREATE TABLE [Photo] (
  [ID] integer NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, 
  [PhotoNO] NVARCHAR(50), 
  [AlbumNo] VARCHAR(50), 
  [PhotoName] NVARCHAR(50), 
  [PhotoRemark] NVARCHAR(100), 
  [IsTitleImg] INT, 
  [PhotoData] NVARCHAR(200), 
  [PhotoMiniData] NVARCHAR(200), 
  [PhotoCreateDate] VARCHAR(50), 
  [PhotoUpdateDate] VARCHAR(50));