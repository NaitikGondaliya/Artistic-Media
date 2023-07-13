USE [ArtisticMedia]
GO
/****** Object:  Table [dbo].[Tbl_Project]    Script Date: 7/11/2023 9:12:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Project](
	[ProjectId] [bigint] IDENTITY(1,1) NOT NULL,
	[ProjectName] [varchar](max) NOT NULL,
	[ProjectDate] [datetime] NOT NULL,
	[ProjectNumber] [int] NOT NULL,
 CONSTRAINT [PK_Tbl_Project] PRIMARY KEY CLUSTERED 
(
	[ProjectId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_ProjectAttachmentMapping]    Script Date: 7/11/2023 9:12:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_ProjectAttachmentMapping](
	[ProjectAttachmentMappingId] [bigint] IDENTITY(1,1) NOT NULL,
	[ProjectId] [bigint] NULL,
	[AttachmentName] [varchar](max) NULL,
	[AttachmentType] [int] NULL,
	[filename] [varchar](max) NULL,
 CONSTRAINT [PK_Tbl_ProjectAttachmentMapping] PRIMARY KEY CLUSTERED 
(
	[ProjectAttachmentMappingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_ReleaseDispo]    Script Date: 7/11/2023 9:12:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_ReleaseDispo](
	[ReleaseDispoId] [bigint] IDENTITY(1,1) NOT NULL,
	[DispoDate] [datetime] NULL,
	[AttachmentName] [varchar](max) NULL,
	[DispoDisc] [varchar](max) NULL,
	[filename] [varchar](max) NULL,
 CONSTRAINT [PK__Tbl_Rele__6297806816554921] PRIMARY KEY CLUSTERED 
(
	[ReleaseDispoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_User]    Script Date: 7/11/2023 9:12:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_User](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](50) NULL,
	[Password] [varchar](500) NULL,
 CONSTRAINT [PK_Tbl_User] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[AddNewAttachment]    Script Date: 7/11/2023 9:12:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[AddNewAttachment]  
(  
   @ProjectId bigint, 
   @attachmentType int,
   @AttachmentName varchar (max),
   @filename varchar (max)
)  
as  
begin  
   Insert into Tbl_ProjectAttachmentMapping ([ProjectId], [AttachmentName],[AttachmentType],[filename]) values(@ProjectId,@AttachmentName,@attachmentType,@filename)  
End 
GO
/****** Object:  StoredProcedure [dbo].[AddNewProject]    Script Date: 7/11/2023 9:12:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[AddNewProject]  
(  
   @ProjectNumber int,  
   @ProjectName varchar (max),  
   @ProjectDate datetime ,
   @id bigint output
)  
as  
begin  
   Insert into Tbl_Project ([ProjectName], [ProjectDate], [ProjectNumber]) values(@ProjectName,@ProjectDate,@ProjectNumber)  
   set @id=SCOPE_IDENTITY()
   return @id
End 
GO
/****** Object:  StoredProcedure [dbo].[AddReleaseDispo]    Script Date: 7/11/2023 9:12:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[AddReleaseDispo]  
(  
   @DispoDate DateTime,
   @DispoDesc varchar (max),
   @AttachmentName varchar (max),
   @filename varchar (max),
    @id bigint output
)  
as  
begin  
   Insert into Tbl_ReleaseDispo (DispoDate, [AttachmentName],DispoDisc,[filename]) values(@DispoDate,@AttachmentName,@DispoDesc,@filename) 
    set @id=SCOPE_IDENTITY()
   return @id
End 
GO
/****** Object:  StoredProcedure [dbo].[AuthenticateUser]    Script Date: 7/11/2023 9:12:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[AuthenticateUser]
@UserName varchar(50),
@Password varchar(500)
AS
BEGIN
	
	SET NOCOUNT ON;
	SELECT * from Tbl_User where UserName=@UserName and [Password]=@Password
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteAttachmentById]    Script Date: 7/11/2023 9:12:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure [dbo].[DeleteAttachmentById]  
(  
   @projectAttachmentMappingId bigint
)
as  
begin  
   delete from Tbl_ProjectAttachmentMapping  where  ProjectAttachmentMappingId=@projectAttachmentMappingId
End 
GO
/****** Object:  StoredProcedure [dbo].[DeleteProjectById]    Script Date: 7/11/2023 9:12:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[DeleteProjectById]  
(  
   @ProjectId bigint
)
as  
begin  
   delete from Tbl_Project  where  ProjectId=@ProjectId
   delete from Tbl_ProjectAttachmentMapping  where  ProjectId=@ProjectId
End 
GO
/****** Object:  StoredProcedure [dbo].[DeleteReleaseDispo]    Script Date: 7/11/2023 9:12:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create Procedure [dbo].[DeleteReleaseDispo]  
(  
   @dispoId bigint
)
as  
begin  
   delete from Tbl_ReleaseDispo  where  releasedispoId=@dispoId
End 
GO
/****** Object:  StoredProcedure [dbo].[GetProjectById]    Script Date: 7/11/2023 9:12:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[GetProjectById]  
(  
   @ProjectId bigint
)
as  
begin  
   select tp.*,tpa.ProjectAttachmentMappingId,tpa.AttachmentName,tpa.AttachmentType,tpa.[filename] from Tbl_Project  tp
   left join Tbl_ProjectAttachmentMapping tpa on tp.ProjectId=tpa.ProjectId
   where  tp.ProjectId=@ProjectId
   order by tpa.AttachmentType,tpa.ProjectAttachmentMappingId asc
End 
GO
/****** Object:  StoredProcedure [dbo].[GetProjects]    Script Date: 7/11/2023 9:12:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[GetProjects]  
as  
begin  
   select tp.*,tpm.AttachmentName,tpm.AttachmentType
   from Tbl_Project tp 
   left join Tbl_ProjectAttachmentMapping tpm on tp.ProjectId=tpm.ProjectId
   order by tpm.AttachmentType,tpm.ProjectAttachmentMappingId asc
End 
GO
/****** Object:  StoredProcedure [dbo].[GetProjectsAttachment]    Script Date: 7/11/2023 9:12:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[GetProjectsAttachment]  
(  
   @ProjectId bigint
)
as  
begin  
   select AttachmentName,[filename] from Tbl_ProjectAttachmentMapping where ProjectId=@ProjectId  
   order by AttachmentType,ProjectAttachmentMappingId asc
End 
GO
/****** Object:  StoredProcedure [dbo].[GetProjectsForDashboard]    Script Date: 7/11/2023 9:12:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[GetProjectsForDashboard]  
as  
begin  
     select tp.*,
   (select top 1  attachmentname from tbl_releasedispo trd where trd.dispodate=CAST(tp.projectdate as date) order by trd.ReleaseDispoId desc) as DispoAttachment,
   (select top 1  DispoDisc from tbl_releasedispo trd where trd.dispodate=CAST(tp.projectdate as date) order by trd.ReleaseDispoId desc) as DispoDisc,
   (select top 1  [filename] from tbl_releasedispo trd where trd.dispodate=CAST(tp.projectdate as date) order by trd.ReleaseDispoId desc) as DispoFileName
   from Tbl_Project tp
End 
GO
/****** Object:  StoredProcedure [dbo].[GetReleaseDispo]    Script Date: 7/11/2023 9:12:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[GetReleaseDispo]  
as  
begin  
  Select * from Tbl_ReleaseDispo order by DispoDate desc
End 
GO
/****** Object:  StoredProcedure [dbo].[ManageProject]    Script Date: 7/11/2023 9:12:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[ManageProject]  
(  
   @ProjectId int,  
   @ProjectNumber int,  
   @ProjectName varchar (max),  
   @ProjectDate datetime ,
   @id bigint output
)  
as  
begin  
IF(@ProjectId=0)
BEGIN
   Insert into Tbl_Project ([ProjectName], [ProjectDate], [ProjectNumber]) values(@ProjectName,@ProjectDate,@ProjectNumber)  
   set @id=SCOPE_IDENTITY()
   return @id
END
ELSE
Update Tbl_Project SET ProjectName=@ProjectName,ProjectDate=@ProjectDate,ProjectNumber=@ProjectNumber where ProjectId=@ProjectId
 set @id= @ProjectId;
  return @id
End 
GO
