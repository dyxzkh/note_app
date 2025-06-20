USE [master]
GO
/****** Object:  Database [note_app]    Script Date: 6/8/2025 1:27:19 PM ******/
CREATE DATABASE [note_app]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'note_app', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\note_app.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'note_app_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\note_app_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [note_app] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [note_app].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [note_app] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [note_app] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [note_app] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [note_app] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [note_app] SET ARITHABORT OFF 
GO
ALTER DATABASE [note_app] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [note_app] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [note_app] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [note_app] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [note_app] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [note_app] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [note_app] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [note_app] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [note_app] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [note_app] SET  DISABLE_BROKER 
GO
ALTER DATABASE [note_app] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [note_app] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [note_app] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [note_app] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [note_app] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [note_app] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [note_app] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [note_app] SET RECOVERY FULL 
GO
ALTER DATABASE [note_app] SET  MULTI_USER 
GO
ALTER DATABASE [note_app] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [note_app] SET DB_CHAINING OFF 
GO
ALTER DATABASE [note_app] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [note_app] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [note_app] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [note_app] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'note_app', N'ON'
GO
ALTER DATABASE [note_app] SET QUERY_STORE = ON
GO
ALTER DATABASE [note_app] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [note_app]
GO
/****** Object:  Table [dbo].[Notes]    Script Date: 6/8/2025 1:27:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[Content] [nvarchar](255) NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[CreatedBy] [int] NULL,
 CONSTRAINT [Notes_PK] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 6/8/2025 1:27:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](100) NOT NULL,
	[RoleDescription] [nvarchar](255) NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NULL,
 CONSTRAINT [Roles_PK] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 6/8/2025 1:27:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Fullname] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[Password] [varchar](255) NOT NULL,
	[RefreshToken] [varchar](255) NULL,
	[RefreshTokenExpiry] [datetime] NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[RoleId] [int] NOT NULL,
 CONSTRAINT [User_PK] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Notes] ON 

INSERT [dbo].[Notes] ([Id], [Title], [Content], [CreatedAt], [UpdatedAt], [CreatedBy]) VALUES (1, N'Testing 1', NULL, CAST(N'2025-06-07T12:06:36.340' AS DateTime), NULL, 9)
INSERT [dbo].[Notes] ([Id], [Title], [Content], [CreatedAt], [UpdatedAt], [CreatedBy]) VALUES (2, N'Testing 2', NULL, CAST(N'2025-06-07T12:06:53.383' AS DateTime), NULL, 9)
INSERT [dbo].[Notes] ([Id], [Title], [Content], [CreatedAt], [UpdatedAt], [CreatedBy]) VALUES (3, N'Testing 4', NULL, CAST(N'2025-06-07T12:09:20.800' AS DateTime), NULL, 9)
INSERT [dbo].[Notes] ([Id], [Title], [Content], [CreatedAt], [UpdatedAt], [CreatedBy]) VALUES (52, N'sdfasdfdsaf', N'dfsadfs dsfasdfasdf', CAST(N'2025-06-08T13:13:15.827' AS DateTime), CAST(N'2025-06-08T13:13:21.637' AS DateTime), 10)
INSERT [dbo].[Notes] ([Id], [Title], [Content], [CreatedAt], [UpdatedAt], [CreatedBy]) VALUES (53, N'dfasdf', N'dsfasdfdsfa', CAST(N'2025-06-08T13:13:18.260' AS DateTime), NULL, 10)
INSERT [dbo].[Notes] ([Id], [Title], [Content], [CreatedAt], [UpdatedAt], [CreatedBy]) VALUES (54, N'dfsasdf', N'sdfasdfasdf', CAST(N'2025-06-08T13:13:25.693' AS DateTime), NULL, 10)
INSERT [dbo].[Notes] ([Id], [Title], [Content], [CreatedAt], [UpdatedAt], [CreatedBy]) VALUES (55, N'dfasdf', N'sdfasdfsdf dfsadfsdf dssdfasdfsdf', CAST(N'2025-06-08T13:13:28.240' AS DateTime), CAST(N'2025-06-08T13:18:15.300' AS DateTime), 10)
INSERT [dbo].[Notes] ([Id], [Title], [Content], [CreatedAt], [UpdatedAt], [CreatedBy]) VALUES (56, N'sdfasdf', N'sdfasdf', CAST(N'2025-06-08T13:18:18.103' AS DateTime), CAST(N'2025-06-08T13:26:11.080' AS DateTime), 10)
SET IDENTITY_INSERT [dbo].[Notes] OFF
GO
SET IDENTITY_INSERT [dbo].[Roles] ON 

INSERT [dbo].[Roles] ([Id], [RoleName], [RoleDescription], [CreatedAt], [UpdatedAt]) VALUES (1, N'Admin', N'Administrator role', CAST(N'2025-06-06T11:33:24.967' AS DateTime), NULL)
INSERT [dbo].[Roles] ([Id], [RoleName], [RoleDescription], [CreatedAt], [UpdatedAt]) VALUES (2, N'User', N'Regular user role', CAST(N'2025-06-06T11:33:24.967' AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[Roles] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([Id], [Fullname], [Email], [Password], [RefreshToken], [RefreshTokenExpiry], [CreatedAt], [UpdatedAt], [RoleId]) VALUES (4, N'Admin', N'admin@example.com', N'$2a$12$Q2j99Z8yRCmZcKvqipi2YejwtF3tATCtKYvj88YE6ukSvgRbDTvY2', N'lg4O8351mH4ozJvz+v1FO8B4AB9EUUb2iCXSQ9hwZBcIfJ7vn6j6ktD+p+aRyp6e141mG3kYzh8RezDE51pcMg==', CAST(N'2025-06-14T20:22:36.313' AS DateTime), CAST(N'2025-06-06T11:33:25.373' AS DateTime), NULL, 1)
INSERT [dbo].[Users] ([Id], [Fullname], [Email], [Password], [RefreshToken], [RefreshTokenExpiry], [CreatedAt], [UpdatedAt], [RoleId]) VALUES (9, N'Sovandy Khgney', N'sovandykh168@gmai.com', N'$2a$12$bcSeoWRvweooKKv2NS3b/OtfbUHdq7nnIFBDeuHPJC3uxjf51Rnpa', NULL, NULL, CAST(N'2025-06-06T13:12:00.040' AS DateTime), NULL, 1)
INSERT [dbo].[Users] ([Id], [Fullname], [Email], [Password], [RefreshToken], [RefreshTokenExpiry], [CreatedAt], [UpdatedAt], [RoleId]) VALUES (10, N'123123', N'sovandykh168@gmail.com', N'$2a$12$GU8lfy1VhFaF7ZQda3T2.OIVe1UYZ4XG/DREaPisNANpjoNfCgRS2', N'WRyrwgpp251vIRFgmtV0BlY3QlT/Cy7l/nONOCgtAIVDmxwy+rSpPnXl8/+7TYxW2wCzhDXQjgcruThX3kvd5g==', CAST(N'2025-06-15T06:22:02.927' AS DateTime), CAST(N'2025-06-07T20:40:23.180' AS DateTime), NULL, 2)
INSERT [dbo].[Users] ([Id], [Fullname], [Email], [Password], [RefreshToken], [RefreshTokenExpiry], [CreatedAt], [UpdatedAt], [RoleId]) VALUES (11, N'df', N'admin@gmail.com', N'$2a$12$2/6RwVS5HYImA/WgbzbvT.MBZ.Qomfh4x.7BytoHL0IK6lOhudqda', N'RBjJi1wtp5JndjLcOkPjQyMTeqjqFZlYtV7BuaPvNfgmbb9ctQKiISSnf3IbJpeCmvTIcfbbDlWvIXk2ZAqS+Q==', CAST(N'2025-06-14T20:50:25.033' AS DateTime), CAST(N'2025-06-07T20:50:13.257' AS DateTime), NULL, 2)
INSERT [dbo].[Users] ([Id], [Fullname], [Email], [Password], [RefreshToken], [RefreshTokenExpiry], [CreatedAt], [UpdatedAt], [RoleId]) VALUES (12, N'4534534', N'admin@samborpreikuk.gov.kh', N'$2a$12$eGmkh8.pW1mlVG8Hr3dJy.7NziznELWYNbRxvyuehnNoIIZKgJ4j6', N'Ff8+UIaSGuE0Kj8ntLHyi5JFZ6S1uRpOdHrFjVmaGM8uwIlpZnYYmDJqgaW+4qBr9ACSjvNltIdb9DhnVkly0w==', CAST(N'2025-06-14T20:52:40.957' AS DateTime), CAST(N'2025-06-07T20:52:30.157' AS DateTime), NULL, 2)
INSERT [dbo].[Users] ([Id], [Fullname], [Email], [Password], [RefreshToken], [RefreshTokenExpiry], [CreatedAt], [UpdatedAt], [RoleId]) VALUES (13, N'123123', N'123123s@gmail.com', N'$2a$12$GGo4ev5XINTVcMCzBjNPbOVxYNUVD6hI/1.uf.int.n9RCXQQHPQe', NULL, NULL, CAST(N'2025-06-08T06:20:07.017' AS DateTime), NULL, 2)
INSERT [dbo].[Users] ([Id], [Fullname], [Email], [Password], [RefreshToken], [RefreshTokenExpiry], [CreatedAt], [UpdatedAt], [RoleId]) VALUES (14, N'123123', N'sovandykh1638@gmail.com', N'$2a$12$Efhp0qfvhqol.K49AULhHuUgE0qKaZNIMk/07QU9IUerhDvB1c.xu', NULL, NULL, CAST(N'2025-06-08T06:21:58.617' AS DateTime), NULL, 2)
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Users_Email]    Script Date: 6/8/2025 1:27:20 PM ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [UQ_Users_Email] UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Notes]  WITH CHECK ADD  CONSTRAINT [Notes_CreatedBy_FK] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Notes] CHECK CONSTRAINT [Notes_CreatedBy_FK]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [Users_RoleId_FK] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [Users_RoleId_FK]
GO
USE [master]
GO
ALTER DATABASE [note_app] SET  READ_WRITE 
GO
