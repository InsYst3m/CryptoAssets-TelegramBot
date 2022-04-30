USE [db_a86115_cryptoportfolio]
GO

INSERT INTO [dbo].[Users] ([Email]) VALUES ('test@test.com')
GO

INSERT [dbo].[CryptoAssets] ([Name], [Abbreviation], [CoinGeckoAbbreviation]) VALUES (N'bitcoin', N'btc', N'bitcoin')
GO
INSERT [dbo].[CryptoAssets] ([Name], [Abbreviation], [CoinGeckoAbbreviation]) VALUES (N'ethereum', N'eth', N'ethereum')
GO
INSERT [dbo].[CryptoAssets] ([Name], [Abbreviation], [CoinGeckoAbbreviation]) VALUES (N'ripple', N'xrp', N'ripple')
GO
INSERT [dbo].[CryptoAssets] ([Name], [Abbreviation], [CoinGeckoAbbreviation]) VALUES (N'polkadot', N'dot', N'polkadot')
GO
INSERT [dbo].[CryptoAssets] ([Name], [Abbreviation], [CoinGeckoAbbreviation]) VALUES (N'near protocol', N'near', N'near')
GO
INSERT [dbo].[CryptoAssets] ([Name], [Abbreviation], [CoinGeckoAbbreviation]) VALUES (N'stellar', N'xlm', N'stellar')
GO
INSERT [dbo].[CryptoAssets] ([Name], [Abbreviation], [CoinGeckoAbbreviation]) VALUES (N'umee', N'umee', N'umee')
GO
INSERT [dbo].[CryptoAssets] ([Name], [Abbreviation], [CoinGeckoAbbreviation]) VALUES (N'chia', N'xch', N'chia')
GO

INSERT INTO [dbo].[UsersCryptoAssets] ([UsersId], [CryptoAssetsId]) VALUES (1, 1)
GO
INSERT INTO [dbo].[UsersCryptoAssets] ([UsersId], [CryptoAssetsId]) VALUES (1, 2)
GO
INSERT INTO [dbo].[UsersCryptoAssets] ([UsersId], [CryptoAssetsId]) VALUES (1, 3)
GO
INSERT INTO [dbo].[UsersCryptoAssets] ([UsersId], [CryptoAssetsId]) VALUES (1, 4)
GO
INSERT INTO [dbo].[UsersCryptoAssets] ([UsersId], [CryptoAssetsId]) VALUES (1, 5)
GO
INSERT INTO [dbo].[UsersCryptoAssets] ([UsersId], [CryptoAssetsId]) VALUES (1, 6)
GO
INSERT INTO [dbo].[UsersCryptoAssets] ([UsersId], [CryptoAssetsId]) VALUES (1, 7)
GO
INSERT INTO [dbo].[UsersCryptoAssets] ([UsersId], [CryptoAssetsId]) VALUES (1, 8)
GO
