CREATE DATABASE [ArtisticMedia]
USE [ArtisticMedia]
GO
/****** Object:  Table [dbo].[Tbl_Project]    Script Date: 5/19/2023 3:09:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Project](
	[ProjectId] [bigint] IDENTITY(1,1) NOT NULL,
	[ProjectName] [varchar](100) NOT NULL,
	[ProjectDate] [datetime] NOT NULL,
	[ProjectNumber] [int] NOT NULL,
 CONSTRAINT [PK_Tbl_Project] PRIMARY KEY CLUSTERED 
(
	[ProjectId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_ProjectAttachmentMapping]    Script Date: 5/19/2023 3:09:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_ProjectAttachmentMapping](
	[ProjectAttachmentMappingId] [bigint] IDENTITY(1,1) NOT NULL,
	[ProjectId] [bigint] NULL,
	[AttachmentName] [varchar](max) NULL,
 CONSTRAINT [PK_Tbl_ProjectAttachmentMapping] PRIMARY KEY CLUSTERED 
(
	[ProjectAttachmentMappingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[AddNewAttachment]    Script Date: 5/19/2023 3:09:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[AddNewAttachment]  
(  
   @ProjectId bigint,  
   @AttachmentName varchar (max)
)  
as  
begin  
   Insert into Tbl_ProjectAttachmentMapping ([ProjectId], [AttachmentName]) values(@ProjectId,@AttachmentName)  
End 
GO
/****** Object:  StoredProcedure [dbo].[AddNewProject]    Script Date: 5/19/2023 3:09:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[AddNewProject]  
(  
   @ProjectNumber int,  
   @ProjectName varchar (100),  
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
/****** Object:  StoredProcedure [dbo].[DeleteProjectById]    Script Date: 5/19/2023 3:09:50 PM ******/
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
/****** Object:  StoredProcedure [dbo].[GetProjects]    Script Date: 5/19/2023 3:09:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure [dbo].[GetProjects]  
as  
begin  
   select * from Tbl_Project  
End 
GO
/****** Object:  StoredProcedure [dbo].[GetProjectsAttachment]    Script Date: 5/19/2023 3:09:50 PM ******/
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
   select AttachmentName from Tbl_ProjectAttachmentMapping where ProjectId=@ProjectId  
End 
GO
USE [master]
GO
ALTER DATABASE [ArtisticMedia] SET  READ_WRITE 
GO
