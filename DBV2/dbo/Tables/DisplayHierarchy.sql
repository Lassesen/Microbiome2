CREATE TABLE [dbo].[DisplayHierarchy] (
    [DisplayGroupId]  INT NOT NULL,
    [DisplayParentId] INT NOT NULL,
    CONSTRAINT [PK_DisplayHierarchy] PRIMARY KEY CLUSTERED ([DisplayGroupId] ASC, [DisplayParentId] ASC),
    CONSTRAINT [FK_DisplayHierarchy_DisplayGroup] FOREIGN KEY ([DisplayGroupId]) REFERENCES [dbo].[DisplayGroup] ([DisplayGroupId]),
    CONSTRAINT [FK_DisplayHierarchy_DisplayGroup1] FOREIGN KEY ([DisplayParentId]) REFERENCES [dbo].[DisplayGroup] ([DisplayGroupId])
);

