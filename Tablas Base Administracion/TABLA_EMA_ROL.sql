USE [PROCESOS]
GO

/****** OBJECT:  TABLE [DBO].[EMA_ROL]    SCRIPT DATE: 26/02/2016 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [DBO].[EMA_ROL](
	[ROLID] [INT] IDENTITY(1,1) NOT NULL,
	[ROL] [NVARCHAR](50) NOT NULL,
	[FECHACREACION] [DATETIME] NOT NULL CONSTRAINT [DF_EMA_ROLES_FECHACREACION]  DEFAULT (GETDATE()),
	[ACTIVO] [BIT] NOT NULL,
	[USUARIOCREACION] [NVARCHAR](50) NULL,
	[USUARIOMODIFICACION] [NVARCHAR](50) NULL,
 CONSTRAINT [PK_EMA_ROLES] PRIMARY KEY CLUSTERED 
(
	[ROLID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


