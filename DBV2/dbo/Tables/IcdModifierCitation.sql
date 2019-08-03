CREATE TABLE [dbo].[IcdModifierCitation] (
    [ICDCode]           VARCHAR (16)  NOT NULL,
    [CitationId]        INT           NOT NULL,
    [ModifierId]        INT           NOT NULL,
    [DirectionOfImpact] INT           CONSTRAINT [DF_IcdModifierCitation_DirectionOfImpact] DEFAULT ((0)) NOT NULL,
    [AmountOfImpact]    FLOAT (53)    NOT NULL,
    [UsageInformation]  VARCHAR (MAX) NULL,
    CONSTRAINT [PK_IcdModifierCitation] PRIMARY KEY CLUSTERED ([ICDCode] ASC, [CitationId] ASC, [ModifierId] ASC),
    CONSTRAINT [FK_IcdModifierCitation_Citation] FOREIGN KEY ([CitationId]) REFERENCES [dbo].[Citation] ([CitationId]),
    CONSTRAINT [FK_IcdModifierCitation_ICDCode] FOREIGN KEY ([ICDCode]) REFERENCES [dbo].[ICDCode] ([Code]),
    CONSTRAINT [FK_IcdModifierCitation_Modifiers] FOREIGN KEY ([ModifierId]) REFERENCES [dbo].[Modifiers] ([ModifierId])
);

