CREATE VIEW GetData AS
SELECT 
[node_id] as [place_id], ISNULL(ISNULL([name], [name_bg]), [name_en]) as [name_bg], [name_en],
location,
 (
 CASE
 WHEN [tourism] IS NOT NULL
	THEN [tourism]
 WHEN [shop] IS NOT NULL
	THEN [shop]
 WHEN [network] IS NOT NULL
	THEN [network]
 WHEN [amenity] IS NOT NULL
	THEN [amenity]
WHEN [aeroway] IS NOT NULL
	THEN [aeroway]
WHEN [highway] IS NOT NULL
	THEN [highway]
WHEN [place] IS NOT NULL
	THEN [place]
WHEN [natural] IS NOT NULL
	THEN [natural]
WHEN [railway] IS NOT NULL
	THEN [railway]
WHEN [trafficsign] IS NOT NULL
	THEN [trafficsign]
WHEN [manmade] IS NOT NULL
	THEN [manmade]
WHEN [junction] IS NOT NULL
	THEN 'junction'
WHEN [office] IS NOT NULL
	THEN [office]
WHEN [disusedrailway] IS NOT NULL
	THEN [disusedrailway]
WHEN [information] IS NOT NULL
	THEN [information]
WHEN [leisure] IS NOT NULL
	THEN [leisure]
WHEN [historic] IS NOT NULL
	THEN [historic]
WHEN [aerialway] IS NOT NULL
	THEN [aerialway]
WHEN [building] IS NOT NULL
	THEN [building]
WHEN [landuse] IS NOT NULL
	THEN [landuse]
WHEN [entrance] IS NOT NULL
	THEN 'entrance'
WHEN [power] IS NOT NULL
	THEN [power]
WHEN [waterway] IS NOT NULL
	THEN [waterway]
WHEN [geological] IS NOT NULL
	THEN [geological]
WHEN [sport] IS NOT NULL
	THEN [sport]
WHEN [barrier] IS NOT NULL
	THEN [barrier]
WHEN [craft] IS NOT NULL
	THEN [craft]
WHEN [abandonedamenity] IS NOT NULL
	THEN [abandonedamenity]
 ELSE 'general'
 END
 ) AS [place_type],
 	(
	CASE 
		WHEN [addr_street] IS NOT NULL
		THEN
		[addr_street] + ' ' + ISNULL([addr_housenumber], '')
		ELSE NULL
	END
	) AS [address],
 [phone], [website],
 [note]
 FROM
 (
	 SELECT NodeId as [node_id], [name] AS name, [nameen] as [name_en], [namebg] as [name_bg],
	 [note],
	 [addrstreet] AS [addr_street], [addrhousenumber] AS [addr_housenumber],
	 [contactphone] as [phone], [website],
	 [amenity], [shop], [tourism], [network], [aeroway], [highway], [place], [natural], [railway], [trafficsign], [manmade], [junction], [office],
	 [disusedrailway], [information], [leisure], [historic], [aerialway], [building],
	 [landuse], [entrance], [power], [waterway], [geological], [sport], [barrier],
	 [craft], [abandonedamenity]
		FROM 
		(
		  SELECT N.Id as NodeId, 
		  NT.Info as TypeInfo, 
		  REPLACE(REPLACE(TT.Name, '_', ''), ':', '') as TypeName
		  FROM [dbo].[tNode] as N
		  INNER JOIN [dbo].[tNodeTag] NT
		  ON NT.NodeId = N.Id
		  INNER JOIN dbo.tTagType TT
		  ON TT.Typ=NT.Typ
		) ps
		PIVOT
		(
			MIN(TypeInfo)
			FOR TypeName IN
			( [name], [nameen], [namebg], 
			[note],
			[addrstreet], [addrhousenumber],
			[contactphone], [website],
			[amenity], [shop], [tourism], [network], [aeroway], [highway], [place], [natural], [railway], [trafficsign], [manmade], [junction], [office],
			[disusedrailway], [information], [leisure], [historic], [aerialway], [building],
			[landuse], [entrance], [power], [waterway], [geological], [sport], [barrier],
			[craft], [abandonedamenity]
			)
		) AS pvt
		WHERE NOT(name IS NULL AND [nameen] IS NULL AND [namebg] IS NULL)
) AS Denormalized
INNER JOIN [dbo].[tNode] as N
ON Denormalized.[node_id] = N.Id