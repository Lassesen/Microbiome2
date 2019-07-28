CREATE TABLE [dbo].[CategoryDisplay] (
    [DisplayGroupId] INT        NOT NULL,
    [CategoryId]     INT        NOT NULL,
    [DisplayOrder]   FLOAT (53) CONSTRAINT [DF_CategoryDisplay_DisplayOrder] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_CategoryDisplay] PRIMARY KEY CLUSTERED ([DisplayGroupId] ASC, [CategoryId] ASC),
    CONSTRAINT [FK_CategoryDisplay_CategoryReference] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[CategoryReference] ([CategoryId]),
    CONSTRAINT [FK_CategoryDisplay_DisplayGroup] FOREIGN KEY ([DisplayGroupId]) REFERENCES [dbo].[DisplayGroup] ([DisplayGroupId])
);

