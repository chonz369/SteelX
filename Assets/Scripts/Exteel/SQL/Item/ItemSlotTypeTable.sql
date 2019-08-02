CREATE TABLE ItemSlotType(
    SlotType int PRIMARY KEY CLUSTERED
,   [Description] varchar(24)
,   Category varchar(24) 
);
GO

INSERT INTO ItemSlotType( SlotType, [Description], Category )
VALUES (0, 'No restriction', 'NO' );
INSERT INTO ItemSlotType( SlotType, [Description], Category )
VALUES (1, 'Melee', 'Weapon' );
INSERT INTO ItemSlotType( SlotType, [Description], Category )
VALUES (2, 'Range', 'Weapon' );
INSERT INTO ItemSlotType( SlotType, [Description], Category )
VALUES (3, 'Item', 'Special item' );
INSERT INTO ItemSlotType( SlotType, [Description], Category )
VALUES (4, 'Head', 'Part' );
INSERT INTO ItemSlotType( SlotType, [Description], Category )
VALUES (5, 'Core', 'Part' );
INSERT INTO ItemSlotType( SlotType, [Description], Category )
VALUES (6, 'Arms', 'Part' );
INSERT INTO ItemSlotType( SlotType, [Description], Category )
VALUES (7, 'Legs', 'Part' );
INSERT INTO ItemSlotType( SlotType, [Description], Category )
VALUES (9, 'Booster', 'Part' );
INSERT INTO ItemSlotType( SlotType, [Description], Category )
VALUES (10, 'SetItem', 'SetItem' );
GO