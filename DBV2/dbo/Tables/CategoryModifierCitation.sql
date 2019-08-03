CREATE TABLE [dbo].[CategoryModifierCitation] (
    [CategoryId]        INT           NOT NULL,
    [CitationId]        INT           NOT NULL,
    [ModifierId]        INT           NOT NULL,
    [DirectionOfImpact] INT           CONSTRAINT [DF_CategoryModifierCitation_DirectionOfImpact] DEFAULT ((0)) NOT NULL,
    [AmountOfImpact]    FLOAT (53)    NOT NULL,
    [UsageInformation]  VARCHAR (MAX) NULL,
    CONSTRAINT [PK_CategoryModifierCitation] PRIMARY KEY CLUSTERED ([CategoryId] ASC, [CitationId] ASC, [ModifierId] ASC),
    CONSTRAINT [FK_CategoryModifierCitation_CategoryReference] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[CategoryReference] ([CategoryId]),
    CONSTRAINT [FK_CategoryModifierCitation_Citation] FOREIGN KEY ([CitationId]) REFERENCES [dbo].[Citation] ([CitationId]),
    CONSTRAINT [FK_CategoryModifierCitation_Modifiers] FOREIGN KEY ([ModifierId]) REFERENCES [dbo].[Modifiers] ([ModifierId])
);

