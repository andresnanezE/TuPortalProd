USE [PROCESOS]
GO

/****** OBJECT:  TABLE [DBO].[EMA_LOG]    SCRIPT DATE: 26/02/2016 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [DBO].[EMA_LOG](
	[LOGID] [INT] IDENTITY(1,1) NOT NULL,
	[LLAVE] [NVARCHAR](250) NOT NULL,
	[DESCRIPCION] [NVARCHAR](512) NULL,
	[EXCEPCION] [NVARCHAR](1024) NULL,
	[PARAMETROXML] [NVARCHAR](1024) NULL,
 CONSTRAINT [PK_EMA_LOG] PRIMARY KEY CLUSTERED 
(
	[LOGID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


